using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class RectangularGeometryOpenings : GBXMLObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "RectangularGeometryID";
        [XmlElement("CartesianPoint")]
        public CartesianPoint CartesianPoint { get; set; } = new CartesianPoint();
        [XmlElement("Width")]
        public double Width { get; set; }
        [XmlElement("Height")]
        public double Height { get; set; }
    }
}