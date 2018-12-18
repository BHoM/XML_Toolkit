using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Absorptance : GBXMLObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "Fraction";
        [XmlAttribute("type")]
        public string Type { get; set; } = "ExtIR";
        [XmlText]
        public string Value { get; set; } = "0";
    }
}