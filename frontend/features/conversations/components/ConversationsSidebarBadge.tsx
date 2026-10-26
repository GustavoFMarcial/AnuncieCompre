import { Badge } from "../../../shared/components/ui";
import { useConversationsRealtime } from "../hooks/useConversationsRealtime";

export function ConversationsSidebarBadge() {
    const { unreadConversations } = useConversationsRealtime();

    if (unreadConversations <= 0) return null;

    return (
        <Badge variant="success" className="ml-auto shrink-0 bg-green-600 px-2 text-white">
            {unreadConversations > 99 ? "99+" : unreadConversations}
        </Badge>
    );
}
