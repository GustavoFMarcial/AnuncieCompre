using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Application.Interfaces;

public interface IOrderRepository : IBaseRepository<Order>
{
    public Task<Order?> GetLastOrderByUserIdAsync(Guid userId);
    public Task ExecuteDeleteByUserIdAsync(Guid userId);
}