import { NavLink, type LinkProps } from "react-router-dom";
import type { ReactNode } from "react";

import { cn } from "../../../utils/cn";

interface SidebarItemProps extends Omit<LinkProps, "to"> {
    title: string;
    to: string;
    icon?: ReactNode;
}

export function SidebarItem({ title, to, icon, ...rest }: SidebarItemProps) {
    return (
        <NavLink
            to={to}
            {...rest}
            className={({ isActive }) =>
                cn(
                    "flex items-center gap-2.5 rounded-lg px-3 py-2 text-sm font-medium transition-colors",
                    isActive
                        ? "bg-neutral-900 text-white"
                        : "text-neutral-600 hover:bg-neutral-100"
                )
            }
        >
            {icon && <span className="flex h-4 w-4 items-center justify-center">{icon}</span>}
            <span>{title}</span>
        </NavLink>
    );
}