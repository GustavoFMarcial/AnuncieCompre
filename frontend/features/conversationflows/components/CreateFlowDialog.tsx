import { useState } from "react";
import { Dialog, DialogDescription, DialogTitle } from "../../../shared/components/ui";
import { useCreateFlow } from "../hooks/useConversationFlows";
import { ErrorsDialog } from "./ErrorsDialog";
import { FlowForm } from "./FlowForm";
import type { FlowValidationErrors } from "../types/conversation-flow";
import { AxiosError } from "axios";

interface CreateFlowDialogProps {
    open: boolean;
    onOpenChange: (open: boolean) => void;
    onCreated?: (id: string) => void;
}

export function CreateFlowDialog({ open, onOpenChange, onCreated }: CreateFlowDialogProps) {
    const [errorsOpen, setErrorsOpen] = useState(false);
    const [errors, setErrors] = useState<string[]>([]);
    const createFlow = useCreateFlow();

    const close = () => onOpenChange(false);

    return (
        <div>
            <Dialog open={open} onClose={close}>
                <DialogTitle>Novo fluxo de conversa</DialogTitle>
                <DialogDescription>
                    Crie um fluxo em rascunho. Depois você adiciona os nodes no editor visual.
                </DialogDescription>
                <FlowForm
                    submitting={createFlow.isPending}
                    submitLabel="Criar fluxo"
                    onCancel={close}
                    onSubmit={({ name, description }) => {
                        createFlow.mutate(
                            { name, description, status: "draft" },
                            {
                                onSuccess: (flow) => {
                                    close();
                                    onCreated?.(flow.id);
                                },
                                onError: (err) => {
                                    const axiosErr = err as AxiosError<FlowValidationErrors>;
                                    const msgs = axiosErr.response?.data?.errors;
                                    if (msgs && msgs.length > 0) {
                                        setErrors(msgs);
                                        setErrorsOpen(true);
                                    }
                                }
                            }
                        );
                    }}
                />
            </Dialog>
            <ErrorsDialog
                open={errorsOpen}
                onOpenChange={setErrorsOpen}
                errors={errors}
            />
        </div>
    );
}