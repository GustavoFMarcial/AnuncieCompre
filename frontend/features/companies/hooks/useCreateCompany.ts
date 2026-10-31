import { useMutation } from "@tanstack/react-query";

import { companiesService } from "../services/company.service";
import type { CreateCompanyInput } from "../types/company";

export function useCreateCompany() {
    return useMutation({
        mutationFn: (input: CreateCompanyInput) => companiesService.create(input),
    });
}
