using System;
using System.Collections.Generic;
using System.Text;
using TDS.Core.Entities;

namespace TDS.Core.Common
{
    public class TransactionObj
    {
        public static WalletTransaction Construct(string reference, string amount, string sourceAccount, string destinationAccount, string type)
        {
            return new WalletTransaction
            {
                Id = Guid.NewGuid(),
                Hash = reference,
                Amount = Convert.ToDecimal(amount),
                SourceAccount = sourceAccount,
                DestinationAccount = destinationAccount,
                Type = type,
                IsLedgerProcessed=false,
                Narration = $"{sourceAccount} transferred {amount} to {destinationAccount}"
            };
        }
    }
}
