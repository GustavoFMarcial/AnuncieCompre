using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.UseCase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories.OrderRepo;

public class OrderRepository(AnuncieCompreContext _context) : BaseRepository<Order>(_context), IOrderRepository
{
    public async Task<Order?> GetLastOrderByPhoneAsync(string userPhone)
    {
        return await context.Set<Order>()
            .Where(o => o.UserPhone.Value == userPhone)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task ExecuteDeleteAsync(string userPhone)
    {
        await context.Set<Order>()
            .Where(o => o.UserPhone.Value == userPhone)
            .OrderByDescending(o => o.CreatedAt)
            .ExecuteDeleteAsync();
    }

}