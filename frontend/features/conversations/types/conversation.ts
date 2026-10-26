export type ConversationStatus = "justcreated" | "open" | "closed";
export type ConversationAttendant = "bot" | "operator";
export type MessageSenderType = "operator" | "bot" | "customer";
export type MessageDirection = "incoming" | "outgoing";

export type ConversationStatusFilter = "all" | ConversationStatus;

export interface Message {
    id: string;
    conversationId: string;
    text: string;
    senderType: MessageSenderType;
    direction: MessageDirection;
    isRead: boolean;
    createdAt: Date;
}

export interface Conversation {
    id: string;
    customerId: string;
    customerName: string;
    customerPhone: string;
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
    isRead: boolean;
    createdAt: string;
}

// Payload cru do evento "ReceiveMessage" do SignalR. O protocolo JSON do hub
// (System.Text.Json) serializa os enums como números:
// MessageSenderType: 0=operator, 1=bot, 2=customer; MessageDirection: 0=incoming, 1=outgoing.
export interface RealtimeMessageDTO {
    conversationId: string;
    senderType: number;
    direction: number;
    isRead: boolean;
    createdAt: string;
    text: string;
}

export interface ConversationDTO {
    conversationId: string;
    customerId: string;
    customerPhone: string;
    customerName: string;
    status: string;
    attendant: string;
    messages: MessageDTO[];
}

export interface GetConversationsResponse {
    conversations: ConversationDTO[];
}