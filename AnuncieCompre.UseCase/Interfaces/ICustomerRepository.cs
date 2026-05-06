using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.UseCase.Interfaces;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    public Task<Customer?> GetCustomerByPhoneAsync(string userPhone);
}