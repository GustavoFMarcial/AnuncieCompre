import { useState } from "react";
import { ArrowLeft, ArrowRight } from "lucide-react";
import { useNavigate, useParams } from "react-router-dom";

import { Button } from "../../../shared/components/ui";
import { FlowCanvas } from "../components/FlowCanvas";
import { NodeEditorPanel } from "../components/NodeEditorPanel";
import { useConversationFlow } from "../hooks/useConversationFlows";

export function FlowEditorPage() {
    const { flowId = "" } = useParams();
    const navigate = useNavigate();
    const { data: flow, isLoading, isError } = useConversationFlow(flowId);
    const [selectedNodeId, setSelectedNodeId] = useState<string | null>(null);
    const [panelOpen, setPanelOpen] = useState(true);

    const selectedNode = flow?.nodes?.find((n) => n.id === selectedNodeId) ?? null;

    return (
        <div className="-m-8 flex h-full flex-col">
            <div className="flex items-center justify-between gap-3 border-b border-neutral-200 bg-white px-6 py-3">
                <div className="flex items-center gap-3">
                    <Button size="icon" variant="ghost" onClick={() => navigate("/flows")}>
                        <ArrowLeft className="h-4 w-4" />
                    </Button>
                    <div>
                        <h1 className="text-lg font-semibold text-neutral-900">
                            {flow?.name ?? "Fluxo"}
                        </h1>
                        <p className="text-xs text-neutral-500">
                            {flow?.nodes?.length ?? 0} nodes
                        </p>
                    </div>
                </div>
                <Button
                    size="sm"
                    variant="outline"
                    onClick={() => setPanelOpen((v) => !v)}
                >
                    {panelOpen ? "Ocultar painel" : "Mostrar painel"}
                    <ArrowRight className="h-4 w-4" />
                </Button>
            </div>

            <div className="flex flex-1 overflow-hidden">
                <div className="relative flex flex-1 flex-col">
                    {isLoading && (
                        <div className="flex h-full items-center justify-center text-sm text-neutral-500">
                            Carregando fluxo…
                        </div>
                    )}
                    {isError && (
                        <div className="flex h-full items-center justify-center text-sm text-red-600">
                            Erro ao carregar o fluxo.
                        </div>
                    )}
                    {!isLoading && !isError && (
                        <FlowCanvas
                            flowId={flowId}
                            selectedNodeId={selectedNodeId}
                            onSelectNode={(id) => {
                                setSelectedNodeId(id);
                                if (id) setPanelOpen(true);
                            }}
                        />
                    )}
                </div>

                {panelOpen && (
                    <NodeEditorPanel
                        key={selectedNode?.id ?? "none"}
                        flowId={flowId}
                        node={selectedNode}
                        nodes={flow?.nodes ?? []}
                        onClose={() => {
                            setSelectedNodeId(null);
                            setPanelOpen(false);
                        }}
                    />
                )}
            </div>
        </div>
    );
}