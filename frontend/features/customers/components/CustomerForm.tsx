import { useState, type FormEvent } from "react";

import { Button, Input, Label } from "../../../shared/components/ui";
import { formatPhoneForDisplay } from "../../../shared/utils/phone";

interface CustomerFormValues {
    name: string;
    email: string;
    phone: string;
}

interface CustomerFormProps {
    mode: "create" | "edit";
    initial?: { name?: string | null; email?: string | null; phone?: string | null };
    submitting?: boolean;
    errors?: string[];
    submitLabel?: string;
    onSubmit: (values: CustomerFormValues) => void;
    onCancel: () => void;
}

export function CustomerForm({
    mode,
    initial,
    submitting,
    errors,
    submitLabel = "Salvar",
    onSubmit,
    onCancel,
}: CustomerFormProps) {
    const [name, setName] = useState(initial?.name ?? "");
    const [email, setEmail] = useState(initial?.email ?? "");
    const [phone, setPhone] = useState(initial?.phone ?? "");

    const canSubmit =
        name.trim().length > 0 &&
        email.trim().length > 0 &&
        (mode === "edit" || phone.trim().length > 0);

    const handleSubmit = (e: FormEvent) => {
        e.preventDefault();
        if (!canSubmit) return;
        onSubmit({ name: name.trim(), email: email.trim(), phone: phone.trim() });
    };

    return (
        <form onSubmit={handleSubmit} className="mt-4 flex flex-col gap-4">
            <div className="space-y-1.5">
                <Label htmlFor="customer-name">Nome</Label>
                <Input
                    id="customer-name"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    placeholder="Ex.: Maria Souza"
                    autoFocus
                />
            </div>

            <div className="space-y-1.5">
                <Label htmlFor="customer-email">E-mail</Label>
                <Input
                    id="customer-email"
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    placeholder="cliente@email.com"
                />
            </div>

            <div className="space-y-1.5">
                <Label htmlFor="customer-phone">Telefone (WhatsApp)</Label>
                <Input
                    id="customer-phone"
                    value={mode === "edit" ? formatPhoneForDisplay(phone) : phone}
                    onChange={(e) => setPhone(e.target.value)}
                    placeholder="DDD + número (ex.: 11 99999-9999)"
                    disabled={mode === "edit"}
                />
            </div>

            {errors && errors.length > 0 && (
                <ul className="space-y-1 rounded-md border border-red-200 bg-red-50 p-3 text-sm text-red-700">
                    {errors.map((err, i) => (
                        <li key={i}>{err}</li>
                    ))}
                </ul>
            )}

            <div className="flex justify-end gap-2 pt-2">
                <Button type="button" variant="outline" onClick={onCancel} disabled={submitting}>
                    Cancelar
                </Button>
                <Button type="submit" disabled={submitting || !canSubmit}>
                    {submitLabel}
                </Button>
            </div>
        </form>
    );
}
