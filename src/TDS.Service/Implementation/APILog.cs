using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS.Core.Entities;
using TDS.Core.Interfaces;
using TDS.Core.ViewModel;

namespace TDS.Service.Implementation
{
    public class APILog : IAPILog
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog _logger;
        public APILog(IUnitOfWork unitOfWork, ILog logger)
        {
            _logger = logger;

        }
        public async Task InsertAsync(ApiLogDTO apiLogDTO)
        {
            try
            {
                var apilog = new ApiLog
                {
                    RequestAction = apiLogDTO.RequestAction,
                    Description = apiLogDTO.Description,
                    HttpMethod = apiLogDTO.HttpMethod,
                    IsSuccessful = apiLogDTO.IsSuccessful,
                    Payload = apiLogDTO.Payload,
                    PubDate = DateTime.Now,
                    RequestUri = apiLogDTO.RequestUri,
                    Responsecode = apiLogDTO.Responsecode,
                  
                };
               await _unitOfWork.ApiLogRepository.AddAsync(apilog);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
