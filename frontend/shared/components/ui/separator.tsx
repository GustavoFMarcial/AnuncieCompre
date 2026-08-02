import type { HTMLAttributes } from "react";

import { cn } from "../../utils/cn";

export function Separator({ className, ...props }: HTMLAttributes<HTMLDivElement>) {
    return <div className={cn("h-px w-full bg-neutral-200", className)} {...props} />;
}