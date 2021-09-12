using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TDS.Core.Common;

namespace TDS.Core.Interfaces
{
    public interface IBackgroundService
    {
      Task ActivateWalletsAsync();
      Task SaveLedgersAsync();
    }
}
