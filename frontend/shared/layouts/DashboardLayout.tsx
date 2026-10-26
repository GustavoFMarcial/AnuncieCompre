import { Outlet } from "react-router-dom";
import type { ReactNode } from "react";

import { Header } from "../components/navigation/Header";
import { Sidebar } from "../components/navigation/Sidebar/Sidebar";

interface DashboardLayoutProps {
    sidebarItemTrailing?: Record<string, ReactNode>;
}

export function DashboardLayout({ sidebarItemTrailing }: DashboardLayoutProps) {
    return (
        <div className="flex h-screen">

            <Sidebar itemTrailing={sidebarItemTrailing} />

            <div className="flex flex-1 flex-col">

                <Header />

                <main className="flex-1 overflow-auto bg-gray-50 p-8">
                    <Outlet />
                </main>

            </div>

        </div>
    );
}