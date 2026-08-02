import { useEffect, useRef } from "react";
import { Bot, Headset, Phone } from "lucide-react";

import { Badge, ScrollArea } from "../../../shared/components/ui";
import type { Conversation } from "../types/conversation";
import { MessageBubble } from "./MessageBubble";
import { MessageComposer } from "./MessageComposer";
import { useSendMessage } from "../hooks/useConversations";

interface ConversationThreadProps {
    conversation: Conversation;
}

export function ConversationThread({ conversation }: ConversationThreadProps) {
    const bottomRef = useRef<HTMLDivElement>(null);
    const sendMessage = useSendMessage(conversation.id);

    useEffect(() => {
        bottomRef.current?.scrollIntoView({ behavior: "smooth" });
    }, [conversation.messages.length]);

    const disabled = conversation.status === "Closed";

    return (
        <div className="flex h-full flex-col">
            <div className="flex items-center justify-between border-b border-neutral-200 bg-white px-5 py-3">
                <div className="flex items-center gap-3">
                    <div className="flex h-10 w-10 items-center justify-center rounded-full bg-neutral-200 font-semibold text-neutral-700">
                        {conversation.userName.charAt(0).toUpperCase()}
                    </div>
                    <div>
                        <h3 className="font-semibold text-neutral-900">{conversation.userName}</h3>
                        <div className="flex items-center gap-1 text-xs text-neutral-400">
                            <Phone className="h-3 w-3" />
                            {conversation.userPhone}
                        </div>
                    </div>
                </div>
                <div className="flex items-center gap-2">
                    {conversation.attendant === "Operator" ? (
                        <Badge variant="secondary" className="gap-1">
                            <Headset className="h-3 w-3" /> Operador
                        </Badge>
                    ) : (
                        <Badge variant="outline" className="gap-1">
                            <Bot className="h-3 w-3" /> Bot
                        </Badge>
                    )}
                    {conversation.status === "Closed" && (
                        <Badge variant="destructive">Encerrada</Badge>
                    )}
                    {conversation.status === "Open" && <Badge variant="success">Aberta</Badge>}
                    {conversation.status === "JustCreated" && (
                        <Badge variant="warning">Nova</Badge>
                    )}
                </div>
            </div>

            <ScrollArea className="flex-1 bg-neutral-50 px-5 py-4">
                <div className="flex flex-col gap-2.5">
                    {conversation.messages.map((m) => (
                        <MessageBubble key={m.id} message={m} />
                    ))}
                    <div ref={bottomRef} />
                </div>
            </ScrollArea>

            {disabled ? (
                <div className="border-t border-neutral-200 bg-white p-3 text-center text-sm text-neutral-400">
                    Esta conversa está encerrada.
                </div>
            ) : (
                <MessageComposer
                    onSend={(text) => sendMessage.mutate(text)}
                    sending={sendMessage.isPending}
                />
            )}
        </div>
    );
}