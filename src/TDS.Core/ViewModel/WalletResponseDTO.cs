using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.Core.ViewModel
{
    public class WalletResponseDTO
    {
        public string Account { get; set; }
        public string Secret { get; set; }
        public bool IsActivated { get; set; } = false;
    }

   
}
