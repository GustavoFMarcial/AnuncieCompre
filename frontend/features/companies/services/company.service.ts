import { api } from "../../../shared/lib/api";
import { toApiPhone } from "../../../shared/utils/phone";
import type { Company, CompanyDTO, CreateCompanyInput } from "../types/company";

function mapCompany(dto: CompanyDTO): Company {
    return {
        id: dto.id,
        name: dto.name,
        cnpj: dto.cnpj,
        email: dto.email,
        phone: dto.phone,
    };
}

export const companiesService = {
    // POST /api/companies cria a empresa e o primeiro usuário admin;
    // nome, e-mail e telefone do admin são derivados da empresa no backend.
    async create(input: CreateCompanyInput): Promise<Company> {
        const res = await api.post<CompanyDTO>("/api/companies", {
            companyName: input.companyName,
            cnpj: input.cnpj,
            email: input.email,
            phone: toApiPhone(input.phone),
            cpf: input.cpf,
            login: input.login,
            password: input.password,
        });
        return mapCompany(res.data);
    },
};
