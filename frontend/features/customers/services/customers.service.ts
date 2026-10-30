import { api } from "../../../shared/lib/api";
import type {
    CreateCustomerInput,
    Customer,
    CustomerDTO,
    EditCustomerInput,
    GetCustomersResponse,
} from "../types/customer";

function mapCustomer(dto: CustomerDTO): Customer {
    return {
        id: dto.id,
        phone: dto.phone,
        name: dto.name ?? null,
        email: dto.email ?? null,
    };
}

// A API exige o telefone no formato Twilio ("whatsapp:+55XXXXXXXXXXX",
// 22 a 23 caracteres); o formulário coleta apenas DDD + número.
function toApiPhone(raw: string): string {
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

export const customersService = {
    async list(): Promise<Customer[]> {
        const res = await api.get<GetCustomersResponse>("/api/customers");
        return res.data.customers.map(mapCustomer);
    },

    async getById(id: string): Promise<Customer> {
        const res = await api.get<CustomerDTO>(`/api/customers/${id}`);
        return mapCustomer(res.data);
    },

    async create(input: CreateCustomerInput): Promise<Customer> {
        const res = await api.post<CustomerDTO>("/api/customers", {
            name: input.name,
            email: input.email,
            phone: toApiPhone(input.phone),
        });
        return mapCustomer(res.data);
    },

    async edit(id: string, input: EditCustomerInput): Promise<void> {
        await api.patch(`/api/customers/${id}`, input);
    },
};
