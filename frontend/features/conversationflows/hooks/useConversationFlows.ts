import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

import { conversationFlowService } from "../services/conversation-flow.service";
import type {
    CreateFlowInput,
    CreateNodeInput,
    UpdateFlowMetaInput,
    UpdateFlowStatusInput,
    UpdateNodeInput,
    UpdateTransitionsInput,
} from "../types/conversation-flow";

const FLOWS_KEY = ["conversation-flows"] as const;

export function useConversationFlows() {
    return useQuery({
        queryKey: FLOWS_KEY,
        queryFn: () => conversationFlowService.getAll(),
    });
}

export function useConversationFlow(flowId: string) {
    return useQuery({
        queryKey: ["conversation-flow", flowId] as const,
        queryFn: () => conversationFlowService.getById(flowId),
        enabled: !!flowId,
    });
}

export function useCreateFlow() {
    const qc = useQueryClient();
    return useMutation({
        mutationFn: (input: CreateFlowInput) => conversationFlowService.createFlow(input),
        onSuccess: () => {
            qc.invalidateQueries({ queryKey: FLOWS_KEY });
        },
    });
}

export function useUpdateFlow() {
    const qc = useQueryClient();
    return useMutation({
        mutationFn: ({ id, input }: { id: string; input: UpdateFlowMetaInput }) =>
            conversationFlowService.updateFlow(id, input),
        onSuccess: (_, vars) => {
            qc.invalidateQueries({ queryKey: FLOWS_KEY });
            qc.invalidateQueries({ queryKey: ["conversation-flow", vars.id] });
        },
    });
}

export function useUpdateFlowStatus() {
    const qc = useQueryClient();
    return useMutation({
        mutationFn: ({ id, input }: { id: string; input: UpdateFlowStatusInput }) =>
            conversationFlowService.updateFlowStatus(id, input),
        onSuccess: (_, vars) => {
            qc.invalidateQueries({ queryKey: FLOWS_KEY });
            qc.invalidateQueries({ queryKey: ["conversation-flow", vars.id] });
        },
    });
}

export function useDeleteFlow() {
    const qc = useQueryClient();
    return useMutation({
        mutationFn: (id: string) => conversationFlowService.deleteFlow(id),
        onSuccess: (_, id) => {
            qc.removeQueries({ queryKey: ["conversation-flow", id] });
            qc.invalidateQueries({ queryKey: FLOWS_KEY });
        },
    });
}

export function useCreateNode(flowId: string) {
    const qc = useQueryClient();
    return useMutation({
        mutationFn: (input: CreateNodeInput) => conversationFlowService.createNode(flowId, input),
        onSuccess: () => {
            qc.invalidateQueries({ queryKey: ["conversation-flow", flowId] });
            qc.invalidateQueries({ queryKey: FLOWS_KEY });
        },
    });
}

export function useUpdateNode(flowId: string) {
    const qc = useQueryClient();
    return useMutation({
        mutationFn: ({ nodeId, input }: { nodeId: string; input: UpdateNodeInput }) =>
            conversationFlowService.updateNode(flowId, nodeId, input),
        onSuccess: () => {
            qc.invalidateQueries({ queryKey: ["conversation-flow", flowId] });
            qc.invalidateQueries({ queryKey: FLOWS_KEY });
        },
    });
}

export function useDeleteNode(flowId: string) {
    const qc = useQueryClient();
    return useMutation({
        mutationFn: (nodeId: string) => conversationFlowService.deleteNode(flowId, nodeId),
        onSuccess: () => {
            qc.invalidateQueries({ queryKey: ["conversation-flow", flowId] });
            qc.invalidateQueries({ queryKey: FLOWS_KEY });
        },
    });
}

export function useUpdateTransitions(flowId: string) {
    const qc = useQueryClient();
    return useMutation({
        mutationFn: ({ nodeId, input }: { nodeId: string; input: UpdateTransitionsInput }) =>
            conversationFlowService.updateTransitions(flowId, nodeId, input),
        onSuccess: () => {
            qc.invalidateQueries({ queryKey: ["conversation-flow", flowId] });
        },
    });
}