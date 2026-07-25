using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.UseCase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories.OrderRepo;

public class OrderRepository(AnuncieCompreContext _context) : BaseRepository<Order>(_context), IOrderRepository
{
    public async Task<Order?> GetLastOrderByUserId(Guid userId)
    {
        return await context.Set<Order>()
            .Where(o => o.User.Id == userId)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task ExecuteDeleteByUserIdAsync(Guid userId)
    {
        await context.Set<Order>()
            .Where(o => o.User.Id == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ExecuteDeleteAsync();
    }

}