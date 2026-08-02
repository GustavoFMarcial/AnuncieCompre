import { useState } from "react";
import { MessagesSquare } from "lucide-react";

import { StatusFilter } from "../components/StatusFilter";
import { ConversationList } from "../components/ConversationList";
import { ConversationThread } from "../components/ConversationThread";
import { FlowPageHeader } from "../../conversationflows/components/FlowPageHeader";
import { useConversations } from "../hooks/useConversations";
import type { ConversationStatusFilter } from "../types/conversation";

export function ConversationsPage() {
    const [status, setStatus] = useState<ConversationStatusFilter>("all");
    const [selectedId, setSelectedId] = useState<string | null>(null);
    const { data, isLoading, isError } = useConversations(status);

    const selected = data?.find((c) => c.id === selectedId) ?? null;
    const sorted = [...(data ?? [])].sort(
        (a, b) => b.lastMessageAt.getTime() - a.lastMessageAt.getTime()
    );

    return (
        <div className="flex flex-col gap-4">
            <FlowPageHeader
                title="Conversas"
                description="Veja e continue os atendimentos iniciados pelo bot."
            />

            <div className="flex h-[calc(100vh-12rem)] overflow-hidden rounded-lg border border-neutral-200 bg-white">
                <div className="flex w-80 shrink-0 flex-col border-r border-neutral-200">
                    <div className="flex items-center justify-between gap-2 border-b border-neutral-100 p-3">
                        <StatusFilter value={status} onChange={(v) => setStatus(v)} />
                    </div>

                    {isLoading && (
                        <div className="flex flex-1 items-center justify-center text-sm text-neutral-400">
                            Carregando…
                        </div>
                    )}
                    {isError && (
                        <div className="flex flex-1 items-center justify-center text-sm text-red-600">
                            Erro ao carregar.
                        </div>
                    )}
                    {data && (
                        <ConversationList
                            conversations={sorted}
                            selectedId={selectedId}
                            status={status}
                            onSelect={setSelectedId}
                        />
                    )}
                </div>

                <div className="flex flex-1 flex-col">
                    {!selected && (
                        <div className="flex h-full flex-col items-center justify-center gap-2 text-center text-sm text-neutral-400">
                            <MessagesSquare className="h-10 w-10" />
                            Selecione uma conversa para visualizar e responder.
                        </div>
                    )}
                    {selected && <ConversationThread conversation={selected} />}
                </div>
            </div>
        </div>
    );
}