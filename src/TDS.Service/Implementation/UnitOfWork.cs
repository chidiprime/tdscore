using Clarit.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS.Core.Entities;
using TDS.Core.Interfaces;
using TDS.Data;

namespace TDS.Service.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly TDSContext _context;
        public UnitOfWork(TDSContext context)
        {
            _context = context;
        }
        public IBaseRepository<Wallet, Guid> WalletRepository => new BaseRepository<Wallet, Guid>(_context);
        public IBaseRepository<ApiLog, Guid> ApiLogRepository => new BaseRepository<ApiLog, Guid>(_context);
        public IBaseRepository<WalletTransaction, Guid> TransactionRepository => new BaseRepository<WalletTransaction, Guid>(_context);
        public IBaseRepository<TransactionLedger, Guid> TransactionLedgersRepository => new BaseRepository<TransactionLedger, Guid>(_context);
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
