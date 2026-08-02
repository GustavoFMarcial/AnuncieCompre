import type {
    NodeValidationKind,
    ValueObjectValidator,
} from "../types/conversation-flow";

export function validationKindOptions(): {
    value: "" | "Final" | "Option" | "Confirmation" | "Validation" | "OptionValidation";
    label: string;
}[] {
    return [
        { value: "", label: "Sem validação (só fluxo)" },
        { value: "Final", label: "Final" },
        { value: "Option", label: "Opção" },
        { value: "Confirmation", label: "Confirmação (Sim/Não)" },
        { value: "Validation", label: "Validar entrada" },
        { value: "OptionValidation", label: "Opção + Validação" },
    ];
}

export function valueObjectValidatorOptions(): { value: ValueObjectValidator; label: string }[] {
    return [
        { value: "None", label: "Nenhum" },
        { value: "Email", label: "E-mail" },
        { value: "Name", label: "Nome" },
        { value: "Quantity", label: "Quantidade" },
        { value: "Product", label: "Produto" },
        { value: "CompanyCategory", label: "Categoria de empresa" },
        { value: "CPF", label: "CPF" },
        { value: "CNPJ", label: "CNPJ" },
        { value: "Phone", label: "Telefone" },
        { value: "UserType", label: "Tipo de usuário" },
    ];
}

export const finalKindsRequiringValueValidator: NodeValidationKind[] = ["Validation", "OptionValidation"];
export const kindsRequiringOptions: NodeValidationKind[] = ["Option", "Confirmation", "OptionValidation"];