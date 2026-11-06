using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Application.Interfaces;

public interface IIdentityUserService
{
    public Task<Result> CreateUserAsync(Guid companyId, Email email, Phone phone, Name name, CPF cpf, string login, string password, bool isAdmin = false);
    public Task<Result> AddToRoleAsync(Guid userId, string role);
}