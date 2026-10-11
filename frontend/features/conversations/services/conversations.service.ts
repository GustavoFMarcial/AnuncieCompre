import { api } from "../../../shared/lib/api";
import type { Conversation, ConversationDTO, ConversationStatusFilter, GetConversationsResponse, Message } from "../types/conversation";

const STATUS_MAP: Record<string, Conversation["status"]> = {
    Nova: "JustCreated",
    Aberta: "Open",
    Encerrada: "Closed",
};

const ATTENDANT_MAP: Record<string, Conversation["attendant"]> = {
    Bot: "Bot",
    Operador: "Operator",
};

const SENDER_TYPE_MAP: Record<string, Message["senderType"]> = {
    Bot: "Bot",
    Operador: "Operator",
    Client: "Customer",
};

const DIRECTION_MAP: Record<string, Message["direction"]> = {
    Entrada: "Incoming",
    "Saída": "Outgoing",
};

function mapConversation(dto: ConversationDTO): Conversation {
    const messages: Message[] = dto.messages.map((m, index) => ({
        id: `${dto.conversationId}_${index}`,
        conversationId: dto.conversationId,
        text: m.text,
        senderType: SENDER_TYPE_MAP[m.senderType] ?? "Customer",
        direction: DIRECTION_MAP[m.direction] ?? "Outgoing",
        createdAt: new Date(m.createdAt),
    }));

    const lastMessage = dto.messages[dto.messages.length - 1];

    return {
        id: dto.conversationId,
        userId: dto.userId,
        userName: dto.userName,
        userPhone: dto.userPhone,
        status: STATUS_MAP[dto.status] ?? "Open",
        attendant: ATTENDANT_MAP[dto.attendant] ?? "Bot",
        awaitingResponseNodeId: "",
        lastMessageAt: lastMessage ? new Date(lastMessage.createdAt) : new Date(),
        messages,
    };
}

export const conversationsService = {
    async list(status: ConversationStatusFilter): Promise<Conversation[]> {
        const res = await api.get<GetConversationsResponse>("/api/conversations", {
            params: status === "all" ? undefined : { status },
        });
        return res.data.conversations.map(mapConversation);
    },

    async getById(id: string): Promise<Conversation> {
        const res = await api.get<ConversationDTO>(`/api/conversations/${id}`);
                return mapConversation(res.data);
    },

    async sendMessage(id: string, text: string): Promise<void> {
        await api.post(`/api/conversations/${id}/messages`, JSON.stringify(text));
    },
}