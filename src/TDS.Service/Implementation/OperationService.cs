using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RestSharp;
using stellar_dotnet_sdk;
using stellar_dotnet_sdk.responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDS.Core.Common;
using TDS.Core.Entities;
using TDS.Core.Interfaces;
using TDS.Core.ViewModel;
using X.PagedList;
using static TDS.Core.Common.Constant;

namespace TDS.Service.Implementation
{
    public class OperationService : IOperationService
    {
        private readonly ILog _logger;
        private readonly Settings _settings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OperationService(ILog logger, IOptions<Settings> settings,  
            IUnitOfWork unitOfWork, IRestClient restclient, IMapper mapper)
        {
            _logger = logger;
            _settings = settings.Value;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task<ResponseModel<string>> TransferAsync(TransferDTO transferDTO)
        {
            try
            {
                var accountObj =await _unitOfWork.WalletRepository.SingleOrDefaultAync(x=>x.Account==transferDTO.SourceAccount);
                //Set network and server
                Network network = new Network(_settings.Network);
                Server server = new Server(_settings.Server);

                //Source keypair from the secret seed
                KeyPair sourceKeypair = KeyPair.FromSecretSeed(accountObj.Secret);

                //Destination keypair from the account id
                KeyPair destinationKeyPair = KeyPair.FromAccountId(transferDTO.DestinationAccount);

                //Load source account data
                AccountResponse sourceAccountResponse = await server.Accounts.Account(sourceKeypair.AccountId);

                //Create source account object
                Account sourceAccount = new Account(sourceKeypair.AccountId, sourceAccountResponse.SequenceNumber);

                //Create asset object with specific amount
                Asset asset = new AssetTypeNative();

                string amount = transferDTO.Amount;

                //Create payment operation
                PaymentOperation operation = new PaymentOperation.Builder(destinationKeyPair, asset, amount).SetSourceAccount(sourceAccount.KeyPair).Build();

                //Create transaction and add the payment operation we created
                Transaction transaction = new Transaction.Builder(sourceAccount).AddOperation(operation).Build();
                Network.UseTestNetwork();
                //Sign Transaction
                transaction.Sign(sourceKeypair);
                var response = await server.SubmitTransaction(transaction);
                if(response.IsSuccess()== false)
                    return new ResponseModel<string>
                    { ResponseCode = ResponseCodes.SERVER_ERROR, ResponseMessage = StatusMessages.SERVER_ERROR, ResponseData = StatusMessages.TRANSFER_FAILED };

                var transactionObj = TransactionObj.Construct(response.Hash, transferDTO.Amount, transferDTO.SourceAccount,
                    transferDTO.DestinationAccount, TransactionType.TRANSFER);

                await _unitOfWork.TransactionRepository.AddAsync(transactionObj);
                await _unitOfWork.CompleteAsync();
                return new ResponseModel<string>
                { ResponseCode = ResponseCodes.OK, ResponseMessage = StatusMessages.OK, ResponseData = StatusMessages.OK };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new ResponseModel<string>
                { ResponseCode = ResponseCodes.SERVER_ERROR, ResponseMessage = StatusMessages.SERVER_ERROR, ResponseData = StatusMessages.FAILED };
            }
        }
        public async Task<ResponseModel<List<string>>> GetNonProcessedLedgerAsync()
        {
            try
            {
                var query = await _unitOfWork.TransactionRepository.FindAsync(x => x.IsLedgerProcessed == false);
                //Check if there is no transaction to be processed
                if (query == null)
                    return new ResponseModel<List<string>>
                    { ResponseCode = ResponseCodes.NOT_FOUND, ResponseMessage = StatusMessages.NOT_FOUND, ResponseData = null };

                //Get transaction to be processed to enable you fetch the ledgers
                var transactionIds = await query.Select(a=>a.Hash).ToListAsync();
                return new ResponseModel<List<string>>
                { ResponseCode = ResponseCodes.OK, ResponseMessage = StatusMessages.OK, ResponseData = transactionIds };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new ResponseModel<List<string>>
                { ResponseCode = ResponseCodes.SERVER_ERROR, ResponseMessage = StatusMessages.SERVER_ERROR, ResponseData = null };
            }
        }

        public async Task<ResponseModel<List<TransactionResponseDTO>>> GetTransactionsAsync(PaginationParameters paginationParameters)
        {
            try
            {
                var query = _unitOfWork.TransactionRepository.GetAll();
                var pagedtransaction = await query.ToPagedListAsync(paginationParameters.PageNumber, paginationParameters.MaxItem);
                var mapped = _mapper.Map<List<TransactionResponseDTO>>(pagedtransaction);
                if (query == null)
                    return new ResponseModel<List<TransactionResponseDTO>>
                    { ResponseCode = ResponseCodes.NOT_FOUND, ResponseMessage = StatusMessages.NOT_FOUND, ResponseData = null };

                return new ResponseModel<List<TransactionResponseDTO>>
                { ResponseCode = ResponseCodes.OK, ResponseMessage = StatusMessages.OK, ResponseData = mapped };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new ResponseModel<List<TransactionResponseDTO>>
                { ResponseCode = ResponseCodes.SERVER_ERROR, ResponseMessage = StatusMessages.SERVER_ERROR, ResponseData = null };
            }
        }

        public async Task<ResponseModel<List<LedgerResponseDTO>>> GetLedgersAsync(PaginationParameters paginationParameters)
        {
            try
            {
                var query = _unitOfWork.TransactionLedgersRepository.GetAll().Where(x=>x.Asset_Type== AssetType.NATIVE);
                var pagedtransaction = await query.ToPagedListAsync(paginationParameters.PageNumber, paginationParameters.MaxItem);
                var mapped = _mapper.Map<List<LedgerResponseDTO>>(pagedtransaction);
                if (query == null)
                    return new ResponseModel<List<LedgerResponseDTO>>
                    { ResponseCode = ResponseCodes.NOT_FOUND, ResponseMessage = StatusMessages.NOT_FOUND, ResponseData = null };

                return new ResponseModel<List<LedgerResponseDTO>>
                { ResponseCode = ResponseCodes.OK, ResponseMessage = StatusMessages.OK, ResponseData = mapped };
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return new ResponseModel<List<LedgerResponseDTO>>
                { ResponseCode = ResponseCodes.SERVER_ERROR, ResponseMessage = StatusMessages.SERVER_ERROR, ResponseData = null };
            }
        }

    }
}
