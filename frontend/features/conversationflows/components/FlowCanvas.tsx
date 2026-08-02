import {
    Background,
    Controls,
    MiniMap,
    ReactFlow,
    type Edge,
    type Node,
    type NodeChange,
    type OnConnect,
    type OnEdgesDelete,
    applyNodeChanges,
} from "@xyflow/react";
import { useMemo, useState } from "react";
import "@xyflow/react/dist/style.css";

import { Button } from "../../../shared/components/ui";
import { Plus } from "lucide-react";
import {
    useConversationFlow,
    useCreateNode,
    useDeleteNode,
    useUpdateTransitions,
} from "../hooks/useConversationFlows";
import { FlowNodeCard } from "./FlowNodeCard";
import { buildGraphData, type FlowRFEdge, type FlowRFNode } from "../utils/dagre-layout";

interface FlowCanvasProps {
    flowId: string;
    selectedNodeId: string | null;
    onSelectNode: (id: string | null) => void;
}

const nodeTypes = { flowNode: FlowNodeCard };

type Position = { x: number; y: number };

export function FlowCanvas({ flowId, selectedNodeId, onSelectNode }: FlowCanvasProps) {
    const { data: flow } = useConversationFlow(flowId);
    const createNode = useCreateNode(flowId);
    const deleteNode = useDeleteNode(flowId);
    const updateTransitions = useUpdateTransitions(flowId);

    const { nodes: layoutNodes, edges: layoutEdges } = useMemo(
        () => buildGraphData(flow?.nodes ?? []),
        [flow?.nodes]
    );

    const [positions, setPositions] = useState<Record<string, Position>>({});

    const nodes = useMemo<FlowRFNode[]>(
        () =>
            layoutNodes.map((n) => ({
                ...n,
                position: positions[n.id] ?? n.position,
                selected: n.id === selectedNodeId,
            })),
        [layoutNodes, positions, selectedNodeId]
    );

    const onNodesChange = (changes: NodeChange<Node>[]) => {
        const next = applyNodeChanges(changes, nodes) as FlowRFNode[];
        const override: Record<string, Position> = {};
        for (const n of next) {
            if (positions[n.id]?.x !== n.position.x || positions[n.id]?.y !== n.position.y) {
                override[n.id] = n.position;
            }
        }
        if (Object.keys(override).length > 0) {
            setPositions((prev) => ({ ...prev, ...override }));
        }
    };

    const onConnect: OnConnect = (connection) => {
        const entry = flow?.nodes?.find((n) => n.id === connection.source);
        if (!entry) return;
        const used = new Set(entry.transitions.map((t) => t.option));
        let opt = "next";
        let i = 1;
        while (used.has(opt)) {
            opt = String(i);
            i++;
        }
        const transitions = [...entry.transitions, { option: opt, targetNodeId: connection.target }];
        updateTransitions.mutate({ nodeId: connection.source, input: { transitions } });
    };

    const onEdgesDelete: OnEdgesDelete = (edges: Edge[]) => {
        if (!flow?.nodes) return;
        for (const e of edges) {
            const entry = flow.nodes.find((n) => n.id === e.source);
            if (!entry) continue;
            const transitions = entry.transitions.filter((t) => t.targetNodeId !== e.target);
            updateTransitions.mutate({ nodeId: entry.id, input: { transitions } });
        }
    };

    const handleAddNode = () => {
        createNode.mutate({
            message: "Nova mensagem do bot",
            validationKind: null,
            valueObjectValidator: "None",
            options: [],
            isFinal: false,
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
                onNodesChange={onNodesChange}
                onConnect={onConnect}
                onEdgesDelete={onEdgesDelete}
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