import { cn } from "../../../shared/utils/cn";
import type { Message } from "../types/conversation";

interface MessageBubbleProps {
    message: Message;
}

function isOwn(message: Message): boolean {
    return message.direction === "outgoing";
}

function senderLabel(message: Message): string {
    switch (message.senderType) {
        case "bot":
            return "Bot";
        case "customer":
            return "Cliente";
        case "operator":
            return "Você";
    }
}

export function MessageBubble({ message }: MessageBubbleProps) {
    const own = isOwn(message);
    const isOperator = message.senderType === "operator";

    return (
        <div className={cn("flex", own ? "justify-end" : "justify-start")}>
            <div
                className={cn(
                    "max-w-[75%] rounded-2xl px-3 py-2 shadow-sm",
                    own ? "rounded-br-sm" : "rounded-bl-sm",
                    isOperator
                        ? "bg-neutral-900 text-white"
                        : message.senderType === "customer"
                            ? "bg-white text-neutral-900"
                            : "bg-neutral-100 text-neutral-900"
                )}
            >
                {!isOperator && (
                    <div className="mb-0.5 text-xs font-semibold text-neutral-500">
                        {senderLabel(message)}
                    </div>
                )}
                <p className="whitespace-pre-wrap break-words text-sm">{message.text}</p>
                <div
                    className={cn(
                        "mt-1 text-right text-[10px]",
                        isOperator ? "text-neutral-300" : "text-neutral-400"
                    )}
                >
                    {message.createdAt.toLocaleTimeString("pt-BR", {
                        hour: "2-digit",
                        minute: "2-digit",
                    })}
                </div>
            </div>
        </div>
    );
}