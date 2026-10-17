import dagre from "@dagrejs/dagre";
import type { Edge, Node } from "@xyflow/react";

import type { ConversationNode } from "../types/conversation-flow";

const NODE_WIDTH = 240;
const NODE_HEIGHT = 110;

export type FlowRFNode = Node<{
    nodeId: string;
    nodeNumber: number;
    message: string;
    validationKind: string | null;
    isFinal: boolean;
    isInitial: boolean;
}>;

export type FlowRFEdge = Edge;

export function buildGraphData(nodes: ConversationNode[]) {
    const g = new dagre.graphlib.Graph();
    g.setDefaultEdgeLabel(() => ({}));
    g.setGraph({ rankdir: "TB", nodesep: 60, ranksep: 80 });

    const rfnodes: FlowRFNode[] = nodes.map((n) => ({
        id: n.id,
        type: "flowNode",
        position: { x: 0, y: 0 },
        data: {
            nodeId: n.id,
            nodeNumber: n.number,
            message: n.message,
            validationKind: n.validationKind,
            isFinal: n.isFinal,
            isInitial: n.isInitial,
        },
    }));

    const rfedges: FlowRFEdge[] = [];
    const savedPositions = new Map<string, { x: number; y: number }>();
    nodes.forEach((n) => {
        if (n.positionX != null && n.positionY != null) {
            savedPositions.set(n.id, { x: n.positionX, y: n.positionY });
        }
    });
    nodes.forEach((n) => {
        n.transitions.forEach((t) => {
            if (nodes.some((x) => x.id === t.targetNodeId)) {
                rfedges.push({
                    id: `${n.id}:${t.option}:${t.targetNodeId}`,
                    source: n.id,
                    target: t.targetNodeId,
                    label: t.option,
                    labelStyle: { fontSize: 11, fontWeight: 600 },
                    labelBgStyle: { fill: "#f5f5f5" },
                    markerEnd: { type: "arrowclosed" as const },
                    selectable: false,
                    deletable: false,
                    focusable: false,
                });
            }
        });
    });

    rfnodes.forEach((n) => {
        g.setNode(n.id, { width: NODE_WIDTH, height: NODE_HEIGHT });
    });
    rfedges.forEach((e) => {
        g.setEdge(e.source, e.target);
    });

    dagre.layout(g);

    const laidOut: FlowRFNode[] = rfnodes.map((n) => {
        const saved = savedPositions.get(n.id);
        const pos = g.node(n.id);
        return {
            ...n,
            position: saved ?? {
                x: pos.x - NODE_WIDTH / 2,
                y: pos.y - NODE_HEIGHT / 2,
            },
        };
    });

    return { nodes: laidOut, edges: rfedges };
}