import { isAxiosError } from "axios";

// Normaliza os formatos de erro devolvidos pela API de companies:
// - POST: { success: false, errors: string[] } (use case e ModelState)
// - GET por id: string crua (ex.: "Company não encontrada")
// - ProblemDetails com errors: { campo: string[] } (defensivo)
export function extractCompanyErrors(err: unknown): string[] {
    if (isAxiosError(err)) {
        const data: unknown = err.response?.data;

        if (typeof data === "string" && data.trim().length > 0) {
            return [data];
        }

        if (Array.isArray(data)) {
            const msgs = data.filter((m): m is string => typeof m === "string");
            if (msgs.length > 0) return msgs;
        }

        if (data && typeof data === "object") {
            const errors = (data as { errors?: unknown }).errors;

            if (Array.isArray(errors)) {
                const msgs = errors.filter((m): m is string => typeof m === "string");
                if (msgs.length > 0) return msgs;
            }

            if (errors && typeof errors === "object") {
                const msgs = Object.values(errors as Record<string, unknown>)
                    .flat()
                    .filter((m): m is string => typeof m === "string");
                if (msgs.length > 0) return msgs;
            }
        }

        return [err.message];
    }

    if (err instanceof Error && err.message) {
        return [err.message];
    }

    return ["Erro inesperado. Tente novamente."];
}
