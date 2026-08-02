import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

import { conversationsService } from "../services/conversations.service";
import type { ConversationStatusFilter } from "../types/conversation";

export function useConversations(status: ConversationStatusFilter) {
    return useQuery({
        queryKey: ["conversations", status] as const,
        queryFn: () => conversationsService.list(status),
    });
}

const conversationKey = (id: string) => ["conversation", id] as const;

export function useConversation(id: string) {
    return useQuery({
        queryKey: conversationKey(id),
        queryFn: () => conversationsService.getById(id),
        enabled: !!id,
        refetchInterval: 10_000,
    });
}

export function useSendMessage(conversationId: string) {
    const qc = useQueryClient();
    return useMutation({
        mutationFn: (text: string) => conversationsService.sendMessage(conversationId, text),
        onMutate: async (text: string) => {
            const key = conversationKey(conversationId);
            await qc.cancelQueries({ queryKey: key });
            const previous = qc.getQueryData<import("../types/conversation").Conversation>(key);
            if (previous) {
                const optimistic = {
                    ...previous,
                    messages: [
                        ...previous.messages,
                        {
                            id: `optimistic_${Date.now()}`,
                            conversationId,
                            text,
                            senderType: "Operator" as const,
                            direction: "Outgoing" as const,
                            createdAt: new Date(),
                        },
                    ],
                    lastMessageAt: new Date(),
                    attendant: "Operator" as const,
                    status: "Open" as const,
                };
                qc.setQueryData(key, optimistic);
            }
            return { previous };
        },
        onError: (_err, _text, context) => {
            if (context?.previous) {
                qc.setQueryData(conversationKey(conversationId), context.previous);
            }
        },
        onSettled: () => {
            qc.invalidateQueries({ queryKey: conversationKey(conversationId) });
            qc.invalidateQueries({ queryKey: ["conversations"] });
        },
    });
}