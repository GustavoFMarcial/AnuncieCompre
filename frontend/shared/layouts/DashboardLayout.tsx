import { Outlet } from "react-router-dom";

import { Header } from "../components/navigation/Header";
import { Sidebar } from "../components/navigation/Sidebar/Sidebar";

export function DashboardLayout() {
    return (
        <div className="flex h-screen">

            <Sidebar />

            <div className="flex flex-1 flex-col">

                <Header />

                <main className="flex-1 overflow-auto bg-gray-50 p-8">
                    <Outlet />
                </main>

            </div>

        </div>
    );
}