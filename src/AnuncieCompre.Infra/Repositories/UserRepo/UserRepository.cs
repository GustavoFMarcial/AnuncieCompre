using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.UseCase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories.UserRepo;

public class UserRepository(AnuncieCompreContext _context) : BaseRepository<User>(_context), IUserRepository
{
    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        var user = await context.Set<User>().FirstOrDefaultAsync(u => u.Id == userId);

        return user;
    }

    public async Task ExecuteDeleteByUserIdAsync(Guid userId)
    {
        await context.Set<User>().Where(u => u.Id == userId).ExecuteDeleteAsync();
    }

    public async Task<User?> GetUserByPhoneAsync(string userPhone)
    {
        var user = await context.Set<User>().FirstOrDefaultAsync(u => u.Phone.Value == userPhone);

        return user;
    }
}