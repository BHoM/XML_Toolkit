using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class CartesianPoint : GBXMLObject
    {
        [XmlElement("Coordinate")]
        public string[] Coordinate { get; set; } = new List<string> { "0" }.ToArray();
    }
}