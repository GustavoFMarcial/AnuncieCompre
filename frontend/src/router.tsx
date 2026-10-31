import { createBrowserRouter } from "react-router-dom";

import { DashboardLayout } from "../shared/layouts/DashboardLayout";
import { DashboardPage } from "../features/dashboard/pages/DashboardPage";
import { ConversationFlowsPage } from "../features/conversationflows/pages/ConversationFlowsPage";
import { FlowEditorPage } from "../features/conversationflows/pages/FlowEditorPage";
import { ConversationsPage } from "../features/conversations/pages/ConversationsPage";
import { CustomersPage } from "../features/customers/pages/CustomersPage";
import { RegisterCompanyPage } from "../features/companies/pages/RegisterCompanyPage";
import { ConversationsRealtimeProvider } from "../features/conversations/components/ConversationsRealtimeProvider";
import { ConversationsSidebarBadge } from "../features/conversations/components/ConversationsSidebarBadge";

export const router = createBrowserRouter([
    {
        path: "/register",
        element: <RegisterCompanyPage />
    },
    {
        element: (
            <ConversationsRealtimeProvider>
                <DashboardLayout
                    sidebarItemTrailing={{ "/conversations": <ConversationsSidebarBadge /> }}
                />
            </ConversationsRealtimeProvider>
        ),
        children: [
            {
                path: "/",
                element: <DashboardPage />
            },
            {
                path: "/flows",
                element: <ConversationFlowsPage />
            },
            {
                path: "/flows/:flowId",
                element: <FlowEditorPage />
            },
            {
                path: "/conversations",
                element: <ConversationsPage />
            },
            {
                path: "/customers",
                element: <CustomersPage />
            }
        ]
    }
]);