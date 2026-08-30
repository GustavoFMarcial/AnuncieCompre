using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories;

public class CustomerRepository(AnuncieCompreContext _context) : BaseRepository<Customer>(_context), ICustomerRepository
{
    public async Task<Customer?> GetCustomerByIdAsync(Guid userId)
    {
        var user = await context.Set<Customer>().FirstOrDefaultAsync(u => u.Id == userId);

        return user;
    }

    public async Task ExecuteDeleteByCustomerIdAsync(Guid userId)
    {
        await context.Set<Customer>().Where(u => u.Id == userId).ExecuteDeleteAsync();
    }

    public async Task<Customer?> GetCustomerByPhoneAsync(string userPhone)
    {
        var user = await context.Set<Customer>().FirstOrDefaultAsync(u => u.Phone.Value == userPhone);

        return user;
    }
}