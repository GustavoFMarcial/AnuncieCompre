import { Select } from "../../../shared/components/ui";
import type { NodeValidationKind } from "../types/conversation-flow";
import { validationKindOptions } from "../utils/validation-options";

interface ValidationKindSelectProps {
    value: NodeValidationKind | null;
    onChange: (kind: NodeValidationKind | null) => void;
}

export function ValidationKindSelect({ value, onChange }: ValidationKindSelectProps) {
    return (
        <Select
            value={value ?? ""}
            onChange={(e) => {
                const val = e.target.value;
                onChange(val === "" ? null : (val as NodeValidationKind));
            }}
        >
            {validationKindOptions().map((opt) => (
                <option key={opt.value} value={opt.value}>
                    {opt.label}
                </option>
            ))}
        </Select>
    );
}