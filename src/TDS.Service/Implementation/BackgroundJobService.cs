using AutoMapper;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS.Core.Common;
using TDS.Core.Entities;
using TDS.Core.Interfaces;
using TDS.Core.ViewModel;
using static TDS.Core.Common.Constant;

namespace TDS.Service.Implementation
{
    public class BackgroundJobService : IBackgroundService
    {
        private readonly ILog _logger;
        private readonly IWalletService _walletservice;
        private readonly IOperationService _operationService;
        private readonly IRestClient _restclient;
        private readonly IMapper _mapper;
        private readonly Settings _settings;
        private readonly IUnitOfWork _unitOfWork;
        public BackgroundJobService(ILog logger, IWalletService walletservice, 
            IOperationService operationService, IRestClient restclient, 
            IMapper mapper, IOptions<Settings> settings, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _walletservice = walletservice;
            _operationService = operationService;
            _restclient = restclient;
            _mapper = mapper;
            _settings = settings.Value;
            _unitOfWork = unitOfWork;
        }
        public async Task ActivateWalletsAsync()
        {
            try
            {
                var query = await _walletservice.GetNonActivatedWalletsAsync();
                List<WalletResponseDTO> nonActivatedWallets = query.ResponseData;
                if (nonActivatedWallets != null || nonActivatedWallets.Count != 0)
                {
                    foreach (WalletResponseDTO nonActivatedWallet in nonActivatedWallets)
                    {
                        await _walletservice.ActivateWalletAsync(nonActivatedWallet.Account);

                    }
                }
            }
            catch (Exception ex)
            {
               _logger.Error(ex);
            }
        }

        public async Task SaveLedgersAsync()
        {
            try
            {

                var transactionIds = await _operationService.GetNonProcessedLedgerAsync();
                foreach (string transactionId in transactionIds.ResponseData)
                {
                    string uri = $"{_settings.Server}/transactions/{transactionId}/effects";
                    var request = new RestRequest(uri, Method.GET) { RequestFormat = DataFormat.Json };
                    var response = await _restclient.ExecuteAsync<TransactionRecords>(request);
                    if (response.StatusCode.ToString() == StatusMessages.OK.ToUpper())
                    {
                        var getTransaction = await _unitOfWork.TransactionRepository.SingleOrDefaultAync(x => x.Hash == transactionId);
                        getTransaction.IsLedgerProcessed = true;
                        var responseData = response.Data._Embedded.Records;
                        foreach (var responseObj in responseData)
                        {
                            var transactionDetails = _mapper.Map<TransactionLedger>(responseObj);
                            transactionDetails.Hash = transactionId;
                            transactionDetails.ReferenceId = responseObj.Id;
                            await _unitOfWork.TransactionLedgersRepository.AddAsync(transactionDetails);

                        }
                        _unitOfWork.TransactionRepository.Update(getTransaction);
                        await _unitOfWork.CompleteAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

            }
        }
    }
}
