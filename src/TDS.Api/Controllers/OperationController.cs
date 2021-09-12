using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDS.Api.Utility;
using TDS.Core.Common;
using TDS.Core.Interfaces;
using TDS.Core.ViewModel;

namespace TDS.Api.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]

    public class OperationController : Controller
    {
        protected readonly IOperationService _operationService;
        public OperationController(IOperationService operationService)
        {
            _operationService = operationService;
        }
        /// <summary>
        /// This endpoint enables you to transfer asset from one accouint to another
        /// </summary>
        /// <param name="transferDTO"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), 200)]
        [HttpPost("transfer")]
        public async Task<ApiResponse> Transfer([FromBody] TransferDTO transferDTO)
        {
            if (!ModelState.IsValid)
            {
                throw new ApiException(ModelState.AllErrors());
            }
            ResponseModel<string> result;
            result = await _operationService.TransferAsync(transferDTO);
            return ResponseFactory<string>.GetResponse(result);
        }
        /// <summary>
        /// This endpoint enables you to get transaction histories
        /// </summary>
        /// <param name="paginationParameters"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(TransactionResponseDTO), 200)]
        [HttpGet("transactionhistories")]
        public async Task<ApiResponse> Histories([FromQuery] PaginationParameters  paginationParameters)
        {
            ResponseModel<List<TransactionResponseDTO>> result;
            result = await _operationService.GetTransactionsAsync(paginationParameters);
            return ResponseFactory<List<TransactionResponseDTO>>.GetResponse(result);
        }
        /// <summary>
        /// This endpoint enables you to get transaction ledgers
        /// </summary>
        /// <param name="paginationParameters"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(TransactionLedgerDTO), 200)]
        [HttpGet("transactionledgers")]
        public async Task<ApiResponse> Ledgers([FromQuery] PaginationParameters paginationParameters)
        {
            ResponseModel<List<LedgerResponseDTO>> result;
            result = await _operationService.GetLedgersAsync(paginationParameters);
            return ResponseFactory<List<LedgerResponseDTO>>.GetResponse(result);
        }
        /// <summary>
        /// This endpoint enables you to get transaction histories in CSV format
        /// </summary>
        /// <param name="paginationParameters"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(TransactionLedgerDTO), 200)]
        [HttpGet("transactionhistories/csv")]
        public async Task<IActionResult> TransactionhistoriesCSV([FromQuery] PaginationParameters paginationParameters)
        {
            ResponseModel<List<TransactionResponseDTO>> result;
            result = await _operationService.GetTransactionsAsync(paginationParameters);
            var ms = CSVHelper<TransactionResponseDTO>.Download(result.ResponseData);
            return File(ms.ToArray(), "text/csv", $"export_{DateTime.UtcNow.Ticks}.csv");
        }
        /// <summary>
        /// This endpoint enables you to get transaction ledgers in CSV format
        /// </summary>
        /// <param name="paginationParameters"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(TransactionLedgerDTO), 200)]
        [HttpGet("transactionledgers/csv")]
        public async Task<IActionResult> LedgersCSV([FromQuery] PaginationParameters paginationParameters)
        {
            ResponseModel<List<LedgerResponseDTO>> result;
            result = await _operationService.GetLedgersAsync(paginationParameters);
           var ms= CSVHelper<LedgerResponseDTO>.Download(result.ResponseData);
            return File(ms.ToArray(), "text/csv", $"export_{DateTime.UtcNow.Ticks}.csv");
        }

    }
    
}
