using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.Core.ViewModel
{
    public class TransactionRecords
    {
        public Embedded _Embedded { get; set; }
    }
    public class TransactionLedgerDTO
    {
        public string Id { get; set; }
        public string Hash { get; set; }
        public string ReferenceId { get; set; }
        public string Paging_Token { get; set; }
        public string Account { get; set; }
        public string Type { get; set; }
        public string Created_at { get; set; }
        public string Type_I { get; set; }
        public string Asset_Type { get; set; }
        public string Amount { get; set; }
    }
    public class Embedded
    {
        public List<TransactionLedgerDTO> Records { get; set; }
    
    }
}
