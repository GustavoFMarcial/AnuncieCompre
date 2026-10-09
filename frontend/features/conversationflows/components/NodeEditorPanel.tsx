import { useState } from "react";
import { Trash2, X, Save, Flag } from "lucide-react";

import {
    Badge,
    Button,
    Dialog,
    DialogDescription,
    DialogTitle,
    Input,
    Label,
    Select,
    Separator,
    Textarea,
} from "../../../shared/components/ui";
import type {
    ConversationNode,
    NodeValidationErrors,
    NodeValidationKind,
    ValueObjectValidator,
} from "../types/conversation-flow";
import {
    useDeleteNode,
    useUpdateNode,
    useUpdateTransitions,
} from "../hooks/useConversationFlows";
import { OptionsEditor } from "./OptionsEditor";
import { ValidationKindSelect } from "./ValidationKindSelect";
import {
    finalKindsRequiringValueValidator,
    kindsRequiringOptions,
    valueObjectValidatorOptions,
} from "../utils/validation-options";
import { AxiosError } from "axios";
import { ErrorsDialog } from "./ErrorsDialog";

interface NodeEditorPanelProps {
    flowId: string;
    node: ConversationNode | null;
    nodes: ConversationNode[];
    onClose: () => void;
}

function transitionTargetLabel(n: ConversationNode): string {
    return `${n.number} - ${n.message.slice(0, 24) || n.id}`;
}

export function NodeEditorPanel({ flowId, node, nodes, onClose }: NodeEditorPanelProps) {
    const updateNode = useUpdateNode(flowId);
    const deleteNode = useDeleteNode(flowId);
    const updateTransitions = useUpdateTransitions(flowId);

    const [message, setMessage] = useState(node?.message ?? "");
    const [validationKind, setValidationKind] = useState<NodeValidationKind | null>(
        node?.validationKind ?? "none"
    );
    const [valueObjectValidator, setValueObjectValidator] = useState<ValueObjectValidator>(
        node?.valueObjectValidator ?? "none"
    );
    const [options, setOptions] = useState<string[]>(node?.options ?? []);
    const [isFinal, setIsFinal] = useState(node?.isFinal ?? false);
    const [isInitial, setIsInitial] = useState(false);
    const [confirmDelete, setConfirmDelete] = useState(false);
    const [newTransitionOption, setNewTransitionOption] = useState("");
    const [newTransitionTarget, setNewTransitionTarget] = useState("");

    const [errorsOpen, setErrorsOpen] = useState(false);
    const [errors, setErrors] = useState<string[]>([]);

    if (!node) {
        return (
            <div className="flex h-full w-80 items-center justify-center border-l border-neutral-200 bg-white p-6 text-sm text-neutral-400">
                Selecione um node para editar
            </div>
        );
    }

    const showValueValidator =
        validationKind && finalKindsRequiringValueValidator.includes(validationKind);
    const showOptions =
        validationKind && kindsRequiringOptions.includes(validationKind);

    const handleSave = () => {
        updateNode.mutate({
            nodeId: node.id,
            input: {
                message,
                validationKind: isFinal ? "final" : validationKind,
                valueObjectValidator: showValueValidator ? valueObjectValidator : "none",
                options: showOptions ? options.filter((o) => o.trim() !== "") : [],
                isFinal,
                isInitial,
            },
        },
            {
                onError: (err) => {
                    const axiosErr = err as AxiosError<NodeValidationErrors>;
                    const msgs = axiosErr.response?.data?.errors;
                    if (msgs && msgs.length > 0) {
                        setErrors(msgs);
                        setErrorsOpen(true);
                    }
                }
            });
    };

    const handleDelete = () => {
        deleteNode.mutate(node.id, { onSuccess: () => onClose() });
    };

    const setTransitionOption = (idx: number, option: string) => {
        const transitions = node.transitions.map((t, i) =>
            i === idx ? { ...t, option } : t
        );
        updateTransitions.mutate({ nodeId: node.id, input: { transitions } });
    };

    const setTransitionTarget = (idx: number, targetNodeId: string) => {
        const transitions = node.transitions.map((t, i) =>
            i === idx ? { ...t, targetNodeId } : t
        );
        updateTransitions.mutate({ nodeId: node.id, input: { transitions } });
    };

    const removeTransition = (idx: number) => {
        const transitions = node.transitions.filter((_, i) => i !== idx);
        updateTransitions.mutate({ nodeId: node.id, input: { transitions } });
    };

    const addTransition = () => {
        const option = newTransitionOption.trim();
        if (!option || !newTransitionTarget) return;
        const transitions = [...node.transitions, { option, targetNodeId: newTransitionTarget }];
        updateTransitions.mutate({ nodeId: node.id, input: { transitions } });
        setNewTransitionOption("");
    };

    const otherNodes = nodes
        .filter((n) => n.id !== node.id)
        .sort((a, b) => a.number - b.number);

    return (
        <div className="flex h-full w-80 flex-col border-l border-neutral-200 bg-white">
            <div className="flex items-center justify-between border-b border-neutral-100 p-4">
                <div className="flex items-center gap-2">
                    <h3 className="font-semibold text-neutral-900">Editar node</h3>
                    {isInitial && <Badge variant="success">Inicial</Badge>}
                    {isFinal && <Badge variant="success">Final</Badge>}
                </div>
                <Button size="icon" variant="ghost" onClick={onClose}>
                    <X className="h-4 w-4" />
                </Button>
            </div>

            <div className="flex-1 space-y-4 overflow-y-auto p-4">
                <div className="space-y-1.5">
                    <Label htmlFor="node-message">Mensagem</Label>
                    <Textarea
                        id="node-message"
                        rows={4}
                        value={message}
                        onChange={(e) => setMessage(e.target.value)}
                        placeholder="Texto que o bot envia ao chegar neste node"
                    />
                </div>

                <Separator />

                <div className="space-y-1.5">
                    <Label>Tipo de validação</Label>
                    <ValidationKindSelect
                        value={isFinal ? "final" : validationKind}
                        onChange={(kind) => {
                            if (kind === "final") {
                                setIsFinal(true);
                                setValidationKind("final");
                            } else {
                                setIsFinal(false);
                                setValidationKind(kind);
                                if (kind === null) setOptions([]);
                            }
                        }}
                    />
                </div>

                {showValueValidator && (
                    <div className="space-y-1.5">
                        <Label>Validar entrada como</Label>
                        <Select
                            value={valueObjectValidator}
                            onChange={(e) =>
                                setValueObjectValidator(e.target.value as ValueObjectValidator)
                            }
                        >
                            {valueObjectValidatorOptions().map((opt) => (
                                <option key={opt.value} value={opt.value}>
                                    {opt.label}
                                </option>
                            ))}
                        </Select>
                    </div>
                )}

                {showOptions && (
                    <div className="space-y-1.5">
                        <Label>Opções</Label>
                        <OptionsEditor options={options} onChange={setOptions} />
                    </div>
                )}

                <Separator />

                <div className="space-y-2">
                    <Label>Transições</Label>
                    {node.transitions.length === 0 && (
                        <p className="text-xs text-neutral-400">
                            Nenhuma transição. Adicione abaixo selecionando a opção e o node alvo.
                        </p>
                    )}
                    <div className="space-y-2">
                        {node.transitions.map((t, idx) => (
                            <div key={idx} className="flex items-center gap-1.5">
                                <Input
                                    className="h-8 w-16 text-xs"
                                    value={t.option}
                                    placeholder="opt"
                                    onChange={(e) => setTransitionOption(idx, e.target.value)}
                                />
                                <Select
                                    className="h-8 flex-1 text-xs"
                                    value={t.targetNodeId}
                                    onChange={(e) => setTransitionTarget(idx, e.target.value)}
                                >
                                    {otherNodes.map((n) => (
                                        <option key={n.id} value={n.id}>
                                            {transitionTargetLabel(n)}
                                        </option>
                                    ))}
                                </Select>
                                <Button size="icon" variant="ghost" onClick={() => removeTransition(idx)}>
                                    <X className="h-3.5 w-3.5" />
                                </Button>
                            </div>
                        ))}
                    </div>
                    <div className="space-y-1.5 rounded-md border border-neutral-200 p-2">
                        <div className="flex items-center gap-1.5">
                            <Input
                                className="h-8 w-16 text-xs"
                                value={newTransitionOption}
                                placeholder="1"
                                onChange={(e) => setNewTransitionOption(e.target.value)}
                            />
                            <Select
                                className="h-8 flex-1 text-xs"
                                value={newTransitionTarget}
                                onChange={(e) => setNewTransitionTarget(e.target.value)}
                            >
                                <option value="" disabled>
                                    Node alvo
                                </option>
                                {otherNodes.map((n) => (
                                    <option key={n.id} value={n.id}>
                                        {transitionTargetLabel(n)}
                                    </option>
                                ))}
                            </Select>
                        </div>
                        <Button
                            size="sm"
                            variant="outline"
                            className="w-full"
                            onClick={addTransition}
                            disabled={
                                otherNodes.length === 0 ||
                                !newTransitionOption.trim() ||
                                !newTransitionTarget ||
                                updateTransitions.isPending
                            }
                        >
                            Adicionar
                        </Button>
                        <p className="text-xs text-neutral-400">
                            A opção é o que o cliente deve enviar para ir ao node alvo.
                        </p>
                    </div>
                </div>
            </div>

            <div className="space-y-2 border-t border-neutral-100 p-4">
                <Button className="w-full" onClick={handleSave} disabled={updateNode.isPending}>
                    <Save className="h-4 w-4" />
                    Salvar node
                </Button>
                <div className="flex gap-2">
                    <Button
                        variant="outline"
                        className="flex-1"
                        onClick={() => {
                            setIsInitial((v) => !v);
                            if (isFinal){
                                setIsFinal(false);
                            }
                        }}
                    >
                        <Flag className="h-4 w-4" />
                        {isInitial ? "Inicial✅" : "inicial"}
                    </Button>
                    <Button
                        variant="outline"
                        className="flex-1"
                        onClick={() => {
                            setIsFinal((v) => !v);
                            setValidationKind((k) => (!isFinal ? "final" : k));
                            if (isInitial){
                                setIsInitial(false);
                                setValidationKind("none");
                            }
                        }}
                    >
                        <Flag className="h-4 w-4" />
                        {isFinal ? "Final✅" : "Final"}
                    </Button>
                    <Button
                        variant="destructive"
                        size="icon"
                        onClick={() => setConfirmDelete(true)}
                    >
                        <Trash2 className="h-4 w-4" />
                    </Button>
                </div>
                <ErrorsDialog
                    open={errorsOpen}
                    onOpenChange={setErrorsOpen}
                    errors={errors}
                />
            </div>

            <Dialog open={confirmDelete} onClose={() => setConfirmDelete(false)}>
                <DialogTitle>Excluir node?</DialogTitle>
                <DialogDescription>
                    Esta ação não pode ser desfeita. As transições que apontam para este node
                    também serão removidas.
                </DialogDescription>
                <div className="mt-5 flex justify-end gap-2">
                    <Button variant="outline" onClick={() => setConfirmDelete(false)}>
                        Cancelar
                    </Button>
                    <Button variant="destructive" onClick={handleDelete} disabled={deleteNode.isPending}>
                        Excluir
                    </Button>
                </div>
            </Dialog>
        </div>
    );
}