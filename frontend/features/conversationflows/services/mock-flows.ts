import type { ConversationFlow, ConversationNode } from "../types/conversation-flow";

const startNode: ConversationNode = {
    id: "start",
    message: "Olá! Deseja fazer um pedido? Digite 1 para Sim ou 2 para Não.",
    validationKind: "Confirmation",
    valueObjectValidator: "None",
    options: ["1", "2"],
    transitions: [
        { option: "1", targetNodeId: "ask_company_category" },
        { option: "2", targetNodeId: "finish" },
    ],
    isFinal: false,
};

const askCategoryNode: ConversationNode = {
    id: "ask_company_category",
    message: "Qual a categoria da sua empresa? (Restaurante, Mercado, Padaria...)",
    validationKind: "Validation",
    valueObjectValidator: "CompanyCategory",
    options: [],
    transitions: [{ option: "next", targetNodeId: "ask_product" }],
    isFinal: false,
};

const askProductNode: ConversationNode = {
    id: "ask_product",
    message: "Qual produto você deseja anunciar?",
    validationKind: "Validation",
    valueObjectValidator: "Product",
    options: [],
    transitions: [{ option: "next", targetNodeId: "ask_quantity" }],
    isFinal: false,
};

const askQuantityNode: ConversationNode = {
    id: "ask_quantity",
    message: "Quantas unidades?",
    validationKind: "Validation",
    valueObjectValidator: "Quantity",
    options: [],
    transitions: [{ option: "next", targetNodeId: "order_ask_confirmation" }],
    isFinal: false,
};

const orderConfirmNode: ConversationNode = {
    id: "order_ask_confirmation",
    message: "Confirma o pedido? 1 para Sim, 2 para Não.",
    validationKind: "Confirmation",
    valueObjectValidator: "None",
    options: ["1", "2"],
    transitions: [
        { option: "1", targetNodeId: "ask_registration" },
        { option: "2", targetNodeId: "ask_product" },
    ],
    isFinal: false,
};

const askRegistrationNode: ConversationNode = {
    id: "ask_registration",
    message: "Deseja se cadastrar? 1 para Sim, 2 para Não.",
    validationKind: "Confirmation",
    valueObjectValidator: "None",
    options: ["1", "2"],
    transitions: [
        { option: "1", targetNodeId: "ask_name" },
        { option: "2", targetNodeId: "finish" },
    ],
    isFinal: false,
};

const askNameNode: ConversationNode = {
    id: "ask_name",
    message: "Qual o seu nome?",
    validationKind: "Validation",
    valueObjectValidator: "Name",
    options: [],
    transitions: [{ option: "next", targetNodeId: "ask_email" }],
    isFinal: false,
};

const askEmailNode: ConversationNode = {
    id: "ask_email",
    message: "Qual o seu e-mail?",
    validationKind: "Validation",
    valueObjectValidator: "Email",
    options: [],
    transitions: [{ option: "next", targetNodeId: "registration_ask_confirmation" }],
    isFinal: false,
};

const registrationConfirmNode: ConversationNode = {
    id: "registration_ask_confirmation",
    message: "Confirma os dados? 1 para Sim, 2 para Não.",
    validationKind: "Confirmation",
    valueObjectValidator: "None",
    options: ["1", "2"],
    transitions: [
        { option: "1", targetNodeId: "ask_another_order" },
        { option: "2", targetNodeId: "ask_name" },
    ],
    isFinal: false,
};

const anotherOrderNode: ConversationNode = {
    id: "ask_another_order",
    message: "Deseja fazer outro pedido? 1 para Sim, 2 para Não.",
    validationKind: "Confirmation",
    valueObjectValidator: "None",
    options: ["1", "2"],
    transitions: [
        { option: "1", targetNodeId: "ask_product" },
        { option: "2", targetNodeId: "finish" },
    ],
    isFinal: false,
};

const finishNode: ConversationNode = {
    id: "finish",
    message: "Obrigado! Atendimento finalizado.",
    validationKind: "Final",
    valueObjectValidator: "None",
    options: [],
    transitions: [],
    isFinal: true,
};

const mainFlowNodes: ConversationNode[] = [
    startNode,
    askCategoryNode,
    askProductNode,
    askQuantityNode,
    orderConfirmNode,
    askRegistrationNode,
    askNameNode,
    askEmailNode,
    registrationConfirmNode,
    anotherOrderNode,
    finishNode,
];

export const mockFlows: ConversationFlow[] = [
    {
        id: "1",
        name: "Atendimento Principal",
        description: "Fluxo principal do WhatsApp",
        status: "Published",
        steps: mainFlowNodes.length,
        updatedAt: new Date("2026-07-28"),
        nodes: mainFlowNodes,
    },
    {
        id: "2",
        name: "Suporte",
        description: "Fluxo de pós-venda",
        status: "Draft",
        steps: 4,
        updatedAt: new Date("2026-07-25"),
        nodes: [
            {
                id: "support_start",
                message: "Olá! Você precisa de suporte? 1 Sim, 2 Não.",
                validationKind: "Confirmation",
                valueObjectValidator: "None",
                options: ["1", "2"],
                transitions: [
                    { option: "1", targetNodeId: "support_problem" },
                    { option: "2", targetNodeId: "support_end" },
                ],
                isFinal: false,
            },
            {
                id: "support_problem",
                message: "Descreva o seu problema.",
                validationKind: null,
                valueObjectValidator: "None",
                options: [],
                transitions: [{ option: "next", targetNodeId: "support_end" }],
                isFinal: false,
            },
            {
                id: "support_end",
                message: "Suporte finalizado. Obrigado!",
                validationKind: "Final",
                valueObjectValidator: "None",
                options: [],
                transitions: [],
                isFinal: true,
            },
        ],
    },
];

function cloneMock(): ConversationFlow[] {
    return structuredClone(mockFlows) as ConversationFlow[];
}

function genFlowId(): string {
    return `flow_${Date.now()}_${Math.random().toString(36).slice(2, 8)}`;
}

export const mockStore = {
    flows: cloneMock(),

    createFlow(input: { name: string; description: string; status: "Draft" | "Published" }): ConversationFlow {
        const flow: ConversationFlow = {
            id: genFlowId(),
            name: input.name,
            description: input.description,
            status: input.status,
            steps: 0,
            updatedAt: new Date(),
            nodes: [],
        };
        this.flows.push(flow);
        return structuredClone(flow) as ConversationFlow;
    },

    updateFlow(id: string, input: { name: string; description: string; status: "Draft" | "Published" }): void {
        const flow = this.flows.find((f) => f.id === id);
        if (!flow) throw new Error("Fluxo não encontrado");
        flow.name = input.name;
        flow.description = input.description;
        flow.status = input.status;
        flow.updatedAt = new Date();
    },

    deleteFlow(id: string): void {
        const idx = this.flows.findIndex((f) => f.id === id);
        if (idx === -1) throw new Error("Fluxo não encontrado");
        this.flows.splice(idx, 1);
    },
};