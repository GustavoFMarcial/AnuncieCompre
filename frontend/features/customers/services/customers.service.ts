import { api } from "../../../shared/lib/api";
import { toApiPhone } from "../../../shared/utils/phone";
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
