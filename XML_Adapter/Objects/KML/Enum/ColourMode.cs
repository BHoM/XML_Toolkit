using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BH.oM.External.XML.KMLSchema
{
    public enum ColourMode
    {
        [XmlEnum(Name = "normal")]
        Normal,
        [XmlEnum(Name = "random")]
        Random
    }
}
