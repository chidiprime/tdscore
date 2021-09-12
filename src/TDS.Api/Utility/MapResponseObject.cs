using AutoWrapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.Api
{
    public class MapResponseObject
    {
        [AutoWrapperPropertyMap(Prop.Result)]
        public object Data { get; set; }
    }
}
