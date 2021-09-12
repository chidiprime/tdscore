using Clarit.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS.Core.Entities;

namespace TDS.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Wallet,Guid> WalletRepository { get; }
        IBaseRepository<ApiLog, Guid> ApiLogRepository { get; }
        IBaseRepository<WalletTransaction, Guid> TransactionRepository { get; }
        IBaseRepository<TransactionLedger, Guid> TransactionLedgersRepository { get; }


        int Complete();
        Task<int> CompleteAsync();
    }
}
