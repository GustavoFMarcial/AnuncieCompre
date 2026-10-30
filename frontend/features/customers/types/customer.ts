export interface Customer {
    id: string;
    phone: string;
    name: string | null;
    email: string | null;
}

export interface CustomerDTO {
    id: string;
    phone: string;
    name: string | null;
    email: string | null;
}

export interface GetCustomersResponse {
    customers: CustomerDTO[];
}

export interface CreateCustomerInput {
    name: string;
    email: string;
    phone: string;
}

export interface EditCustomerInput {
    name: string;
    email: string;
}
