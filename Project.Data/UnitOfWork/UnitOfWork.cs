using Microsoft.EntityFrameworkCore.Storage;
using Project.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FinalProjectDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(FinalProjectDbContext context)
        {
            _context = context;
        }
        public async Task BeginTransaction()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            await _transaction.CommitAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task RollBackTransaction()
        {
            await _transaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
