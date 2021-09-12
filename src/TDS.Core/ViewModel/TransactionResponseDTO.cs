using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.Core.ViewModel
{
    public class TransactionResponseDTO
    {
        public string Hash { get; set; }
        public string SourceAccount { get; set; }
        public string DestinationAccount { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
    }
}
