using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Application.Interfaces;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    public Task<Customer?> GetCustomerByPhoneAsync(string userPhone);
    public Task<Customer?> GetCustomerByIdAsync(Guid userId);
    public Task ExecuteDeleteByCustomerIdAsync(Guid userId);
}