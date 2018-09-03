using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class RectangularGeometry : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "RectangularGeometryID";
        [XmlElement("Azimuth")]
        public double Azimuth { get; set; }
        [XmlElement("CartesianPoint")]
        public CartesianPoint CartesianPoint { get; set; } = new CartesianPoint();
        [XmlElement("Tilt")]
        public double Tilt { get; set; }
        [XmlElement("Width")]
        public double Width { get; set; }
        [XmlElement("Height")]
        public double Height { get; set; }
    }
}