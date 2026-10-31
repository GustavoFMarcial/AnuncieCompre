export function formatPhoneForDisplay(phone: string): string {
    const raw = phone.replace(/^whatsapp:/i, "");
    const digits = raw.replace(/\D/g, "");

    if (!digits.startsWith("55") || digits.length < 12 || digits.length > 13) {
        return raw;
    }

    const ddd = digits.slice(2, 4);
    const number = digits.slice(4);

    if (number.length === 9) return `+55 ${ddd} ${number.slice(0, 5)}-${number.slice(5)}`;
    if (number.length === 8) return `+55 ${ddd} ${number.slice(0, 4)}-${number.slice(4)}`;
    return `+55 ${ddd} ${number}`;
}

// A API exige o telefone no formato Twilio ("whatsapp:+55XXXXXXXXXXX",
// 22 a 23 caracteres); os formulários coletam apenas DDD + número.
export function toApiPhone(raw: string): string {
    const digits = raw.replace(/\D/g, "");
    const national =
        digits.startsWith("55") && digits.length >= 12 && digits.length <= 13
            ? digits.slice(2)
            : digits;
    const phone = `whatsapp:+55${national}`;

    if (phone.length < 22 || phone.length > 23) {
        throw new Error("Telefone inválido. Informe o DDD e o número (ex.: 11 99999-9999).");
    }

    return phone;
}
