import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

import { conversationFlowService } from "../services/conversation-flow.service";
import type {
    CreateNodeInput,
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