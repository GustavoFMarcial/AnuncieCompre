import { Dialog, DialogDescription, DialogTitle } from "../../../shared/components/ui";
import { useCreateFlow } from "../hooks/useConversationFlows";
import { FlowForm } from "./FlowForm";

interface CreateFlowDialogProps {
    open: boolean;
    onOpenChange: (open: boolean) => void;
    onCreated?: (id: string) => void;
}

export function CreateFlowDialog({ open, onOpenChange, onCreated }: CreateFlowDialogProps) {
    const createFlow = useCreateFlow();

    const close = () => onOpenChange(false);

    return (
        <Dialog open={open} onClose={close}>
            <DialogTitle>Novo fluxo de conversa</DialogTitle>
            <DialogDescription>
                Crie um fluxo em branco. Depois você adiciona os nodes no editor visual.
            </DialogDescription>
            <FlowForm
                submitting={createFlow.isPending}
                submitLabel="Criar fluxo"
                onCancel={close}
                onSubmit={(input) => {
                    createFlow.mutate(input, {
                        onSuccess: (flow) => {
                            close();
                            onCreated?.(flow.id);
                        },
                    });
                }}
            />
        </Dialog>
    );
}