using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.UseCase.Interfaces;

public interface IOrderRepository : IBaseRepository<Order>
{
    public Task<Order?> GetLastOrderByUserId(Guid userId);
    public Task ExecuteDeleteByUserIdAsync(Guid userId);
}