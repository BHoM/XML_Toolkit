using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class BuildingStorey : GBXMLObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "StoreyID";
        [XmlElement("Level")]
        public float Level { get; set; } = 0;
        [XmlElement("PlanarGeometry")]
        public PlanarGeometry PlanarGeometry { get; set; } = new PlanarGeometry();
        [XmlElement("Name")]
        public string Name { get; set; } = "Storey";
    }
}