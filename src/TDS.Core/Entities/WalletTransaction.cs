using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.Core.Entities
{
    public class WalletTransaction : BaseEntity<Guid>
    {
        public string Hash { get; set; }
        public string SourceAccount { get; set; }
        public string DestinationAccount { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Narration { get; set; }
        public bool IsLedgerProcessed { get; set; } = false;

    }
}
