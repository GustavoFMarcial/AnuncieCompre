using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.UseCase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories.OrderRepo;

public class OrderRepository(AnuncieCompreContext _context) : BaseRepository<Order>(_context), IOrderRepository
{
    public async Task<Order?> GetLastOrderByPhoneAsync(string phone)
    {
        return await context.Set<Order>()
            .Where(o => o.UserPhone.Value == phone)
            .OrderByDescending(o => o.Id)
            .FirstOrDefaultAsync();
    }
}