using AutoWrapper.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Core.Common;
using static TDS.Core.Common.Constant;

namespace TDS.Api.Utility
{
    public static class ResponseFactory<T>
    {
        public static ApiResponse GetResponse(ResponseModel<T> responseModel)
        {

            if (responseModel.ResponseCode != ResponseCodes.CREATED && responseModel.ResponseCode != ResponseCodes.OK)
            {

                if (responseModel.ResponseCode == ResponseCodes.BAD_REQUEST)
                    throw new ApiException(responseModel.ResponseMessage, 400);
                if (responseModel.ResponseCode == ResponseCodes.NOT_FOUND)
                    throw new ApiException(responseModel.ResponseMessage, 404);
                if (responseModel.ResponseCode == ResponseCodes.UNAUTHORIZED)
                    throw new ApiException(responseModel.ResponseMessage, 401);
                throw new ApiException(responseModel.ResponseMessage, 500);
            }
            return new ApiResponse(responseModel.ResponseMessage, responseModel.ResponseData, 200);
        }
    }
}
