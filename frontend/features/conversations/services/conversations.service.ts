import { api } from "../../../shared/lib/api";
import type { Conversation, ConversationDTO, ConversationStatusFilter, GetConversationsResponse, Message } from "../types/conversation";

const STATUS_MAP: Record<string, Conversation["status"]> = {
    justcreated: "justcreated",
    open: "open",
    closed: "closed",
};

const ATTENDANT_MAP: Record<string, Conversation["attendant"]> = {
    operator: "operator",
    bot: "bot",
};

const SENDER_TYPE_MAP: Record<string, Message["senderType"]> = {
    operator: "operator",
    bot: "bot",
    customer: "customer",
};

const DIRECTION_MAP: Record<string, Message["direction"]> = {
    incoming: "incoming",
    outgoing: "outgoing",
};

function mapConversation(dto: ConversationDTO): Conversation {
    const messages: Message[] = dto.messages.map((m, index) => ({
        id: `${dto.conversationId}_${index}`,
        conversationId: dto.conversationId,
        text: m.text,
        senderType: SENDER_TYPE_MAP[m.senderType] ?? "customer",
        direction: DIRECTION_MAP[m.direction] ?? "outgoing",
        createdAt: new Date(m.createdAt),
    }));

    const lastMessage = dto.messages[dto.messages.length - 1];

    return {
        id: dto.conversationId,
        userId: dto.userId,
        userName: dto.userName,
        userPhone: dto.userPhone,
        status: STATUS_MAP[dto.status] ?? "open",
        attendant: ATTENDANT_MAP[dto.attendant] ?? "bot",
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