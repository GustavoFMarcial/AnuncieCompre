import { useConversationFlows } from "../hooks/useConversationFlows";
import { FlowGrid } from "../components/FlowGrid";
import { FlowPageHeader } from "../components/FlowPageHeader";
import { Button } from "../../../shared/components/ui";

export function ConversationFlowsPage() {
    const { data, isLoading, isError, refetch } = useConversationFlows();

    return (
        <div className="flex flex-col gap-6">
            <FlowPageHeader
                title="Fluxos de conversa"
                description="Crie e edite os fluxos de atendimento do bot."
                action={
                    <Button variant="outline" onClick={() => refetch()}>
                        Atualizar
                    </Button>
                }
            />

            {isLoading && <p className="text-sm text-neutral-500">Carregando fluxos…</p>}
            {isError && <p className="text-sm text-red-600">Erro ao carregar fluxos.</p>}
            {!isLoading && !isError && data && data.length === 0 && (
                <p className="text-sm text-neutral-500">Nenhum fluxo encontrado.</p>
            )}
            {data && data.length > 0 && <FlowGrid flows={data} />}
        </div>
    );
}