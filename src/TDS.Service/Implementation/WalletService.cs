using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDS.Core.Common;
using TDS.Core.Entities;
using TDS.Core.Interfaces;
using TDS.Core.ViewModel;
using stellar_dotnet_sdk;
using static TDS.Core.Common.Constant;

namespace TDS.Service.Implementation
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog _logger;
        private readonly Settings _settings;
        private readonly IRestClient _restclient;
        public WalletService(IUnitOfWork unitOfWork, ILog logger, IOptions<Settings> settings, IRestClient restclient)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _settings = settings.Value;
            _restclient = restclient;
        
        }

        public async Task<ResponseModel<string>> ActivateWalletAsync(string account)
        {
            try
            {
            
                string uri = $"{_settings.FriendlyBotUrl}/?addr={account}";
                var request = new RestRequest(uri, Method.GET)
                { RequestFormat = DataFormat.Json };
                var response = await _restclient.ExecuteAsync<TransactionDetailDTO>(request);
                if (response.StatusCode.ToString() != StatusMessages.OK.ToUpper())
                {
                     return new ResponseModel<string>
                    { ResponseCode = ResponseCodes.BAD_REQUEST, ResponseMessage = StatusMessages.BAD_REQUEST, ResponseData = response.ErrorMessage };
                }
                //Get Account Detail
                var accountObj = await _unitOfWork.WalletRepository.SingleOrDefaultAync(x => x.Account == account);
                accountObj.IsActivated = true;
                var data = response.Data;
                var transaction = TransactionObj.Construct(data.Id.ToString(), data.Max_Fee, data.Source_Account,
                    account, TransactionType.TRANSFER);
                
                await _unitOfWork.TransactionRepository.AddAsync(transaction);
                _unitOfWork.WalletRepository.Update(accountObj);
                await _unitOfWork.CompleteAsync();
                return new ResponseModel<string>
                { ResponseCode = ResponseCodes.OK, ResponseMessage = StatusMessages.OK, ResponseData = response.StatusDescription };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new ResponseModel<string>
                { ResponseCode = ResponseCodes.SERVER_ERROR, ResponseMessage = StatusMessages.SERVER_ERROR, ResponseData = ResponseMessages.FAILED };
            }
        }

        public async Task<ResponseModel<List<WalletResponseDTO>>> GetActivatedWalletsAsync()
        {
            try
            {
                var query = await _unitOfWork.WalletRepository.FindAsync(x => x.IsActivated == true);
                //Check if there is no wallet activated
                if(query==null)
                    return new ResponseModel<List<WalletResponseDTO>>
                    { ResponseCode = ResponseCodes.NOT_FOUND, ResponseMessage = StatusMessages.NOT_FOUND, ResponseData = null };

                //Get activated wallets
                var wallets = await query.Select(a => new WalletResponseDTO { Account = a.Account, Secret=a.Secret,IsActivated=a.IsActivated}).ToListAsync();
                return new ResponseModel<List<WalletResponseDTO>>
                { ResponseCode = ResponseCodes.OK, ResponseMessage = StatusMessages.OK, ResponseData = wallets };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new ResponseModel<List<WalletResponseDTO>>
                { ResponseCode = ResponseCodes.SERVER_ERROR, ResponseMessage = StatusMessages.SERVER_ERROR, ResponseData = null };
            }
        }

        public async Task<ResponseModel<List<WalletResponseDTO>>> GetNonActivatedWalletsAsync()
        {
            try
            {
                var query = await _unitOfWork.WalletRepository.FindAsync(x => x.IsActivated == false);
                //Check if there is no wallet to be activated
                if (query == null)
                    return new ResponseModel<List<WalletResponseDTO>>
                    { ResponseCode = ResponseCodes.NOT_FOUND, ResponseMessage = StatusMessages.NOT_FOUND, ResponseData = null };

                //Get non activated wallets
                var wallets = await query.Select(a => new WalletResponseDTO { Account = a.Account, Secret=a.Secret,IsActivated=a.IsActivated }).ToListAsync();
                return new ResponseModel<List<WalletResponseDTO>>
                { ResponseCode = ResponseCodes.OK, ResponseMessage = StatusMessages.OK, ResponseData = wallets };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new ResponseModel<List<WalletResponseDTO>>
                { ResponseCode = ResponseCodes.SERVER_ERROR, ResponseMessage = StatusMessages.SERVER_ERROR, ResponseData = null };
            }
        }

        public async Task<ResponseModel<WalletResponseDTO>> GetWalletAsync(string account)
        {
            try
            {
                var query = await _unitOfWork.WalletRepository.SingleOrDefaultAync(x => x.Account == account);
                //Check if wallet exist on the databased
                if (query == null)
                    return new ResponseModel<WalletResponseDTO>
                    { ResponseCode = ResponseCodes.NOT_FOUND, ResponseMessage = StatusMessages.NOT_FOUND, ResponseData = null };

                //Get wallet
                var wallet = new WalletResponseDTO { Account = query.Account, Secret=query.Secret, IsActivated=query.IsActivated };
                return new ResponseModel<WalletResponseDTO>
                { ResponseCode = ResponseCodes.OK, ResponseMessage = StatusMessages.OK, ResponseData = wallet };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new ResponseModel<WalletResponseDTO>
                { ResponseCode = ResponseCodes.SERVER_ERROR, ResponseMessage = StatusMessages.SERVER_ERROR, ResponseData = null };
            }
        }

        public async Task<ResponseModel<string>> InsertAsync(List<WalletRequestDTO> walletRequestDTO)
        {
            try
            {
                // Ensuring that a stellar wallet is posted
                if (walletRequestDTO.Count == 0)
                    return new ResponseModel<string>
                    { ResponseCode = ResponseCodes.BAD_REQUEST, ResponseMessage = ResponseMessages.WALLET_REQUIRED, ResponseData = StatusMessages.FAILED };

                // Get list of wallet and persist on the database 
                
                var walletsToAdd = walletRequestDTO
                                  .Select(a => new Wallet()
                                  {
                                    Id = Guid.NewGuid(),
                                    Account = a.Account,
                                    Secret = a.Secret,
                                    IsActivated=false
                                  }).ToList();
                 _unitOfWork.WalletRepository.AddRange(walletsToAdd);
                 await _unitOfWork.CompleteAsync();
                _logger.Info(walletRequestDTO, ResponseMessages.WALLETS_SAVED);
                 return new ResponseModel<string>
                 { ResponseCode = ResponseCodes.CREATED, ResponseMessage = StatusMessages.OK, ResponseData = StatusMessages.OK };
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return new ResponseModel<string>
                { ResponseCode = ResponseCodes.SERVER_ERROR, ResponseMessage = StatusMessages.SERVER_ERROR, ResponseData = StatusMessages.FAILED };
            }
        }

        public async Task<ResponseModel<bool>> IsWalletActivatedAsync(string account)
        {
            try
            {
                var query = await GetWalletAsync(account);
                //Check if there is no wallet to be activated
                if (query == null)
                    return new ResponseModel<bool>
                    { ResponseCode = ResponseCodes.NOT_FOUND, ResponseMessage = StatusMessages.NOT_FOUND, ResponseData = false };

                return new ResponseModel<bool>
                { ResponseCode = ResponseCodes.OK, ResponseMessage = StatusMessages.OK, ResponseData = query.ResponseData.IsActivated};
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new ResponseModel<bool>
                { ResponseCode = ResponseCodes.SERVER_ERROR, ResponseMessage = StatusMessages.SERVER_ERROR, ResponseData = false };
            }
        }

      
    }
}
