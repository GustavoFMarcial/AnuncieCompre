using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.Infra.Repositories.CustomerRepo;

public class CustomerRepository(AnuncieCompreContext _context) : BaseRepository<Customer>(_context), ICustomerRepository
{
    
}