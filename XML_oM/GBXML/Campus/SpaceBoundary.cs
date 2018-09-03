using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class SpaceBoundary : GBXMLObject
    {
        [XmlAttribute(AttributeName = "isSecondLevelBoundary")]
        public string IsSecondLevelBoundary { get; set; } = "false";
        [XmlAttribute(AttributeName = "surfaceIdRef")]
        public string SurfaceIDRef { get; set; } = "";
        [XmlElement("PlanarGeometry")]
        public PlanarGeometry PlanarGeometry { get; set; } = new PlanarGeometry();
    }
}