import { api } from "../../../shared/lib/api";
import type {
    ConversationFlow,
    CreateFlowInput,
    CreateNodeInput,
    UpdateFlowMetaInput,
    UpdateFlowStatusInput,
    UpdateNodeInput,
    UpdateTransitionsInput,
} from "../types/conversation-flow";

export const conversationFlowService = {
    async getAll(): Promise<ConversationFlow[]> {
        return (await api.get<ConversationFlow[]>("/api/flows")).data;
    },

    async getById(id: string): Promise<ConversationFlow> {
        return (await api.get<ConversationFlow>(`/api/flows/${id}`)).data;
    },

    async createFlow(input: CreateFlowInput): Promise<ConversationFlow> {
        return (await api.post<ConversationFlow>("/api/flows", input)).data;
    },

    async updateFlow(id: string, input: UpdateFlowMetaInput): Promise<void> {
        await api.put(`/api/flows/${id}`, input);
    },

    async updateFlowStatus(id: string, input: UpdateFlowStatusInput): Promise<void> {
        await api.patch(`/api/flows/${id}/status`, input);
    },

    async deleteFlow(id: string): Promise<void> {
        await api.delete(`/api/flows/${id}`);
    },

    async createNode(flowId: string, input: CreateNodeInput): Promise<void> {
        await api.post(`/api/flows/${flowId}/nodes`, input);
    },

    async updateNode(flowId: string, nodeId: string, input: UpdateNodeInput): Promise<void> {
        await api.put(`/api/flows/${flowId}/nodes/${nodeId}`, input);
    },

    async deleteNode(flowId: string, nodeId: string): Promise<void> {
        await api.delete(`/api/flows/${flowId}/nodes/${nodeId}`);
    },

    async updateTransitions(flowId: string, nodeId: string, input: UpdateTransitionsInput): Promise<void> {
        await api.patch(`/api/flows/${flowId}/nodes/${nodeId}/transitions`,input);
    },
};