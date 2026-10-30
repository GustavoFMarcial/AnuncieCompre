import { useState } from "react";

import { Dialog, DialogDescription, DialogTitle } from "../../../shared/components/ui";
import { useCustomer, useEditCustomer } from "../hooks/useCustomers";
import { extractCustomerErrors } from "../utils/customer-errors";
import { CustomerForm } from "./CustomerForm";

interface EditCustomerDialogProps {
    customerId: string;
    open: boolean;
    onOpenChange: (open: boolean) => void;
}

export function EditCustomerDialog({ customerId, open, onOpenChange }: EditCustomerDialogProps) {
    const [errors, setErrors] = useState<string[]>([]);
    const { data: customer, isLoading, isError } = useCustomer(customerId, open);
    const editCustomer = useEditCustomer();

    const close = () => {
        setErrors([]);
        onOpenChange(false);
    };

    return (
        <Dialog open={open} onClose={close}>
            <DialogTitle>Editar cliente</DialogTitle>
            <DialogDescription>
                Atualize o nome e o e-mail do cliente. O telefone não pode ser alterado.
            </DialogDescription>
            {isError ? (
                <p className="mt-4 text-sm text-red-600">Erro ao carregar os dados do cliente.</p>
            ) : isLoading || !customer ? (
                <p className="mt-4 text-sm text-neutral-500">Carregando dados do cliente…</p>
            ) : (
                <CustomerForm
                    key={customer.id}
                    mode="edit"
                    initial={customer}
                    submitting={editCustomer.isPending}
                    errors={errors}
                    submitLabel="Salvar alterações"
                    onCancel={close}
                    onSubmit={({ name, email }) => {
                        setErrors([]);
                        editCustomer.mutate(
                            { id: customerId, input: { name, email } },
                            {
                                onSuccess: close,
                                onError: (err) => setErrors(extractCustomerErrors(err)),
                            }
                        );
                    }}
                />
            )}
        </Dialog>
    );
}
