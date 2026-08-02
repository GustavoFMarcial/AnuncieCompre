import { cn } from "../../../shared/utils/cn";
import type { ConversationStatusFilter } from "../types/conversation";

interface StatusFilterProps {
    value: ConversationStatusFilter;
    onChange: (value: ConversationStatusFilter) => void;
}

const options: { value: ConversationStatusFilter; label: string }[] = [
    { value: "all", label: "Todas" },
    { value: "Open", label: "Em aberto" },
    { value: "JustCreated", label: "Novas" },
    { value: "Closed", label: "Encerradas" },
];

export function StatusFilter({ value, onChange }: StatusFilterProps) {
    return (
        <div className="flex gap-1 rounded-md border border-neutral-200 bg-white p-1 text-sm">
            {options.map((opt) => (
                <button
                    key={opt.value}
                    onClick={() => onChange(opt.value)}
                    className={cn(
                        "rounded px-3 py-1 font-medium transition-colors",
                        value === opt.value
                            ? "bg-neutral-900 text-white"
                            : "text-neutral-600 hover:bg-neutral-100"
                    )}
                >
                    {opt.label}
                </button>
            ))}
        </div>
    );
}