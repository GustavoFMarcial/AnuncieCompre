export type NodeValidationKind =
    | "Final"
    | "Option"
    | "Confirmation"
    | "Validation"
    | "OptionValidation";

export type ValueObjectValidator =
    | "None"
    | "Email"
    | "Name"
    | "Quantity"
    | "Product"
    | "CompanyCategory"
    | "CPF"
    | "CNPJ"
    | "Phone"
    | "UserType";

export interface NodeTransition {
    option: string;
    targetNodeId: string;
}

export interface ConversationNode {
    id: string;
    message: string;
    validationKind: NodeValidationKind | null;
    valueObjectValidator: ValueObjectValidator;
    options: string[];
    transitions: NodeTransition[];
    isFinal: boolean;
}

export interface ConversationFlow {
    id: string;
    name: string;
    description: string;
    status: "Draft" | "Published";
    steps: number;
    updatedAt: Date;
    nodes?: ConversationNode[];
}

export type FlowDraft = Omit<ConversationFlow, "id" | "steps" | "updatedAt"> & {
    id?: string;
};

export interface CreateNodeInput {
    message: string;
    validationKind: NodeValidationKind | null;
    valueObjectValidator: ValueObjectValidator;
    options: string[];
    isFinal: boolean;
}

export type UpdateNodeInput = CreateNodeInput;

export interface UpdateTransitionsInput {
    transitions: NodeTransition[];
}