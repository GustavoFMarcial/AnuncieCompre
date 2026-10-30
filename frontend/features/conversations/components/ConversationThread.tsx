import { useEffect, useRef, useState } from "react";
import { Bot, Headset, Phone, SquarePen, UserRound } from "lucide-react";

import { Badge, Button, ScrollArea } from "../../../shared/components/ui";
import { formatPhoneForDisplay } from "../../../shared/utils/phone";
import type { Conversation } from "../types/conversation";
import { MessageBubble } from "./MessageBubble";
import { MessageComposer } from "./MessageComposer";
import { useSendMessage } from "../hooks/useConversations";
import { EditCustomerDialog } from "../../customers/components/EditCustomerDialog";

interface ConversationThreadProps {
    conversation: Conversation;
}

export function ConversationThread({ conversation }: ConversationThreadProps) {
    const bottomRef = useRef<HTMLDivElement>(null);
    const sendMessage = useSendMessage(conversation.id);
    const [editCustomerOpen, setEditCustomerOpen] = useState(false);

    const customerName = conversation.customerName;
    const displayName = customerName || formatPhoneForDisplay(conversation.customerPhone);

    useEffect(() => {
        bottomRef.current?.scrollIntoView({ behavior: "smooth" });
    }, [conversation.messages.length]);

    const disabled = conversation.status === "closed";

    return (
        <div className="flex h-full flex-col">
            <div className="flex items-center justify-between border-b border-neutral-200 bg-white px-5 py-3">
                <div className="flex items-center gap-3">
                    <div className="flex h-10 w-10 items-center justify-center rounded-full bg-neutral-200 font-semibold text-neutral-700">
                        {customerName ? (
                            customerName.charAt(0).toUpperCase()
                        ) : (
                            <UserRound className="h-5 w-5" />
                        )}
                    </div>
                    <div>
                        <h3 className="font-semibold text-neutral-900">{displayName}</h3>
                        <div className="flex items-center gap-1 text-xs text-neutral-400">
                            <Phone className="h-3 w-3" />
                            {formatPhoneForDisplay(conversation.customerPhone)}
                        </div>
                    </div>
                    <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => setEditCustomerOpen(true)}
                        title="Editar dados do cliente"
                        aria-label="Editar dados do cliente"
                    >
                        <SquarePen className="h-4 w-4" />
                    </Button>
                </div>
                <div className="flex items-center gap-2">
                    {conversation.attendant === "operator" ? (
                        <Badge variant="secondary" className="gap-1">
                            <Headset className="h-3 w-3" /> Operador
                        </Badge>
                    ) : (
                        <Badge variant="outline" className="gap-1">
                            <Bot className="h-3 w-3" /> Bot
                        </Badge>
                    )}
                    {conversation.status === "closed" && (
                        <Badge variant="destructive">Encerrada</Badge>
                    )}
                    {conversation.status === "open" && <Badge variant="success">Aberta</Badge>}
                    {conversation.status === "justcreated" && (
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

            <EditCustomerDialog
                customerId={conversation.customerId}
                open={editCustomerOpen}
                onOpenChange={setEditCustomerOpen}
            />
        </div>
    );
}