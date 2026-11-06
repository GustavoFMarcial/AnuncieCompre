// Máscaras de entrada aplicadas enquanto o usuário digita.

export function maskCnpj(value: string): string {
    const digits = value.replace(/\D/g, "").slice(0, 14);

    let masked = digits.slice(0, 2);
    if (digits.length > 2) masked += `.${digits.slice(2, 5)}`;
    if (digits.length > 5) masked += `.${digits.slice(5, 8)}`;
    if (digits.length > 8) masked += `/${digits.slice(8, 12)}`;
    if (digits.length > 12) masked += `-${digits.slice(12)}`;
    return masked;
}

export function maskCpf(value: string): string {
    const digits = value.replace(/\D/g, "").slice(0, 11);

    let masked = digits.slice(0, 3);
    if (digits.length > 3) masked += `.${digits.slice(3, 6)}`;
    if (digits.length > 6) masked += `.${digits.slice(6, 9)}`;
    if (digits.length > 9) masked += `-${digits.slice(9)}`;
    return masked;
}

// 10 dígitos = telefone fixo ((00) 0000-0000); 11 = celular ((00) 00000-0000).
export function maskPhone(value: string): string {
    const digits = value.replace(/\D/g, "").slice(0, 11);

    if (digits.length === 0) return "";
    if (digits.length <= 2) return `(${digits}`;
    if (digits.length <= 6) return `(${digits.slice(0, 2)}) ${digits.slice(2)}`;

    const splitAt = digits.length > 10 ? 7 : 6;
    return `(${digits.slice(0, 2)}) ${digits.slice(2, splitAt)}-${digits.slice(splitAt)}`;
}
