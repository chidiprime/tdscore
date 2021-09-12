using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.Core.ViewModel
{
    public class ApiLogDTO
    {
        public string HttpMethod { get; set; }
        public string RequestUri { get; set; }
        public string Payload { get; set; }
        public string Description { get; set; }
        public string Responsecode { get; set; }
        public DateTime PubDate { get; set; } = DateTime.Now;
        public String RequestAction { get; set; }
        public bool IsSuccessful { get; set; } = false;
        public string ReferenceId { get; set; }
    }
}
