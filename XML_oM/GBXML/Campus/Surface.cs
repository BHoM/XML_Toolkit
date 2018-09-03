using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Surface : GBXMLObject
    {
        [XmlAttribute(AttributeName = "surfaceType")]
        public string SurfaceType { get; set; } = "Unknown";
        [XmlAttribute(AttributeName = "exposedToSun")]
        public string ExposedToSun { get; set; } = "false";
        [XmlAttribute(AttributeName = "constructionIdRef")]
        public string ConstructionIDRef { get; set; } = "";
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "SurfaceID";
        [XmlElement("AdjacentSpaceId")]
        public AdjacentSpaceId[] AdjacentSpaceID { get; set; }
        [XmlElement("RectangularGeometry")]
        public RectangularGeometry RectangularGeometry { get; set; } = new RectangularGeometry();
        [XmlElement("PlanarGeometry")]
        public PlanarGeometry PlanarGeometry { get; set; } = new PlanarGeometry();
        [XmlElement("Opening")]
        public Opening[] Opening { get; set; }
        [XmlElement("CADObjectId")]
        public string CADObjectID { get; set; } = "";
        [XmlElement("Name")]
        public string Name { get; set; } = "Surface";
    }
}