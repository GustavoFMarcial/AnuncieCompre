import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { ArrowLeft, PanelRight, Pencil, Trash2 } from "lucide-react";

import { Button } from "../../../shared/components/ui";
import { FlowCanvas } from "../components/FlowCanvas";
import { NodeEditorPanel } from "../components/NodeEditorPanel";
import { EditFlowDialog } from "../components/EditFlowDialog";
import { DeleteFlowDialog } from "../components/DeleteFlowDialog";
import { useConversationFlow } from "../hooks/useConversationFlows";

export function FlowEditorPage() {
    const { flowId = "" } = useParams();
    const navigate = useNavigate();
    const { data: flow, isLoading, isError } = useConversationFlow(flowId);
    const [selectedNodeId, setSelectedNodeId] = useState<string | null>(null);
    const [panelOpen, setPanelOpen] = useState(true);
    const [editOpen, setEditOpen] = useState(false);
    const [deleteOpen, setDeleteOpen] = useState(false);

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
                <div className="flex items-center gap-2">
                    {flow && (
                        <>
                            <Button size="sm" variant="outline" onClick={() => setEditOpen(true)}>
                                <Pencil className="h-4 w-4" />
                                Editar fluxo
                            </Button>
                            <Button
                                size="sm"
                                variant="ghost"
                                className="text-red-600 hover:bg-red-50"
                                onClick={() => setDeleteOpen(true)}
                            >
                                <Trash2 className="h-4 w-4" />
                                Excluir
                            </Button>
                        </>
                    )}
                    <Button
                        size="sm"
                        variant="outline"
                        onClick={() => setPanelOpen((v) => !v)}
                    >
                        <PanelRight className="h-4 w-4" />
                        {panelOpen ? "Ocultar painel" : "Mostrar painel"}
                    </Button>
                </div>
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

            {flow && (
                <>
                    <EditFlowDialog flow={flow} open={editOpen} onOpenChange={setEditOpen} />
                    <DeleteFlowDialog
                        flow={flow}
                        open={deleteOpen}
                        onOpenChange={setDeleteOpen}
                        onDeleted={() => navigate("/flows")}
                    />
                </>
            )}
        </div>
    );
}