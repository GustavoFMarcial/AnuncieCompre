import axios from "axios";

export const api = axios.create({
    baseURL: import.meta.env.VITE_API_URL ?? "http://localhost:5235",
    headers: { "Content-Type": "application/json" },
    timeout: 15000,
});

api.interceptors.response.use(
    (response) => response,
    (error) => {
        if (import.meta.env.DEV) {
            console.error("[api] request failed", error?.config?.url, error?.message);
        }
        return Promise.reject(error);
    }
);