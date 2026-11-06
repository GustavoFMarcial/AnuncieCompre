import { useState } from "react";
import { Building2, CheckCircle2 } from "lucide-react";

import {
    Button,
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "../../../shared/components/ui";
import { CompanyRegistrationForm } from "../components/CompanyRegistrationForm";
import { useCreateCompany } from "../hooks/useCreateCompany";
import { extractCompanyErrors } from "../utils/company-errors";
import type { Company } from "../types/company";

export function RegisterCompanyPage() {
    const [errors, setErrors] = useState<string[]>([]);
    const [createdCompany, setCreatedCompany] = useState<Company | null>(null);
    const createCompany = useCreateCompany();

    const reset = () => {
        setCreatedCompany(null);
        setErrors([]);
    };

    if (createdCompany) {
        return (
            <div className="flex min-h-screen items-center justify-center bg-neutral-50 p-6">
                <div className="w-full max-w-lg">
                    <Card>
                        <CardHeader>
                            <CardTitle>Empresa cadastrada com sucesso</CardTitle>
                            <CardDescription>
                                {createdCompany.name} foi criada junto com o acesso do administrador.
                                Use o login e a senha informados para acessar o sistema.
                            </CardDescription>
                        </CardHeader>
                        <CardContent className="flex flex-col items-center gap-4">
                            <CheckCircle2 className="h-12 w-12 text-green-600" />
                            <Button variant="outline" onClick={reset}>
                                Cadastrar outra empresa
                            </Button>
                        </CardContent>
                    </Card>
                </div>
            </div>
        );
    }

    return (
        <div className="flex min-h-screen items-center justify-center bg-neutral-50 p-6">
            <div className="w-full max-w-lg">
                <Card>
                    <CardHeader>
                        <CardTitle className="flex items-center gap-2">
                            <Building2 className="h-5 w-5" />
                            Cadastre sua empresa
                        </CardTitle>
                        <CardDescription>
                            Crie a empresa e o acesso inicial do administrador para começar a usar
                            o AnuncieCompre.
                        </CardDescription>
                    </CardHeader>
                    <CardContent>
                        <CompanyRegistrationForm
                            submitting={createCompany.isPending}
                            errors={errors}
                            onSubmit={(values) => {
                                setErrors([]);
                                createCompany.mutate(values, {
                                    onSuccess: (company) => setCreatedCompany(company),
                                    onError: (err) => setErrors(extractCompanyErrors(err)),
                                });
                            }}
                        />
                    </CardContent>
                </Card>
            </div>
        </div>
    );
}
