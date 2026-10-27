import { createContext, useCallback, useContext, useEffect, useMemo, useRef } from "react";
import { useQueryClient } from "@tanstack/react-query";
import type { QueryClient } from "@tanstack/react-query";
import { HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import type { HubConnection } from "@microsoft/signalr";

import { API_BASE_URL } from "../../../shared/lib/api";
import { conversationsService } from "../services/conversations.service";
import { useConversations } from "./useConversations";
import type { Conversation, Message, RealtimeMessageDTO } from "../types/conversation";

const CHAT_HUB_URL = `${API_BASE_URL}/hubs/chathub`;
const RECEIVE_MESSAGE_EVENT = "ReceiveMessage";
const RECEIVE_CONVERSATION_EVENT = "ReceiveConversation";
const JOIN_CONVERSATION_METHOD = "JoinConversation";
const START_RETRY_DELAY_MS = 5_000;

function containsMessage(messages: Message[], candidate: Message): boolean {
    return messages.some(
        (m) =>
            m.senderType === candidate.senderType &&
            m.text === candidate.text &&
            m.createdAt.getTime() === candidate.createdAt.getTime(),
    );
}

function withAppendedMessage(conversation: Conversation, message: Message): Conversation {
    return {
        ...conversation,
        messages: [...conversation.messages, message],
        lastMessageAt: message.createdAt,
    };
}

function withConversationRead(conversation: Conversation): Conversation {
    return {
        ...conversation,
        messages: conversation.messages.map((m) =>
            m.senderType === "customer" && !m.isRead ? { ...m, isRead: true } : m,
        ),
    };
}

function markConversationAsReadInCache(qc: QueryClient, conversationId: string): void {
    const listEntries = qc.getQueriesData<Conversation[]>({ queryKey: ["conversations"] });
    for (const [queryKey, conversations] of listEntries) {
        if (!conversations) continue;
        const index = conversations.findIndex((c) => c.id === conversationId);
        if (index === -1) continue;

        const updated = [...conversations];
        updated[index] = withConversationRead(conversations[index]);
        qc.setQueryData(queryKey, updated);
    }

    const detail = qc.getQueryData<Conversation>(["conversation", conversationId]);
    if (detail) {
        qc.setQueryData(["conversation", conversationId], withConversationRead(detail));
    }
}

async function markMessagesAsRead(conversationId: string): Promise<void> {
    await conversationsService.markMessagesAsRead(conversationId).catch((error: unknown) => {
        if (import.meta.env.DEV) {
            console.error("[realtime] falha ao marcar mensagens como lidas", conversationId, error);
        }
    });
}

async function joinConversations(
    connection: HubConnection,
    conversationIds: string[],
    joinedIds: Set<string>,
): Promise<void> {
    const pendingIds = conversationIds.filter((id) => !joinedIds.has(id));
    pendingIds.forEach((id) => joinedIds.add(id));

    await Promise.all(
        pendingIds.map((id) =>
            connection.invoke(JOIN_CONVERSATION_METHOD, id).catch((error: unknown) => {
                joinedIds.delete(id);
                if (import.meta.env.DEV) {
                    console.error("[realtime] falha ao entrar na conversa", id, error);
                }
            }),
        ),
    );
}

export interface ConversationsRealtimeContextValue {
    setSelectedConversation: (id: string | null) => void;
    unreadConversations: number;
}

export const ConversationsRealtimeContext = createContext<ConversationsRealtimeContextValue | null>(null);

export function useConversationsRealtimeCore(): ConversationsRealtimeContextValue {
    const qc = useQueryClient();
    const { data } = useConversations("all");
    const conversationIds = useMemo(
        () => data?.map((c) => c.id) ?? [],
        [data],
    );

    const connectionRef = useRef<HubConnection | null>(null);
    const conversationIdsRef = useRef<string[]>(conversationIds);
    const selectedIdRef = useRef<string | null>(null);
    const joinedIdsRef = useRef<Set<string>>(new Set());

    useEffect(() => {
        const joinedIds = new Set<string>();
        joinedIdsRef.current = joinedIds;

        const connection = new HubConnectionBuilder()
            .withUrl(CHAT_HUB_URL, { withCredentials: false })
            .withAutomaticReconnect()
            .build();
        connectionRef.current = connection;

        connection.on(RECEIVE_MESSAGE_EVENT, (dto: RealtimeMessageDTO) => {
            let message = conversationsService.mapRealtimeMessage(dto);

            if (message.conversationId === selectedIdRef.current && message.senderType === "customer") {
                message = { ...message, isRead: true };
                void markMessagesAsRead(message.conversationId);
            }

            const listEntries = qc.getQueriesData<Conversation[]>({ queryKey: ["conversations"] });
            let knownConversation = false;

            for (const [queryKey, conversations] of listEntries) {
                if (!conversations) continue;
                const index = conversations.findIndex((c) => c.id === message.conversationId);
                if (index === -1) continue;

                knownConversation = true;
                const conversation = conversations[index];
                if (containsMessage(conversation.messages, message)) continue;

                const updated = [...conversations];
                updated[index] = withAppendedMessage(conversation, message);
                qc.setQueryData(queryKey, updated);
            }

            const detail = qc.getQueryData<Conversation>(["conversation", message.conversationId]);
            if (detail && !containsMessage(detail.messages, message)) {
                qc.setQueryData(["conversation", message.conversationId], withAppendedMessage(detail, message));
            }

            if (!knownConversation) {
                void qc.invalidateQueries({ queryKey: ["conversations"] });
            }
        });

        connection.on(RECEIVE_CONVERSATION_EVENT, () => {
            void qc.invalidateQueries({ queryKey: ["conversations"] });
        });

        connection.onreconnected(async () => {
            joinedIds.clear();
            void joinConversations(connection, conversationIdsRef.current, joinedIds);

            if (selectedIdRef.current) {
                await markMessagesAsRead(selectedIdRef.current);
            }
            void qc.invalidateQueries({ queryKey: ["conversations"] });
        });

        let stopped = false;
        let retryTimer: ReturnType<typeof setTimeout> | undefined;

        const startConnection = () => {
            connection
                .start()
                .then(() => {
                    if (stopped) return;
                    void joinConversations(connection, conversationIdsRef.current, joinedIds);
                    if (selectedIdRef.current) {
                        void markMessagesAsRead(selectedIdRef.current);
                    }
                })
                .catch((error: unknown) => {
                    if (stopped) return;
                    if (import.meta.env.DEV) {
                        console.error("[realtime] falha ao conectar ao hub, nova tentativa em", START_RETRY_DELAY_MS, error);
                    }
                    retryTimer = setTimeout(startConnection, START_RETRY_DELAY_MS);
                });
        };

        startConnection();

        return () => {
            stopped = true;
            if (retryTimer !== undefined) clearTimeout(retryTimer);
            connectionRef.current = null;
            connection.stop().catch(() => undefined);
        };
    }, [qc]);

    useEffect(() => {
        conversationIdsRef.current = conversationIds;

        const connection = connectionRef.current;
        if (connection && connection.state === HubConnectionState.Connected) {
            void joinConversations(connection, conversationIds, joinedIdsRef.current);
        }
    }, [conversationIds]);

    const setSelectedConversation = useCallback(
        (id: string | null) => {
            selectedIdRef.current = id;
            if (!id) return;
            void markMessagesAsRead(id);
            markConversationAsReadInCache(qc, id);
        },
        [qc],
    );

    const unreadConversations = (data ?? []).filter(
        (conversation) =>
            conversation.messages.some((m) => m.senderType === "customer" && !m.isRead),
    ).length;

    return { setSelectedConversation, unreadConversations };
}

export function useConversationsRealtime(): ConversationsRealtimeContextValue {
    const context = useContext(ConversationsRealtimeContext);
    if (!context) {
        throw new Error("useConversationsRealtime deve ser usado dentro de ConversationsRealtimeProvider");
    }
    return context;
}
