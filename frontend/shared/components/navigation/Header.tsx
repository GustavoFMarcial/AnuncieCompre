import { useLocation } from "react-router-dom";
import { Bell } from "lucide-react";

const titles: Record<string, string> = {
    "/": "Dashboard",
    "/flows": "Fluxos de conversa",
    "/conversations": "Conversas",
    "/products": "Produtos",
    "/customers": "Clientes",
};

function resolveTitle(pathname: string): string {
    if (titles[pathname]) return titles[pathname];
    if (pathname.startsWith("/flows/")) return "Editor de fluxo";
    return "AnuncieCompre";
}

export function Header() {
    const { pathname } = useLocation();
    const title = resolveTitle(pathname);

    return (
        <header className="flex h-16 items-center justify-between border-b border-neutral-200 bg-white px-8">
            <h2 className="text-lg font-semibold text-neutral-900">{title}</h2>
            <div className="flex items-center gap-3">
                <button className="flex h-9 w-9 items-center justify-center rounded-full text-neutral-500 hover:bg-neutral-100">
                    <Bell className="h-5 w-5" />
                </button>
                <div className="flex h-9 w-9 items-center justify-center rounded-full bg-neutral-900 text-sm font-semibold text-white">
                    G
                </div>
            </div>
        </header>
    );
}