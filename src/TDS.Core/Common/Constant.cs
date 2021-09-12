using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.Core.Common
{
    public class Constant
    {
        public class StatusMessages
        {

            public static string OK = "Ok";
            public static string NOT_FOUND = "Record not found";
            public static string SERVER_ERROR = "Server error";
            public static string BAD_REQUEST = "Bad Request";
            public static string FAILED= "Failed";
            public static string DUPLICATE_ENTRY = "Duplicate entry";
            public static string UNAUTHORIZED = "Unauthorize";
            public static string TRANSFER_FAILED = "ERROR Processing transfer";

        }
        public class TransactionType
        {

            public static string CREDIT = "CREDIT";
            public static string DEBIT = "DEBIT";
            public static string TRANSFER = "TRANSFER";
            
        }
        public class AssetType
        {

            public static string NATIVE = "native";
           
        }

        public class ResponseCodes
        {

            public static string OK = "200";
            public static string CREATED = "201";
            public static string NOT_FOUND = "404";
            public static string BAD_REQUEST = "400";
            public static string UNAUTHORIZED = "401";
            public static string SERVER_ERROR= "500";

        }
        public class RequestAction
        {

            public static string POST = "POST";
            public static string GET = "GET";
            public static string PUT = "PUT";
            public static string DELETE = "DELETE";
            public static string PATCH = "PATCH";
        
        }
        public class ResponseMessages
        {

            public static string WALLET_REQUIRED = "Insert a stellar wallet address";
            public static string WALLETS_SAVED= "stellar wallet address sucessfully saved";
            public static string WALLETACTIVATION_FAILED = "Failed to activate and fund wallet using friendbot ";
            public static string WALLETACTIVATION_SUCCESSFUL= "Activate and fund wallet using friendbot was successful ";
            public static string NOT_FOUND = "Record not found";
            public static string SERVER_ERROR = "Server error";
            public static string BAD_REQUEST = "Bad Request";
            public static string FAILED = "Failed";
            public static string DUPLICATE_ENTRY = "Duplicate entry";
            public static string UNAUTHORIZED = "Unauthorize";

        }
    }
}
