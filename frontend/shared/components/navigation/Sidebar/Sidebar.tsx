import {
    LayoutDashboard,
    Workflow,
    MessagesSquare,
    Package,
    Users,
} from "lucide-react";

import { SidebarItem } from "./SidebarItem";

export function Sidebar() {
    return (
        <aside className="flex w-64 flex-col border-r border-neutral-200 bg-white">
            <div className="border-b border-neutral-100 p-6">
                <h1 className="text-xl font-bold text-neutral-900">AnuncieCompre</h1>
            </div>
            <nav className="flex flex-col gap-1 p-3">
                <SidebarItem title="Dashboard" to="/" icon={<LayoutDashboard className="h-4 w-4" />} />
                <SidebarItem title="Fluxos" to="/flows" icon={<Workflow className="h-4 w-4" />} />
                <SidebarItem title="Conversas" to="/conversations" icon={<MessagesSquare className="h-4 w-4" />} />
                <SidebarItem title="Produtos" to="/products" icon={<Package className="h-4 w-4" />} />
                <SidebarItem title="Clientes" to="/customers" icon={<Users className="h-4 w-4" />} />
            </nav>
        </aside>
    );
}