import type { HTMLAttributes } from "react";

import { cn } from "../../utils/cn";

export function ScrollArea({ className, ...props }: HTMLAttributes<HTMLDivElement>) {
    return (
        <div
            className={cn("overflow-y-auto [scrollbar-width:thin]", className)}
            {...props}
        />
    );
}