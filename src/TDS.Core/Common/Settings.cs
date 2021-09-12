using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.Core.Common
{
    public class Settings
    {
        public string CronExpression { get; set; }
        public string WalletToActivate { get; set; }
        public string FriendlyBotUrl { get; set; }
        public string Network { get; set; }
        public string Server { get; set; }
    }
}
