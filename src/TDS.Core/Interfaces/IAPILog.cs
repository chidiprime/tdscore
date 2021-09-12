using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS.Core.ViewModel;

namespace TDS.Core.Interfaces
{
    public interface IAPILog
    {
        Task InsertAsync(ApiLogDTO apiLogDTO);

    }
}
