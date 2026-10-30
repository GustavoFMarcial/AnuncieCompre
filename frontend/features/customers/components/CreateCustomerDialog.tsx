import { useState } from "react";

import { Dialog, DialogDescription, DialogTitle } from "../../../shared/components/ui";
import { useCreateCustomer } from "../hooks/useCustomers";
import { extractCustomerErrors } from "../utils/customer-errors";
import { CustomerForm } from "./CustomerForm";

interface CreateCustomerDialogProps {
    open: boolean;
    onOpenChange: (open: boolean) => void;
}

export function CreateCustomerDialog({ open, onOpenChange }: CreateCustomerDialogProps) {
    const [errors, setErrors] = useState<string[]>([]);
    const createCustomer = useCreateCustomer();

    const close = () => {
        setErrors([]);
        onOpenChange(false);
    };

    return (
        <Dialog open={open} onClose={close}>
            <DialogTitle>Novo cliente</DialogTitle>
            <DialogDescription>
                Cadastre um cliente com nome, e-mail e telefone de WhatsApp.
            </DialogDescription>
            <CustomerForm
                mode="create"
                submitting={createCustomer.isPending}
                errors={errors}
                submitLabel="Cadastrar cliente"
                onCancel={close}
                onSubmit={(values) => {
                    setErrors([]);
                    createCustomer.mutate(values, {
                        onSuccess: close,
                        onError: (err) => setErrors(extractCustomerErrors(err)),
                    });
                }}
            />
        </Dialog>
    );
}
