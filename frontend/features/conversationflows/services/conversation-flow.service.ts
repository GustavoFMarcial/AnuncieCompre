import { api } from "../../../shared/lib/api";
import type {
    ConversationFlow,
    CreateNodeInput,
    UpdateNodeInput,
    UpdateTransitionsInput,
} from "../types/conversation-flow";
import { mockStore } from "./mock-flows";

const wait = (ms: number) => new Promise((r) => setTimeout(r, ms));

function genId(): string {
    return `node_${Date.now()}_${Math.random().toString(36).slice(2, 8)}`;
}

async function withFallback<T>(real: () => Promise<T>, fallback: () => Promise<T> | T): Promise<T> {
    try {
        return await real();
    } catch (err) {
        if (import.meta.env.DEV) {
            console.warn("[conversation-flow.service] usando mock fallback", err);
        }
        return await fallback();
    }
}

export const conversationFlowService = {
    async getAll(): Promise<ConversationFlow[]> {
        return withFallback(
            async () => (await api.get<ConversationFlow[]>("/api/conversation-flows")).data,
            () => structuredClone(mockStore.flows) as ConversationFlow[]
        );
    },

    async getById(id: string): Promise<ConversationFlow> {
        return withFallback(
            async () => (await api.get<ConversationFlow>(`/api/conversation-flows/${id}`)).data,
            () => {
                const found = mockStore.flows.find((f) => f.id === id);
                if (!found) throw new Error("Fluxo não encontrado");
                return structuredClone(found) as ConversationFlow;
            }
        );
    },

    async createNode(flowId: string, input: CreateNodeInput): Promise<void> {
        await withFallback(
            async () => {
                await api.post(`/api/conversation-flows/${flowId}/nodes`, input);
            },
            async () => {
                await wait(200);
                const flow = mockStore.flows.find((f) => f.id === flowId);
                if (!flow) throw new Error("Fluxo não encontrado");
                flow.nodes = flow.nodes ?? [];
                flow.nodes.push({
                    id: genId(),
                    ...input,
                    transitions: [],
                });
                flow.steps = flow.nodes.length;
                flow.updatedAt = new Date();
            }
        );
    },

    async updateNode(flowId: string, nodeId: string, input: UpdateNodeInput): Promise<void> {
        await withFallback(
            async () => {
                await api.put(`/api/conversation-flows/${flowId}/nodes/${nodeId}`, input);
            },
            async () => {
                await wait(200);
                const flow = mockStore.flows.find((f) => f.id === flowId);
                const node = flow?.nodes?.find((n) => n.id === nodeId);
                if (!node) throw new Error("Node não encontrado");
                node.message = input.message;
                node.validationKind = input.validationKind;
                node.valueObjectValidator = input.valueObjectValidator;
                node.options = input.options;
                node.isFinal = input.isFinal;
                flow!.updatedAt = new Date();
            }
        );
    },

    async deleteNode(flowId: string, nodeId: string): Promise<void> {
        await withFallback(
            async () => {
                await api.delete(`/api/conversation-flows/${flowId}/nodes/${nodeId}`);
            },
            async () => {
                await wait(200);
                const flow = mockStore.flows.find((f) => f.id === flowId);
                if (!flow?.nodes) return;
                flow.nodes = flow.nodes.filter((n) => n.id !== nodeId);
                flow.nodes.forEach((n) => {
                    n.transitions = n.transitions.filter((t) => t.targetNodeId !== nodeId);
                });
                flow.steps = flow.nodes.length;
                flow.updatedAt = new Date();
            }
        );
    },

    async updateTransitions(flowId: string, nodeId: string, input: UpdateTransitionsInput): Promise<void> {
        await withFallback(
            async () => {
                await api.patch(
                    `/api/conversation-flows/${flowId}/nodes/${nodeId}/transitions`,
                    input
                );
            },
            async () => {
                await wait(150);
                const flow = mockStore.flows.find((f) => f.id === flowId);
                const node = flow?.nodes?.find((n) => n.id === nodeId);
                if (!node) throw new Error("Node não encontrado");
                node.transitions = input.transitions;
                flow!.updatedAt = new Date();
            }
        );
    },
};