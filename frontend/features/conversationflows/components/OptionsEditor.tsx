import { Plus, X } from "lucide-react";

import { Button, Input } from "../../../shared/components/ui";

interface OptionsEditorProps {
    options: string[];
    onChange: (options: string[]) => void;
}

export function OptionsEditor({ options, onChange }: OptionsEditorProps) {
    const add = () => onChange([...options, ""]);
    const remove = (idx: number) => onChange(options.filter((_, i) => i !== idx));
    const update = (idx: number, val: string) =>
        onChange(options.map((o, i) => (i === idx ? val : o)));

    return (
        <div className="flex flex-col gap-2">
            {options.map((opt, idx) => (
                <div key={idx} className="flex items-center gap-2">
                    <Input
                        value={opt}
                        placeholder={`Ex.: ${idx + 1}`}
                        onChange={(e) => update(idx, e.target.value)}
                    />
                    <Button
                        type="button"
                        size="icon"
                        variant="ghost"
                        onClick={() => remove(idx)}
                    >
                        <X className="h-4 w-4" />
                    </Button>
                </div>
            ))}
            <Button type="button" size="sm" variant="outline" onClick={add}>
                <Plus className="h-4 w-4" />
                Adicionar opção
            </Button>
        </div>
    );
}