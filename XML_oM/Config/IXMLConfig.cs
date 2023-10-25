using BH.oM.Adapter;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.Adapters.XML
{
    public interface IXMLConfig : IObject
    {
        FileSettings File { get; set; }
    }
}
