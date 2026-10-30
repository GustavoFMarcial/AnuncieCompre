import { useState } from "react";
import { Plus, RefreshCw, Users } from "lucide-react";

import { Button } from "../../../shared/components/ui";
import { PageHeader } from "../../../shared/components/PageHeader";
import { useCustomers } from "../hooks/useCustomers";
import { CreateCustomerDialog } from "../components/CreateCustomerDialog";
import { CustomerGrid } from "../components/CustomerGrid";

export function CustomersPage() {
    const { data, isLoading, isError, refetch } = useCustomers();
    const [createOpen, setCreateOpen] = useState(false);

    return (
        <div className="flex flex-col gap-6">
            <PageHeader
                title="Clientes"
                description="Veja todos os clientes que chegaram pelo WhatsApp e complete os cadastros."
                action={
                    <div className="flex items-center gap-2">
                        <Button variant="outline" onClick={() => refetch()}>
                            <RefreshCw className="h-4 w-4" />
                            Atualizar
                        </Button>
                        <Button onClick={() => setCreateOpen(true)}>
                            <Plus className="h-4 w-4" />
                            Novo cliente
                        </Button>
                    </div>
                }
            />

            {isLoading && <p className="text-sm text-neutral-500">Carregando clientes…</p>}
            {isError && <p className="text-sm text-red-600">Erro ao carregar clientes.</p>}
            {!isLoading && !isError && data && data.length === 0 && (
                <div className="flex flex-col items-center justify-center gap-3 rounded-lg border border-dashed border-neutral-200 bg-white p-10 text-center">
                    <Users className="h-10 w-10 text-neutral-300" />
                    <p className="text-sm text-neutral-500">
                        Nenhum cliente encontrado. Cadastre o primeiro para começar.
                    </p>
                    <Button onClick={() => setCreateOpen(true)}>
                        <Plus className="h-4 w-4" />
                        Novo cliente
                    </Button>
                </div>
            )}
            {data && data.length > 0 && <CustomerGrid customers={data} />}

            <CreateCustomerDialog open={createOpen} onOpenChange={setCreateOpen} />
        </div>
    );
}
