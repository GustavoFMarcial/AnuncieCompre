import { useEffect, useRef, useState, type ReactNode } from "react";

import { cn } from "../../utils/cn";

interface DropdownMenuProps {
    trigger: ReactNode;
    children: ReactNode;
    className?: string;
    align?: "start" | "end";
}

export function DropdownMenu({ trigger, children, className, align = "end" }: DropdownMenuProps) {
    const [open, setOpen] = useState(false);
    const ref = useRef<HTMLDivElement>(null);

    useEffect(() => {
        if (!open) return;
        const handleClick = (e: MouseEvent) => {
            if (ref.current && !ref.current.contains(e.target as Node)) {
                setOpen(false);
            }
        };
        const handleKey = (e: KeyboardEvent) => {
            if (e.key === "Escape") setOpen(false);
        };
        document.addEventListener("mousedown", handleClick);
        document.addEventListener("keydown", handleKey);
        return () => {
            document.removeEventListener("mousedown", handleClick);
            document.removeEventListener("keydown", handleKey);
        };
    }, [open]);

    return (
        <div className="relative" ref={ref}>
            <button
                type="button"
                onClick={() => setOpen((v) => !v)}
                className="inline-flex items-center justify-center rounded-md p-1 text-neutral-500 transition-colors hover:bg-neutral-100 hover:text-neutral-900"
            >
                {trigger}
            </button>
            {open && (
                <div
                    className={cn(
                        "absolute z-20 mt-1 min-w-[10rem] rounded-md border border-neutral-200 bg-white py-1 shadow-md",
                        align === "end" ? "right-0" : "left-0",
                        className
                    )}
                    onClick={() => setOpen(false)}
                >
                    {children}
                </div>
            )}
        </div>
    );
}

interface DropdownMenuItemProps {
    children: ReactNode;
    onClick?: () => void;
    destructive?: boolean;
    icon?: ReactNode;
}

export function DropdownMenuItem({
    children,
    onClick,
    destructive,
    icon,
}: DropdownMenuItemProps) {
    return (
        <button
            type="button"
            onClick={onClick}
            className={cn(
                "flex w-full items-center gap-2 px-3 py-1.5 text-left text-sm transition-colors",
                destructive
                    ? "text-red-600 hover:bg-red-50"
                    : "text-neutral-700 hover:bg-neutral-100"
            )}
        >
            {icon && <span className="flex h-4 w-4 items-center justify-center">{icon}</span>}
            <span>{children}</span>
        </button>
    );
}

export function DropdownMenuSeparator() {
    return <div className="my-1 h-px bg-neutral-100" />;
}