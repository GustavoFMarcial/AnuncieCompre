import { useState, type FormEvent } from "react";

import { Button, Input, Label, Separator } from "../../../shared/components/ui";
import { maskCnpj, maskCpf, maskPhone } from "../utils/masks";
import type { CreateCompanyInput } from "../types/company";

interface CompanyRegistrationFormProps {
    submitting?: boolean;
    errors?: string[];
    submitLabel?: string;
    onSubmit: (values: CreateCompanyInput) => void;
}

const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

// Mesma política exigida pelo Identity no backend.
function isPasswordValid(password: string): boolean {
    return (
        password.length >= 6 &&
        /[a-z]/.test(password) &&
        /[A-Z]/.test(password) &&
        /\d/.test(password) &&
        /[^a-zA-Z0-9]/.test(password)
    );
}

export function CompanyRegistrationForm({
    submitting,
    errors,
    submitLabel = "Criar empresa",
    onSubmit,
}: CompanyRegistrationFormProps) {
    const [companyName, setCompanyName] = useState("");
    const [cnpj, setCnpj] = useState("");
    const [email, setEmail] = useState("");
    const [phone, setPhone] = useState("");
    const [cpf, setCpf] = useState("");
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");

    const canSubmit =
        companyName.trim().length > 0 &&
        cnpj.replace(/\D/g, "").length === 14 &&
        EMAIL_PATTERN.test(email.trim()) &&
        phone.replace(/\D/g, "").length >= 10 &&
        cpf.replace(/\D/g, "").length === 11 &&
        login.trim().length > 0 &&
        isPasswordValid(password);

    const handleSubmit = (e: FormEvent) => {
        e.preventDefault();
        if (!canSubmit) return;
        onSubmit({
            companyName: companyName.trim(),
            cnpj,
            email: email.trim(),
            phone,
            cpf,
            login: login.trim(),
            password,
        });
    };

    return (
        <form onSubmit={handleSubmit} className="flex flex-col gap-5">
            <section className="flex flex-col gap-4">
                <h3 className="text-sm font-semibold text-neutral-900">Dados da empresa</h3>
                <div className="space-y-1.5">
                    <Label htmlFor="company-name">Nome da empresa</Label>
                    <Input
                        id="company-name"
                        value={companyName}
                        onChange={(e) => setCompanyName(e.target.value)}
                        placeholder="Ex.: Acme Comércio Ltda"
                        autoFocus
                    />
                </div>

                <div className="space-y-1.5">
                    <Label htmlFor="company-cnpj">CNPJ</Label>
                    <Input
                        id="company-cnpj"
                        value={cnpj}
                        onChange={(e) => setCnpj(maskCnpj(e.target.value))}
                        placeholder="00.000.000/0000-00"
                        inputMode="numeric"
                    />
                </div>

                <div className="space-y-1.5">
                    <Label htmlFor="company-email">E-mail</Label>
                    <Input
                        id="company-email"
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        placeholder="contato@empresa.com.br"
                    />
                </div>

                <div className="space-y-1.5">
                    <Label htmlFor="company-phone">Telefone (WhatsApp)</Label>
                    <Input
                        id="company-phone"
                        value={phone}
                        onChange={(e) => setPhone(maskPhone(e.target.value))}
                        placeholder="(11) 99999-9999"
                        inputMode="numeric"
                    />
                    <p className="text-xs text-neutral-500">
                        O e-mail e o telefone da empresa também serão usados para o acesso do
                        administrador.
                    </p>
                </div>
            </section>

            <Separator />

            <section className="flex flex-col gap-4">
                <h3 className="text-sm font-semibold text-neutral-900">Acesso do administrador</h3>
                <div className="space-y-1.5">
                    <Label htmlFor="admin-cpf">CPF</Label>
                    <Input
                        id="admin-cpf"
                        value={cpf}
                        onChange={(e) => setCpf(maskCpf(e.target.value))}
                        placeholder="000.000.000-00"
                        inputMode="numeric"
                    />
                </div>

                <div className="space-y-1.5">
                    <Label htmlFor="admin-login">Login</Label>
                    <Input
                        id="admin-login"
                        value={login}
                        onChange={(e) => setLogin(e.target.value)}
                        placeholder="Ex.: admin.acme"
                    />
                </div>

                <div className="space-y-1.5">
                    <Label htmlFor="admin-password">Senha</Label>
                    <Input
                        id="admin-password"
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        placeholder="Senha de acesso"
                    />
                    <p className="text-xs text-neutral-500">
                        Mínimo de 6 caracteres com letra maiúscula, minúscula, número e símbolo.
                    </p>
                </div>
            </section>

            {errors && errors.length > 0 && (
                <ul className="space-y-1 rounded-md border border-red-200 bg-red-50 p-3 text-sm text-red-700">
                    {errors.map((err, i) => (
                        <li key={i}>{err}</li>
                    ))}
                </ul>
            )}

            <Button type="submit" disabled={submitting || !canSubmit} className="w-full">
                {submitLabel}
            </Button>
        </form>
    );
}
