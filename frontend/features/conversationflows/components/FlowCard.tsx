import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Edit, Layers, MoreVertical, Pencil, Trash2 } from "lucide-react";

import {
    Badge,
    Button,
    DropdownMenu,
    DropdownMenuItem,
    DropdownMenuSeparator,
} from "../../../shared/components/ui";
import type { ConversationFlow } from "../types/conversation-flow";
import { EditFlowDialog } from "./EditFlowDialog";
import { DeleteFlowDialog } from "./DeleteFlowDialog";

interface FlowCardProps {
    flow: ConversationFlow;
}

export function FlowCard({ flow }: FlowCardProps) {
    const navigate = useNavigate();
    const [editOpen, setEditOpen] = useState(false);
    const [deleteOpen, setDeleteOpen] = useState(false);

    return (
        <>
            <div className="flex flex-col gap-3 rounded-lg border border-neutral-200 bg-white p-5 shadow-sm">
                <div className="flex items-start justify-between gap-2">
                    <div className="flex items-center gap-2">
                        <div className="flex h-9 w-9 items-center justify-center rounded-md bg-neutral-100 text-neutral-600">
                            <Layers className="h-5 w-5" />
                        </div>
                        <div>
                            <h3 className="font-semibold text-neutral-900">{flow.name}</h3>
                            <p className="text-xs text-neutral-500">{flow.description}</p>
                        </div>
                    </div>
                    <div className="flex items-center gap-1.5">
                        <Badge variant={flow.status === "Published" ? "success" : "warning"}>
                            {flow.status === "Published" ? "Publicado" : "Rascunho"}
                        </Badge>
                        <DropdownMenu
                            trigger={<MoreVertical className="h-4 w-4" />}
                            align="end"
                        >
                            <DropdownMenuItem
                                icon={<Pencil className="h-4 w-4" />}
                                onClick={() => setEditOpen(true)}
                            >
                                Editar fluxo
                            </DropdownMenuItem>
                            <DropdownMenuSeparator />
                            <DropdownMenuItem
                                destructive
                                icon={<Trash2 className="h-4 w-4" />}
                                onClick={() => setDeleteOpen(true)}
                            >
                                Excluir fluxo
                            </DropdownMenuItem>
                        </DropdownMenu>
                    </div>
                </div>

                <div className="flex items-center justify-between border-t border-neutral-100 pt-3 text-xs text-neutral-500">
                    <span>{flow.steps} nodes</span>
                    <span>Atualizado em {flow.updatedAt.toLocaleDateString("pt-BR")}</span>
                </div>

                <Button
                    size="sm"
                    variant="outline"
                    onClick={() => navigate(`/flows/${flow.id}`)}
                >
                    <Edit className="h-4 w-4" />
                    Abrir editor
                </Button>
            </div>

            <EditFlowDialog flow={flow} open={editOpen} onOpenChange={setEditOpen} />
            <DeleteFlowDialog
                flow={flow}
                open={deleteOpen}
                onOpenChange={setDeleteOpen}
            />
        </>
    );
}