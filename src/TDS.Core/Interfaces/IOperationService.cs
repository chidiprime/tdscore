using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS.Core.Common;
using TDS.Core.ViewModel;

namespace TDS.Core.Interfaces
{
    public interface IOperationService
    {
        Task<ResponseModel<string>> TransferAsync(TransferDTO transferDTO);
        Task<ResponseModel<List<string>>> GetNonProcessedLedgerAsync();
        Task<ResponseModel<List<TransactionResponseDTO>>> GetTransactionsAsync(PaginationParameters paginationParameters);
        Task<ResponseModel<List<LedgerResponseDTO>>> GetLedgersAsync(PaginationParameters paginationParameters);
    }
}
