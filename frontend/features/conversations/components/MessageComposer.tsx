import { useRef, useState, type KeyboardEvent } from "react";
import { Send } from "lucide-react";

import { Button, Textarea } from "../../../shared/components/ui";

interface MessageComposerProps {
    disabled?: boolean;
    onSend: (text: string) => void;
    sending?: boolean;
}

export function MessageComposer({ disabled, onSend, sending }: MessageComposerProps) {
    const [text, setText] = useState("");
    const ref = useRef<HTMLTextAreaElement>(null);

    const send = () => {
        const value = text.trim();
        if (!value || disabled || sending) return;
        onSend(value);
        setText("");
        ref.current?.focus();
    };

    const handleKeyDown = (e: KeyboardEvent<HTMLTextAreaElement>) => {
        if (e.key === "Enter" && !e.shiftKey) {
            e.preventDefault();
            send();
        }
    };

    return (
        <div className="flex items-end gap-2 border-t border-neutral-200 bg-white p-3">
            <Textarea
                ref={ref}
                rows={1}
                value={text}
                onChange={(e) => setText(e.target.value)}
                onKeyDown={handleKeyDown}
                placeholder="Escreva uma mensagem… (Enter envia, Shift+Enter quebra linha)"
                className="min-h-10 max-h-32 resize-none"
                disabled={disabled}
            />
            <Button size="icon" onClick={send} disabled={disabled || sending || !text.trim()}>
                <Send className="h-4 w-4" />
            </Button>
        </div>
    );
}