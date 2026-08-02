import type { Conversation, MessageDirection, MessageSenderType } from "../types/conversation";

type MsgTuple = [MessageSenderType, MessageDirection, string, Date];

function mk(
    id: string,
    userName: string,
    userPhone: string,
    status: Conversation["status"],
    attendant: Conversation["attendant"],
    awaiting: string,
    msgs: MsgTuple[]
): Conversation {
    const messages: Conversation["messages"] = msgs.map(([sender, dir, text, at], i) => ({
        id: `${id}-m${i}`,
        conversationId: id,
        text,
        senderType: sender,
        direction: dir,
        createdAt: at,
    }));
    return {
        id,
        userId: `user-${id}`,
        userName,
        userPhone,
        status,
        attendant,
        awaitingResponseNodeId: awaiting,
        lastMessageAt: messages[messages.length - 1]?.createdAt ?? new Date(),
        messages,
    };
}

const now = Date.now();
const min = 60 * 1000;
const hour = 60 * min;
const day = 24 * hour;

export const mockConversations: Conversation[] = [
    mk("c1", "Maria Silva", "+5511987654321", "Open", "Bot", "ask_product", [
        ["Customer", "Incoming", "Olá, quero comprar", new Date(now - hour)],
        ["Bot", "Outgoing", "Qual categoria da sua empresa?", new Date(now - hour + 60000)],
        ["Customer", "Incoming", "Restaurante", new Date(now - hour + 120000)],
        ["Bot", "Outgoing", "Qual produto você quer anunciar?", new Date(now - hour + 180000)],
    ]),
    mk("c2", "João Pereira", "+5511912345678", "Open", "Operator", "ask_quantity", [
        ["Customer", "Incoming", "Bom dia", new Date(now - 30 * min)],
        ["Bot", "Outgoing", "Olá! Deseja fazer um pedido? (1 Sim, 2 Não)", new Date(now - 29 * min)],
        ["Customer", "Incoming", "1", new Date(now - 28 * min)],
        ["Bot", "Outgoing", "Qual categoria da sua empresa?", new Date(now - 27 * min)],
        ["Customer", "Incoming", "Mercado", new Date(now - 26 * min)],
        ["Bot", "Outgoing", "Qual produto?", new Date(now - 25 * min)],
        ["Customer", "Incoming", "Feijão", new Date(now - 24 * min)],
        ["Operator", "Outgoing", "Olá João, aqui é da equipe. Quantas unidades?", new Date(now - 5 * min)],
    ]),
    mk("c3", "Ana Costa", "+5511900000000", "Closed", "Bot", "finish", [
        ["Customer", "Incoming", "Quero falar com atendente", new Date(now - day)],
        ["Bot", "Outgoing", "Atendimento finalizado. Obrigado!", new Date(now - day + 60000)],
    ]),
    mk("c4", "Carlos Souza", "+5511999999999", "Open", "Bot", "ask_email", [
        ["Customer", "Incoming", "Oi", new Date(now - 2 * hour)],
        ["Bot", "Outgoing", "Olá! Deseja fazer um pedido? (1 Sim, 2 Não)", new Date(now - 2 * hour + 5000)],
    ]),
];

export const mockStore = {
    conversations: structuredClone(mockConversations) as Conversation[],
};