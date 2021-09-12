using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.Core.Entities
{
    public class Wallet:BaseEntity<Guid>
    {
        public string Account { get; set; }
        public string Secret { get; set; }
        public bool IsActivated { get; set; }
    }
}
