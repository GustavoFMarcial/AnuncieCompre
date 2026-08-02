import { ScrollArea } from "../../../shared/components/ui";
import type { Conversation, ConversationStatusFilter } from "../types/conversation";
import { ConversationListItem } from "./ConversationListItem";

interface ConversationListProps {
    conversations: Conversation[];
    selectedId: string | null;
    status: ConversationStatusFilter;
    onSelect: (id: string) => void;
}

import { Inbox } from "lucide-react";

export function ConversationList({
    conversations,
    selectedId,
    onSelect,
}: ConversationListProps) {
    if (conversations.length === 0) {
        return (
            <div className="flex flex-col items-center justify-center gap-2 p-8 text-center text-sm text-neutral-400">
                <Inbox className="h-8 w-8" />
                Nenhuma conversa.
            </div>
        );
    }

    return (
        <ScrollArea className="flex-1">
            {conversations.map((c) => (
                <ConversationListItem
                    key={c.id}
                    conversation={c}
                    selected={c.id === selectedId}
                    onSelect={() => onSelect(c.id)}
                />
            ))}
        </ScrollArea>
    );
}