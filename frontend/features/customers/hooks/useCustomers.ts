import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

import { customersService } from "../services/customers.service";
import type { CreateCustomerInput, EditCustomerInput } from "../types/customer";

const CUSTOMERS_KEY = ["customers"] as const;

export function useCustomers() {
    return useQuery({
        queryKey: CUSTOMERS_KEY,
        queryFn: () => customersService.list(),
    });
}

const customerKey = (id: string) => ["customer", id] as const;

export function useCustomer(id: string, enabled: boolean = true) {
    return useQuery({
        queryKey: customerKey(id),
        queryFn: () => customersService.getById(id),
        enabled: enabled && !!id,
    });
}

export function useCreateCustomer() {
    const qc = useQueryClient();
    return useMutation({
        mutationFn: (input: CreateCustomerInput) => customersService.create(input),
        onSuccess: () => {
            qc.invalidateQueries({ queryKey: CUSTOMERS_KEY });
        },
    });
}

export function useEditCustomer() {
    const qc = useQueryClient();
    return useMutation({
        mutationFn: ({ id, input }: { id: string; input: EditCustomerInput }) =>
            customersService.edit(id, input),
        onSuccess: (_data, vars) => {
            qc.invalidateQueries({ queryKey: customerKey(vars.id) });
            qc.invalidateQueries({ queryKey: CUSTOMERS_KEY });
            qc.invalidateQueries({ queryKey: ["conversations"] });
        },
    });
}
