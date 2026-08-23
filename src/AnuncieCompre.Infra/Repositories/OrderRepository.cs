using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories;

public class OrderRepository(AnuncieCompreContext _context) : BaseRepository<Order>(_context), IOrderRepository
{
    public async Task<Order?> GetLastOrderByUserIdAsync(Guid userId)
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