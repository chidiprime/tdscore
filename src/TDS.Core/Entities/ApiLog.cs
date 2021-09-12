using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.Core.Entities
{
    public class ApiLog : BaseEntity<Guid>
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
