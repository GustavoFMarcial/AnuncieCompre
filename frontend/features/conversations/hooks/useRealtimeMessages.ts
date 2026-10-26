import { useEffect, useRef } from "react";
import { useQueryClient } from "@tanstack/react-query";
import type { QueryClient } from "@tanstack/react-query";
import { HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import type { HubConnection } from "@microsoft/signalr";

import { API_BASE_URL } from "../../../shared/lib/api";
import { conversationsService } from "../services/conversations.service";
import type { Conversation, Message, RealtimeMessageDTO } from "../types/conversation";

const CHAT_HUB_URL = `${API_BASE_URL}/hubs/chathub`;
const RECEIVE_MESSAGE_EVENT = "ReceiveMessage";
const RECEIVE_CONVERSATION_EVENT = "ReceiveConversation";
const JOIN_CONVERSATION_METHOD = "JoinConversation";
const MARK_MESSAGES_AS_READ_METHOD = "MarkMessagesAsRead";
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

// Zera o contador de não lidas de uma conversa no cache: apenas mensagens do
// cliente contam para o badge.
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

async function markMessagesAsReadOnHub(connection: HubConnection, conversationId: string): Promise<void> {
    await connection.invoke(MARK_MESSAGES_AS_READ_METHOD, conversationId).catch((error: unknown) => {
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

export function useRealtimeMessages(conversationIds: string[], selectedId: string | null) {
    const qc = useQueryClient();
    const connectionRef = useRef<HubConnection | null>(null);
    const conversationIdsRef = useRef<string[]>(conversationIds);
    const selectedIdRef = useRef<string | null>(selectedId);
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

            // Mensagem do cliente chegando na conversa aberta: entra no cache já
            // como lida e o backend persiste o IsRead.
            if (message.conversationId === selectedIdRef.current && message.senderType === "customer") {
                message = { ...message, isRead: true };
                void markMessagesAsReadOnHub(connection, message.conversationId);
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

        // Conversa recém-criada: o backend notifica depois de persistir, então um
        // refetch da lista já a traz com cliente e mensagens completas.
        connection.on(RECEIVE_CONVERSATION_EVENT, () => {
            void qc.invalidateQueries({ queryKey: ["conversations"] });
        });

        connection.onreconnected(async () => {
            joinedIds.clear();
            void joinConversations(connection, conversationIdsRef.current, joinedIds);

            // Mensagens enviadas ao hub durante a desconexão não são recuperadas
            // (o SignalR não faz replay): a conversa aberta é marcada como lida
            // antes do refetch, para a lista já voltar com o IsRead atualizado.
            if (selectedIdRef.current) {
                await markMessagesAsReadOnHub(connection, selectedIdRef.current);
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
                    // Caso o operador tenha aberto a conversa antes da conexão ser
                    // estabelecida, o mark as read pendente é enviado agora.
                    if (selectedIdRef.current) {
                        void markMessagesAsReadOnHub(connection, selectedIdRef.current);
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

    useEffect(() => {
        selectedIdRef.current = selectedId;
    }, [selectedId]);

    useEffect(() => {
        if (!selectedId) return;

        // Abrir a conversa marca as mensagens do cliente como lidas: o backend
        // persiste e o contador zera otimisticamente no cache local.
        const connection = connectionRef.current;
        if (connection && connection.state === HubConnectionState.Connected) {
            void markMessagesAsReadOnHub(connection, selectedId);
        }
        markConversationAsReadInCache(qc, selectedId);
    }, [selectedId, qc]);
}
