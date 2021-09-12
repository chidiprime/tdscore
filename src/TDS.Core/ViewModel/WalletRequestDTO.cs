using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TDS.Core.ViewModel
{
    public class WalletRequestDTO
    {
        [Required(ErrorMessage ="Wallet Address is required")]
        [JsonProperty("address")]
        public string Account { get; set; }

        [Required(ErrorMessage = "Wallet Secret key is required")]
        [JsonProperty("secret")]
        public string Secret { get; set; }
    }
}
