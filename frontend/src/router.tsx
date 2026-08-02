import { createBrowserRouter } from "react-router-dom";

import { DashboardLayout } from "../shared/layouts/DashboardLayout";
import { DashboardPage } from "../features/dashboard/pages/DashboardPage";
import { ConversationFlowsPage } from "../features/conversationflows/pages/ConversationFlowsPage";
import { FlowEditorPage } from "../features/conversationflows/pages/FlowEditorPage";
import { ConversationsPage } from "../features/conversations/pages/ConversationsPage";

export const router = createBrowserRouter([
    {
        element: <DashboardLayout />,
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
            }
        ]
    }
]);