using AnuncieCompre.Infra.Data;
using AnuncieCompre.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AnuncieCompre.Infra.Repositories;

public class UnitOfWork(AnuncieCompreContext _context) : IUnitOfWork
{
    private readonly DbContext context = _context;
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<IApplicationTransaction> BeginTransactionAsync()
    {
        IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
        return new ApplicationTransaction(transaction); 
    }
}