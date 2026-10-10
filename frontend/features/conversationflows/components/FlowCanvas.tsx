import {
    Background,
    Controls,
    MiniMap,
    ReactFlow,
    useNodesState,
} from "@xyflow/react";
import { useEffect, useMemo, useRef } from "react";
import "@xyflow/react/dist/style.css";

import { Button } from "../../../shared/components/ui";
import { Plus } from "lucide-react";
import {
    useConversationFlow,
    useCreateNode,
    useDeleteNode,
} from "../hooks/useConversationFlows";
import { FlowNodeCard } from "./FlowNodeCard";
import { buildGraphData, type FlowRFEdge, type FlowRFNode } from "../utils/dagre-layout";

interface FlowCanvasProps {
    flowId: string;
    selectedNodeId: string | null;
    onSelectNode: (id: string | null) => void;
}

const nodeTypes = { flowNode: FlowNodeCard };

export function FlowCanvas({ flowId, selectedNodeId, onSelectNode }: FlowCanvasProps) {
    const { data: flow } = useConversationFlow(flowId);
    const createNode = useCreateNode(flowId);
    const deleteNode = useDeleteNode(flowId);

    const { nodes: layoutNodes, edges: layoutEdges } = useMemo(
        () => buildGraphData(flow?.nodes ?? []),
        [flow?.nodes]
    );

    const [nodes, setNodes, onNodesChange] = useNodesState<FlowRFNode>(layoutNodes);

    const lastSyncedNodesRef = useRef(flow?.nodes);
    useEffect(() => {
        if (lastSyncedNodesRef.current === flow?.nodes) return;
        lastSyncedNodesRef.current = flow?.nodes;
        setNodes((prev) => {
            const prevById = new Map(prev.map((p) => [p.id, p]));
            return layoutNodes.map((n) => {
                const existing = prevById.get(n.id);
                return existing ? { ...existing, ...n, position: existing.position } : n;
            });
        });
    }, [flow?.nodes, layoutNodes, setNodes]);

    const handleAddNode = () => {
        createNode.mutate({
            message: "Nova mensagem do bot",
            validationKind: null,
            valueObjectValidator: "none",
            options: [],
            isFinal: false,
            isInitial: false,
        });
    };

    const handleNodesDelete = () => {
        if (selectedNodeId) {
            deleteNode.mutate(selectedNodeId);
            onSelectNode(null);
        }
    };

    return (
        <div className="relative flex-1">
            <div className="absolute left-4 top-4 z-10">
                <Button size="sm" variant="default" onClick={handleAddNode} disabled={createNode.isPending}>
                    <Plus className="h-4 w-4" />
                    Adicionar node
                </Button>
            </div>

            <ReactFlow
                nodes={nodes}
                edges={layoutEdges as FlowRFEdge[]}
                nodeTypes={nodeTypes}
                nodesConnectable={false}
                onNodesChange={onNodesChange}
                onNodesDelete={handleNodesDelete}
                onNodeClick={(_, node) => onSelectNode(node.id)}
                onPaneClick={() => onSelectNode(null)}
                deleteKeyCode={["Delete", "Backspace"]}
                fitView
                className="bg-neutral-50"
            >
                <Background gap={16} size={1} />
                <Controls />
                <MiniMap pannable zoomable className="!bg-white" />
            </ReactFlow>
        </div>
    );
}