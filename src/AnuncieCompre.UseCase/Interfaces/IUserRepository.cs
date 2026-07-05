using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.UseCase.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    public Task<User?> GetUserByPhoneAsync(string userPhone);
}