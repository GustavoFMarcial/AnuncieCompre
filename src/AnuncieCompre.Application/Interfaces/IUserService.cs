using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.Interfaces;

public interface IUserService
{
    public Task<Result> CreateUserAsync(Guid companyId, Email email, Phone phone, Name name, CPF cpf, string login, string password);
}