import type { ReactNode } from "react";

interface FlowPageHeaderProps {
    title: string;
    description?: string;
    action?: ReactNode;
}

export function FlowPageHeader({ title, description, action }: FlowPageHeaderProps) {
    return (
        <div className="flex items-start justify-between gap-4">
            <div>
                <h1 className="text-2xl font-bold text-neutral-900">{title}</h1>
                {description && <p className="mt-1 text-sm text-neutral-500">{description}</p>}
            </div>
            {action}
        </div>
    );
}