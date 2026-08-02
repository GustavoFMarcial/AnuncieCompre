import { useState, type FormEvent } from "react";

import { Button, Input, Label, Select, Textarea } from "../../../shared/components/ui";
import type { CreateFlowInput } from "../types/conversation-flow";

interface FlowFormProps {
    initial?: CreateFlowInput;
    submitting?: boolean;
    onSubmit: (input: CreateFlowInput) => void;
    onCancel: () => void;
    submitLabel?: string;
}

export function FlowForm({
    initial,
    submitting,
    onSubmit,
    onCancel,
    submitLabel = "Salvar",
}: FlowFormProps) {
    const [name, setName] = useState(initial?.name ?? "");
    const [description, setDescription] = useState(initial?.description ?? "");
    const [status, setStatus] = useState<CreateFlowInput["status"]>(initial?.status ?? "Draft");

    const handleSubmit = (e: FormEvent) => {
        e.preventDefault();
        if (!name.trim()) return;
        onSubmit({ name: name.trim(), description: description.trim(), status });
    };

    return (
        <form onSubmit={handleSubmit} className="mt-4 flex flex-col gap-4">
            <div className="space-y-1.5">
                <Label htmlFor="flow-name">Nome</Label>
                <Input
                    id="flow-name"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    placeholder="Ex.: Atendimento principal"
                    autoFocus
                />
            </div>

            <div className="space-y-1.5">
                <Label htmlFor="flow-description">Descrição</Label>
                <Textarea
                    id="flow-description"
                    rows={3}
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    placeholder="Para que serve este fluxo?"
                />
            </div>

            <div className="space-y-1.5">
                <Label htmlFor="flow-status">Status</Label>
                <Select
                    id="flow-status"
                    value={status}
                    onChange={(e) => setStatus(e.target.value as CreateFlowInput["status"])}
                >
                    <option value="Draft">Rascunho</option>
                    <option value="Published">Publicado</option>
                </Select>
            </div>

            <div className="flex justify-end gap-2 pt-2">
                <Button type="button" variant="outline" onClick={onCancel} disabled={submitting}>
                    Cancelar
                </Button>
                <Button type="submit" disabled={submitting || !name.trim()}>
                    {submitLabel}
                </Button>
            </div>
        </form>
    );
}