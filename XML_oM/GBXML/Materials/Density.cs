using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Density : GBXMLObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "KgPerCubicM";

        [XmlText]
        public string Value { get; set; } = "0";
    }
}