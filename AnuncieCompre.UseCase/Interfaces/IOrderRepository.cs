using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.UseCase.Interfaces;

public interface IOrderRepository : IBaseRepository<Order>
{
    public Task<Order?> GetLastOrderByPhoneAsync(string phone);
}