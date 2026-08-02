import { memo } from "react";
import { Handle, Position, type NodeProps } from "@xyflow/react";
import { Bot, CheckCircle2, AlignLeft } from "lucide-react";

import { cn } from "../../../shared/utils/cn";
import type { FlowRFNode } from "../utils/dagre-layout";

function validationLabel(kind: string | null): string {
    switch (kind) {
        case "Option":
            return "Opção";
        case "Confirmation":
            return "Confirmação";
        case "Validation":
            return "Validação";
        case "OptionValidation":
            return "Opção + Validação";
        case "Final":
            return "Final";
        default:
            return "Sem validação";
    }
}

function FlowNodeCardBase({ data, selected }: NodeProps<FlowRFNode>) {
    const preview = data.message.length > 70 ? data.message.slice(0, 70) + "…" : data.message;

    return (
        <div
            className={cn(
                "w-60 rounded-lg border bg-white px-3 py-2 shadow-sm transition-shadow",
                selected ? "border-neutral-900 shadow-md ring-2 ring-neutral-900" : "border-neutral-200",
                data.isFinal && "border-green-300 bg-green-50"
            )}
        >
            {!data.isFinal && (
                <Handle type="target" position={Position.Top} className="!h-3 !w-3 !bg-neutral-400" />
            )}

            <div className="flex items-center gap-1.5 text-xs font-medium text-neutral-500">
                {data.validationKind === "Final" ? (
                    <CheckCircle2 className="h-3.5 w-3.5 text-green-600" />
                ) : (
                    <Bot className="h-3.5 w-3.5" />
                )}
                <span>{validationLabel(data.validationKind)}</span>
            </div>

            <p className="mt-1 flex items-start gap-1 text-sm text-neutral-800">
                <AlignLeft className="mt-0.5 h-3.5 w-3.5 shrink-0 text-neutral-400" />
                <span className="line-clamp-3">{preview || "Node vazio"}</span>
            </p>

            {!data.isFinal && (
                <Handle
                    type="source"
                    position={Position.Bottom}
                    className="!h-3 !w-3 !bg-neutral-900"
                />
            )}
        </div>
    );
}

export const FlowNodeCard = memo(FlowNodeCardBase);