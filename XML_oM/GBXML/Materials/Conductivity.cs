using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Conductivity : GBXMLObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "WPerMeterK";

        [XmlText]
        public string Value { get; set; } = "0";
    }
}