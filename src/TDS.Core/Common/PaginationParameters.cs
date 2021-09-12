using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.Core.Common
{
    public class PaginationParameters
    {
        public int PageNumber { get; set; } = 1;
        public int MaxItem { get; set; } = 50;
    }
}
