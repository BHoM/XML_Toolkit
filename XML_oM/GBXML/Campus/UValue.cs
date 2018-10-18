using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class UValue : GBXMLObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "WPerSquareMeterK";
    }
}