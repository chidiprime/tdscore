using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TDS.Core.ViewModel
{
    public class TransferDTO
    {
        [Required(ErrorMessage ="Source Account is required")]
        public string SourceAccount { get; set; }

        [Required(ErrorMessage = "Destination Account is required")]
        public string DestinationAccount { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public string Amount { get; set; }
    }
}
