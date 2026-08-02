import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Plus, RefreshCw } from "lucide-react";

import { Button } from "../../../shared/components/ui";
import { useConversationFlows } from "../hooks/useConversationFlows";
import { FlowGrid } from "../components/FlowGrid";
import { FlowPageHeader } from "../components/FlowPageHeader";
import { CreateFlowDialog } from "../components/CreateFlowDialog";

export function ConversationFlowsPage() {
    const { data, isLoading, isError, refetch } = useConversationFlows();
    const navigate = useNavigate();
    const [createOpen, setCreateOpen] = useState(false);

    return (
        <div className="flex flex-col gap-6">
            <FlowPageHeader
                title="Fluxos de conversa"
                description="Crie e edite os fluxos de atendimento do bot."
                action={
                    <div className="flex items-center gap-2">
                        <Button variant="outline" onClick={() => refetch()}>
                            <RefreshCw className="h-4 w-4" />
                            Atualizar
                        </Button>
                        <Button onClick={() => setCreateOpen(true)}>
                            <Plus className="h-4 w-4" />
                            Novo fluxo
                        </Button>
                    </div>
                }
            />

            {isLoading && <p className="text-sm text-neutral-500">Carregando fluxos…</p>}
            {isError && <p className="text-sm text-red-600">Erro ao carregar fluxos.</p>}
            {!isLoading && !isError && data && data.length === 0 && (
                <div className="flex flex-col items-center justify-center gap-3 rounded-lg border border-dashed border-neutral-200 bg-white p-10 text-center">
                    <p className="text-sm text-neutral-500">
                        Nenhum fluxo encontrado. Crie o primeiro para começar.
                    </p>
                    <Button onClick={() => setCreateOpen(true)}>
                        <Plus className="h-4 w-4" />
                        Novo fluxo
                    </Button>
                </div>
            )}
            {data && data.length > 0 && <FlowGrid flows={data} />}

            <CreateFlowDialog
                open={createOpen}
                onOpenChange={setCreateOpen}
                onCreated={(id) => navigate(`/flows/${id}`)}
            />
        </div>
    );
}