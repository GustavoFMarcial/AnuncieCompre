import { forwardRef, type InputHTMLAttributes } from "react";

import { cn } from "../../utils/cn";

export const Input = forwardRef<HTMLInputElement, InputHTMLAttributes<HTMLInputElement>>(
    ({ className, ...props }, ref) => {
        return (
            <input
                ref={ref}
                className={cn(
                    "flex h-10 w-full rounded-md border border-neutral-300 bg-white px-3 py-2 text-sm",
                    "placeholder:text-neutral-400",
                    "focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-neutral-900 focus-visible:ring-offset-1",
                    "disabled:cursor-not-allowed disabled:opacity-50",
                    className
                )}
                {...props}
            />
        );
    }
);
Input.displayName = "Input";