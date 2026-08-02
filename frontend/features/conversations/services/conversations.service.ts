import { api } from "../../../shared/lib/api";
import type { Conversation, ConversationStatusFilter } from "../types/conversation";
import { mockStore } from "./mock-conversations";

const wait = (ms: number) => new Promise((r) => setTimeout(r, ms));

async function withFallback<T>(real: () => Promise<T>, fallback: () => Promise<T> | T): Promise<T> {
    try {
        return await real();
    } catch (err) {
        if (import.meta.env.DEV) {
            console.warn("[conversations.service] usando mock fallback", err);
        }
        return await fallback();
    }
}

function genId(): string {
    return `m_${Date.now()}_${Math.random().toString(36).slice(2, 8)}`;
}

export const conversationsService = {
    async list(status: ConversationStatusFilter): Promise<Conversation[]> {
        return withFallback(
            async () =>
                (await api.get<Conversation[]>("/api/conversations", {
                    params: status === "all" ? undefined : { status },
                })).data,
            () => {
                const list = structuredClone(mockStore.conversations) as Conversation[];
                return status === "all"
                    ? list
                    : list.filter((c) => c.status === status);
            }
        );
    },

    async getById(id: string): Promise<Conversation> {
        return withFallback(
            async () => (await api.get<Conversation>(`/api/conversations/${id}`)).data,
            () => {
                const found = mockStore.conversations.find((c) => c.id === id);
                if (!found) throw new Error("Conversa não encontrada");
                return structuredClone(found) as Conversation;
            }
        );
    },

    async sendMessage(id: string, text: string): Promise<void> {
        return withFallback(
            async () => {
                await api.post(`/api/conversations/${id}/messages`, { text });
            },
            async () => {
                await wait(150);
                const conv = mockStore.conversations.find((c) => c.id === id);
                if (!conv) throw new Error("Conversa não encontrada");
                conv.messages.push({
                    id: genId(),
                    conversationId: id,
                    text,
                    senderType: "Operator",
                    direction: "Outgoing",
                    createdAt: new Date(),
                });
                conv.lastMessageAt = new Date();
                conv.attendant = "Operator";
                conv.status = "Open";
            }
        );
    },
};