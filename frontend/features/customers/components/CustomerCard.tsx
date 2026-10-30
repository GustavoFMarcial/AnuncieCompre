import { useState } from "react";
import { Mail, MoreVertical, Pencil, UserRound } from "lucide-react";

import { Badge, DropdownMenu, DropdownMenuItem } from "../../../shared/components/ui";
import { formatPhoneForDisplay } from "../../../shared/utils/phone";
import type { Customer } from "../types/customer";
import { EditCustomerDialog } from "./EditCustomerDialog";

interface CustomerCardProps {
    customer: Customer;
}

export function CustomerCard({ customer }: CustomerCardProps) {
    const [editOpen, setEditOpen] = useState(false);
    const incomplete = !customer.name || !customer.email;

    return (
        <>
            <div className="flex flex-col gap-3 rounded-lg border border-neutral-200 bg-white p-5 shadow-sm">
                <div className="flex items-start justify-between gap-2">
                    <div className="flex min-w-0 items-center gap-2">
                        <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-neutral-100 text-neutral-600">
                            {customer.name ? (
                                <span className="text-sm font-semibold">
                                    {customer.name.charAt(0).toUpperCase()}
                                </span>
                            ) : (
                                <UserRound className="h-5 w-5" />
                            )}
                        </div>
                        <div className="min-w-0">
                            <h3 className="truncate font-semibold text-neutral-900">
                                {customer.name ?? "Não informado"}
                            </h3>
                            <p className="text-xs text-neutral-500">
                                {formatPhoneForDisplay(customer.phone)}
                            </p>
                        </div>
                    </div>
                    <div className="flex shrink-0 items-center gap-1.5">
                        {incomplete && <Badge variant="warning">Cadastro incompleto</Badge>}
                        <DropdownMenu trigger={<MoreVertical className="h-4 w-4" />} align="end">
                            <DropdownMenuItem
                                icon={<Pencil className="h-4 w-4" />}
                                onClick={() => setEditOpen(true)}
                            >
                                Editar dados
                            </DropdownMenuItem>
                        </DropdownMenu>
                    </div>
                </div>

                <div className="flex items-center gap-1.5 border-t border-neutral-100 pt-3 text-xs text-neutral-500">
                    <Mail className="h-3.5 w-3.5 shrink-0" />
                    <span className="truncate">{customer.email ?? "E-mail não informado"}</span>
                </div>
            </div>

            <EditCustomerDialog
                customerId={customer.id}
                open={editOpen}
                onOpenChange={setEditOpen}
            />
        </>
    );
}
