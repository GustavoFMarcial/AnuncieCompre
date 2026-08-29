import { Dialog, DialogDescription, DialogTitle } from "../../../shared/components/ui";
import { useUpdateFlow } from "../hooks/useConversationFlows";
import type { ConversationFlow } from "../types/conversation-flow";
import { FlowForm } from "./FlowForm";

interface EditFlowDialogProps {
    flow: ConversationFlow;
    open: boolean;
    onOpenChange: (open: boolean) => void;
}

export function EditFlowDialog({ flow, open, onOpenChange }: EditFlowDialogProps) {
    const updateFlow = useUpdateFlow();

    const close = () => onOpenChange(false);

    return (
        <Dialog open={open} onClose={close}>
            <DialogTitle>Editar fluxo</DialogTitle>
            <DialogDescription>
                Atualize o nome e a descrição do fluxo. O status (rascunho/publicado) é alterado
                pelo botão dedicado no editor.
            </DialogDescription>
            <FlowForm
                key={`${flow.id}-${flow.updatedAt.getTime()}`}
                initial={{ name: flow.name, description: flow.description }}
                submitting={updateFlow.isPending}
                submitLabel="Salvar alterações"
                onCancel={close}
                onSubmit={({ name, description }) => {
                    updateFlow.mutate(
                        { id: flow.id, input: { name, description } },
                        { onSuccess: close }
                    );
                }}
            />
        </Dialog>
    );
}