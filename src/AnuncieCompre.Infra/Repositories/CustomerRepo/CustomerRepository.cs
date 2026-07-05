using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.UseCase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories.CustomerRepo;

public class CustomerRepository(AnuncieCompreContext _context) : BaseRepository<Customer>(_context), ICustomerRepository
{
    public async Task<Customer?> GetCustomerByPhoneAsync(string userPhone)
    {
        return await context.Set<Customer>().FirstOrDefaultAsync(c => c.User.Phone.Value == userPhone);
    }
}