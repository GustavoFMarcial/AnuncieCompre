export type NodeValidationKind =
    | "final"
    | "option"
    | "confirmation"
    | "validation"
    | "optionValidation";

export type ValueObjectValidator =
    | "none"
    | "email"
    | "name"
    | "quantity"
    | "product"
    | "companyCategory"
    | "cpf"
    | "cnpj"
    | "phone"
    | "userType";

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
    status: "draft" | "published";
    steps: number;
    updatedAt: string | Date;
    nodes?: ConversationNode[];
}

export type FlowDraft = Omit<ConversationFlow, "id" | "steps" | "updatedAt"> & {
    id?: string;
};

export interface CreateFlowInput {
    name: string;
    description: string;
    status: "draft" | "published";
}

export interface UpdateFlowMetaInput {
    name: string;
    description: string;
}

export interface UpdateFlowStatusInput {
    status: "draft" | "published";
}

export interface FlowValidationErrors {
    success: false;
    errors: string[];
}

export interface NodeValidationErrors {
    success: false;
    errors: string[];
}

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