using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS.Core.Common;
using TDS.Core.ViewModel;

namespace TDS.Core.Interfaces
{
    public interface IWalletService
    {
       Task<ResponseModel<string>> InsertAsync(List<WalletRequestDTO> walletRequestDTO);
       Task<ResponseModel<List<WalletResponseDTO>>> GetNonActivatedWalletsAsync();
       Task<ResponseModel<List<WalletResponseDTO>>> GetActivatedWalletsAsync();
       Task<ResponseModel<WalletResponseDTO>> GetWalletAsync(string address);
       Task<ResponseModel<bool>> IsWalletActivatedAsync(string address);
       Task<ResponseModel<string>> ActivateWalletAsync(string address);
    }
}
