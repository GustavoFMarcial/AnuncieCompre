import { AlertTriangle } from "lucide-react";

import { Button, Dialog, DialogDescription, DialogTitle } from "../../../shared/components/ui";

interface PublishErrorsDialogProps {
    open: boolean;
    onOpenChange: (open: boolean) => void;
    errors: string[];
}

export function PublishErrorsDialog({ open, onOpenChange, errors }: PublishErrorsDialogProps) {
    return (
        <Dialog open={open} onClose={() => onOpenChange(false)}>
            <div className="flex items-start gap-3">
                <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-amber-100 text-amber-600">
                    <AlertTriangle className="h-5 w-5" />
                </div>
                <div className="flex-1">
                    <DialogTitle>Não foi possível publicar</DialogTitle>
                    <DialogDescription>
                        Corrija os problemas abaixo no fluxo e tente publicar novamente.
                    </DialogDescription>
                </div>
            </div>

            <ul className="mt-4 max-h-60 space-y-1.5 overflow-y-auto rounded-md border border-neutral-200 bg-neutral-50 p-3 text-sm text-neutral-700">
                {errors.map((err, i) => (
                    <li key={i} className="flex items-start gap-2">
                        <span className="mt-0.5 text-amber-600">•</span>
                        <span>{err}</span>
                    </li>
                ))}
            </ul>

            <div className="mt-5 flex justify-end">
                <Button onClick={() => onOpenChange(false)}>Entendi</Button>
            </div>
        </Dialog>
    );
}