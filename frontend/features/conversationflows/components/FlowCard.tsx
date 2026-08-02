import { useNavigate } from "react-router-dom";
import { Edit, Layers } from "lucide-react";

import { Badge, Button, Card } from "../../../shared/components/ui";
import type { ConversationFlow } from "../types/conversation-flow";

interface FlowCardProps {
    flow: ConversationFlow;
}

export function FlowCard({ flow }: FlowCardProps) {
    const navigate = useNavigate();

    return (
        <Card className="flex flex-col gap-3 p-5">
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
                <Badge variant={flow.status === "Published" ? "success" : "warning"}>
                    {flow.status === "Published" ? "Publicado" : "Rascunho"}
                </Badge>
            </div>

            <div className="flex items-center justify-between border-t border-neutral-100 pt-3 text-xs text-neutral-500">
                <span>{flow.steps} nodes</span>
                <span>Atualizado em {flow.updatedAt.toLocaleDateString("pt-BR")}</span>
            </div>

            <Button size="sm" variant="outline" onClick={() => navigate(`/flows/${flow.id}`)}>
                <Edit className="h-4 w-4" />
                Editar fluxo
            </Button>
        </Card>
    );
}