import { Button, Dialog, DialogDescription, DialogTitle } from "../../../shared/components/ui";
import { useDeleteFlow } from "../hooks/useConversationFlows";
import type { ConversationFlow } from "../types/conversation-flow";

interface DeleteFlowDialogProps {
    flow: ConversationFlow;
    open: boolean;
    onOpenChange: (open: boolean) => void;
    onDeleted?: () => void;
}

export function DeleteFlowDialog({ flow, open, onOpenChange, onDeleted }: DeleteFlowDialogProps) {
    const deleteFlow = useDeleteFlow();

    const close = () => onOpenChange(false);

    return (
        <Dialog open={open} onClose={close}>
            <DialogTitle>Excluir fluxo?</DialogTitle>
            <DialogDescription>
                O fluxo <strong>{flow.name}</strong> e todos os seus nodes e transições serão
                removidos. Esta ação não pode ser desfeita.
            </DialogDescription>
            <div className="mt-5 flex justify-end gap-2">
                <Button variant="outline" onClick={close} disabled={deleteFlow.isPending}>
                    Cancelar
                </Button>
                <Button
                    variant="destructive"
                    disabled={deleteFlow.isPending}
                    onClick={() => {
                        deleteFlow.mutate(flow.id, {
                            onSuccess: () => {
                                close();
                                onDeleted?.();
                            },
                        });
                    }}
                >
                    Excluir
                </Button>
            </div>
        </Dialog>
    );
}