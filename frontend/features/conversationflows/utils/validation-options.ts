import type {
    NodeValidationKind,
    ValueObjectValidator,
} from "../types/conversation-flow";

export function validationKindOptions(): {
    value: "none" | "final" | "option" | "confirmation" | "validation" | "optionValidation";
    label: string;
}[] {
    return [
        { value: "none", label: "Sem validação (só fluxo)" },
        { value: "final", label: "Final" },
        { value: "option", label: "Opção" },
        { value: "confirmation", label: "Confirmação (Sim/Não)" },
        { value: "validation", label: "Validar entrada" },
        { value: "optionValidation", label: "Opção + Validação" },
    ];
}

export function valueObjectValidatorOptions(): { value: ValueObjectValidator; label: string }[] {
    return [
        { value: "none", label: "Nenhum" },
        { value: "email", label: "E-mail" },
        { value: "name", label: "Nome" },
        { value: "quantity", label: "Quantidade" },
        { value: "product", label: "Produto" },
        { value: "companyCategory", label: "Categoria de empresa" },
        { value: "cpf", label: "CPF" },
        { value: "cnpj", label: "CNPJ" },
        { value: "phone", label: "Telefone" },
        { value: "userType", label: "Tipo de usuário" },
    ];
}

export const finalKindsRequiringValueValidator: NodeValidationKind[] = ["validation", "optionValidation"];
export const kindsRequiringOptions: NodeValidationKind[] = ["option", "confirmation", "optionValidation"];