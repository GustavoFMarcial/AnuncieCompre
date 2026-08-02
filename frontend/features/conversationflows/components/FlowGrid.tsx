import type { ConversationFlow } from "../types/conversation-flow";
import { FlowCard } from "./FlowCard";

interface FlowGridProps {
    flows: ConversationFlow[];
}

export function FlowGrid({ flows }: FlowGridProps) {
    return (
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {flows.map((flow) => (
                <FlowCard key={flow.id} flow={flow} />
            ))}
        </div>
    );
}