using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Opening : GBXMLObject
    {
        [XmlAttribute(AttributeName = "constructionIdRef")]
        public string ConstructionIDRef { get; set; } = "";
        [XmlAttribute(AttributeName = "openingType")]
        public string OpeningType { get; set; } = "FixedWindow";
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "OpeningID";
        [XmlElement("RectangularGeometry")]
        public RectangularGeometryOpenings RectangularGeometry { get; set; } = new RectangularGeometryOpenings();
        [XmlElement("PlanarGeometry")]
        public PlanarGeometry PlanarGeometry { get; set; } = new PlanarGeometry();
        [XmlElement("CADObjectId")]
        public string CADObjectID { get; set; } = "WinInst: SIM_EXT_GLZ [xxxxxx]";
        [XmlElement("Name")]
        public string Name { get; set; } = "Opening";
    }
}