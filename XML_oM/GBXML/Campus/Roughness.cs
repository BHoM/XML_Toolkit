using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    public class Roughness : GBXMLObject
    {
        [XmlAttribute("value")]
        public string Value { get; set; } = "Rough";
    }
}