import type { ReactNode } from "react";

import {
    ConversationsRealtimeContext,
    useConversationsRealtimeCore,
} from "../hooks/useConversationsRealtime";

export function ConversationsRealtimeProvider({ children }: { children: ReactNode }) {
    const value = useConversationsRealtimeCore();

    return (
        <ConversationsRealtimeContext.Provider value={value}>
            {children}
        </ConversationsRealtimeContext.Provider>
    );
}
