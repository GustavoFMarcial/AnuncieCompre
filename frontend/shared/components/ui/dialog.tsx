import { useEffect, type ReactNode } from "react";

import { cn } from "../../utils/cn";

interface DialogProps {
    open: boolean;
    onClose: () => void;
    children: ReactNode;
    className?: string;
}

export function Dialog({ open, onClose, children, className }: DialogProps) {
    useEffect(() => {
        if (!open) return;
        const handler = (e: KeyboardEvent) => {
            if (e.key === "Escape") onClose();
        };
        window.addEventListener("keydown", handler);
        return () => window.removeEventListener("keydown", handler);
    }, [open, onClose]);

    if (!open) return null;

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center">
            <div
                className="absolute inset-0 bg-black/40"
                onClick={onClose}
            />
            <div
                className={cn(
                    "relative z-10 w-full max-w-md rounded-lg border border-neutral-200 bg-white p-5 shadow-lg",
                    className
                )}
            >
                {children}
            </div>
        </div>
    );
}

export function DialogTitle({ children }: { children: ReactNode }) {
    return <h2 className="text-lg font-semibold text-neutral-900">{children}</h2>;
}

export function DialogDescription({ children }: { children: ReactNode }) {
    return <p className="mt-1 text-sm text-neutral-500">{children}</p>;
}