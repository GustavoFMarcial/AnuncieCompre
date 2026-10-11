export type ConversationStatus = "JustCreated" | "Open" | "Closed";
export type ConversationAttendant = "Bot" | "Operator";
export type MessageSenderType = "Operator" | "Bot" | "Customer";
export type MessageDirection = "Incoming" | "Outgoing";

export type ConversationStatusFilter = "all" | ConversationStatus;

export interface Message {
    id: string;
    conversationId: string;
    text: string;
    senderType: MessageSenderType;
    direction: MessageDirection;
    createdAt: Date;
}

export interface Conversation {
    id: string;
    userId: string;
    userName: string;
    userPhone: string;
    status: ConversationStatus;
    attendant: ConversationAttendant;
    awaitingResponseNodeId: string;
    lastMessageAt: Date;
    messages: Message[];
}

export interface MessageDTO {
    text: string;
    senderType: string;
    direction: string;
    createdAt: string;
}

export interface ConversationDTO {
    conversationId: string;
    userId: string;
    userPhone: string;
    userName: string;
    status: string;
    attendant: string;
    messages: MessageDTO[];
}

export interface GetConversationsResponse {
    conversations: ConversationDTO[];
}