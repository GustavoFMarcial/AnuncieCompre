export interface Company {
    id: string;
    name: string;
    cnpj: string;
    email: string;
    phone: string;
}

export interface CompanyDTO {
    id: string;
    name: string;
    cnpj: string;
    email: string;
    phone: string;
}

// Saída do formulário de cadastro: o telefone vem como DDD + número
// e é convertido para o formato da API apenas no service.
export interface CreateCompanyInput {
    companyName: string;
    cnpj: string;
    email: string;
    phone: string;
    cpf: string;
    login: string;
    password: string;
}
