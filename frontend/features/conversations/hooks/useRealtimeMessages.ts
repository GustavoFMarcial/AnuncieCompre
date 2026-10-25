import { useEffect, useRef } from "react";
import { useQueryClient } from "@tanstack/react-query";
import { HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import type { HubConnection } from "@microsoft/signalr";

import { API_BASE_URL } from "../../../shared/lib/api";
import { conversationsService } from "../services/conversations.service";
import type { Conversation, Message, RealtimeMessageDTO } from "../types/conversation";

const CHAT_HUB_URL = `${API_BASE_URL}/hubs/chathub`;
const RECEIVE_MESSAGE_EVENT = "ReceiveMessage";
const JOIN_CONVERSATION_METHOD = "JoinConversation";
const START_RETRY_DELAY_MS = 5_000;

// A mesma mensagem pode chegar pelo SignalR e por um refetch (ex.: invalidação do
// useSendMessage), então o append só acontece se ela ainda não estiver em cache.
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

export function useRealtimeMessages(conversationIds: string[]) {
    const qc = useQueryClient();
    const connectionRef = useRef<HubConnection | null>(null);
    const conversationIdsRef = useRef<string[]>(conversationIds);
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
            const message = conversationsService.mapRealtimeMessage(dto);

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

        connection.onreconnected(() => {
            joinedIds.clear();
            void joinConversations(connection, conversationIdsRef.current, joinedIds);
        });

        let stopped = false;
        let retryTimer: ReturnType<typeof setTimeout> | undefined;

        const startConnection = () => {
            connection
                .start()
                .then(() => {
                    if (stopped) return;
                    void joinConversations(connection, conversationIdsRef.current, joinedIds);
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
}
