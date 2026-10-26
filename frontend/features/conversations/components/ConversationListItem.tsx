import { Bot, Headset } from "lucide-react";

import { Badge } from "../../../shared/components/ui";
import { cn } from "../../../shared/utils/cn";
import type { Conversation } from "../types/conversation";

interface ConversationListItemProps {
    conversation: Conversation;
    selected: boolean;
    onSelect: () => void;
}

function formatTime(date: Date): string {
    const now = new Date();
    const diff = now.getTime() - date.getTime();
    const sameDay = now.toDateString() === date.toDateString();
    if (sameDay) return date.toLocaleTimeString("pt-BR", { hour: "2-digit", minute: "2-digit" });
    if (diff < 1000 * 60 * 60 * 24 * 7) {
        return date.toLocaleDateString("pt-BR", { weekday: "short" });
    }
    return date.toLocaleDateString("pt-BR", { day: "2-digit", month: "2-digit" });
}

export function ConversationListItem({
    conversation,
    selected,
    onSelect,
}: ConversationListItemProps) {
    const last = conversation.messages[conversation.messages.length - 1];
    const unreadCount = conversation.messages.filter(
        (m) => m.senderType === "customer" && !m.isRead,
    ).length;

    return (
        <button
            onClick={onSelect}
            className={cn(
                "flex w-full items-start gap-3 border-b border-neutral-100 p-3 text-left transition-colors hover:bg-neutral-50",
                selected && "bg-neutral-100"
            )}
        >
            <div className="mt-0.5 flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-neutral-200 text-sm font-semibold text-neutral-700">
                {conversation.customerName.charAt(0).toUpperCase()}
            </div>
            <div className="flex min-w-0 flex-1 flex-col">
                <div className="flex items-center justify-between gap-2">
                    <span className="truncate font-medium text-neutral-900">
                        {conversation.customerName}
                    </span>
                    <span className="shrink-0 text-xs text-neutral-400">
                        {formatTime(conversation.lastMessageAt)}
                    </span>
                </div>
                <span className="truncate text-xs text-neutral-400">{conversation.customerPhone}</span>
                <div className="mt-1 flex items-center justify-between gap-2">
                    <span className="truncate text-sm text-neutral-500">
                        {last ? last.text : "Sem mensagens"}
                    </span>
                    {unreadCount > 0 && (
                        <Badge variant="success" className="shrink-0 bg-green-600 px-2 text-white">
                            {unreadCount > 99 ? "99+" : unreadCount}
                        </Badge>
                    )}
                </div>
                <div className="mt-1.5 flex items-center gap-1.5">
                    {conversation.attendant === "operator" ? (
                        <Badge variant="secondary" className="gap-1">
                            <Headset className="h-3 w-3" />
                            Operador
                        </Badge>
                    ) : (
                        <Badge variant="outline" className="gap-1">
                            <Bot className="h-3 w-3" />
                            Bot
                        </Badge>
                    )}
                    {conversation.status === "open" && (
                        <Badge variant="success">Aberta</Badge>
                    )}
                    {conversation.status === "justcreated" && (
                        <Badge variant="warning">Nova</Badge>
                    )}
                    {conversation.status === "closed" && (
                        <Badge variant="destructive">Encerrada</Badge>
                    )}
                </div>
            </div>
        </button>
    );
}