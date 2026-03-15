using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.UseCase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories.UserRepo;

public class UserRepository(AnuncieCompreContext _context) : BaseRepository<User>(_context), IUserRepository
{
    public async Task<User?> GetUserByPhoneAsync(string userPhone)
    {
        var user = await context.Set<User>().FirstOrDefaultAsync(u => u.Phone.Number == userPhone);

        return user;
    }
}