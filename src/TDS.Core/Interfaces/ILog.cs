using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.Core.Interfaces
{
    public interface ILog
    {
        void Error(Exception ex);
        void Info(object data, string message);
        void Write(string msg);
    }
}
