using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public class WalletController : Controller
    {
        protected readonly IWalletService _walletService;
        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }
        /// <summary>
        /// This endpoint enables you to create multiple accounts
        /// </summary>
        /// <param name="walletRequestDTO"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), 200)]
        [HttpPost]
        public async Task<ApiResponse> Post([FromBody] List<WalletRequestDTO> walletRequestDTO)
        {
            if(!ModelState.IsValid)
            {
                throw new ApiException(ModelState.AllErrors());
            }
            ResponseModel<string> result;
            result = await _walletService.InsertAsync(walletRequestDTO);
            return ResponseFactory<string>.GetResponse(result);
        }
       
        /// <summary>
        /// This endpoint enables you to get account information
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(WalletResponseDTO), 200)]
        [HttpGet("{account}")]
        public async Task<ApiResponse> GetAccount(string account)
        {
            ResponseModel<WalletResponseDTO> result;
            result = await _walletService.GetWalletAsync(account);
            return ResponseFactory<WalletResponseDTO>.GetResponse(result);
        }
        /// <summary>
        /// This endpoint enables you to get all non activated accounts
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WalletResponseDTO>), 200)]
        [HttpGet("nonactivatedwallets")]
        public async Task<ApiResponse> GetNonActiatedWallets()
        {
            ResponseModel<List<WalletResponseDTO>> result;
            result = await _walletService.GetNonActivatedWalletsAsync();
            return ResponseFactory<List<WalletResponseDTO>>.GetResponse(result);
        }
        /// <summary>
        /// This endpoint enables you to get all activated accounts
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<WalletResponseDTO>), 200)]
        [HttpGet("activatedwallets")]
        public async Task<ApiResponse> GetActiatedWallets()
        {
            ResponseModel<List<WalletResponseDTO>> result;
            result = await _walletService.GetActivatedWalletsAsync();
            return ResponseFactory<List<WalletResponseDTO>>.GetResponse(result);
        }

        /// <summary>
        ///  This endpoint enables you to know if an account is activated
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(bool), 200)]
        [HttpGet("{address}/isactivated")]
        public async Task<ApiResponse> CheckIfActivated(string account)
        {
            ResponseModel<bool> result;
            result = await _walletService.IsWalletActivatedAsync(account);
            return ResponseFactory<bool>.GetResponse(result);
        }

      
    }
}
