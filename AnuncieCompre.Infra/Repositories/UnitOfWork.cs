using AnuncieCompre.Infra.Data;
using AnuncieCompre.UseCase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories;

public class UnitOfWork(AnuncieCompreContext _context) : IUnitOfWork
{
    private readonly DbContext context = _context;
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}