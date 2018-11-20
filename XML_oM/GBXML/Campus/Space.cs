using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Space : GBXMLObject
    {
        [XmlAttribute(AttributeName = "zoneIdRef")]
        public string ZoneIDRef { get; set; } = "ZoneID";
        [XmlAttribute(AttributeName = "conditionType")]
        public string ConditionType { get; set; } = "Unknown";
        [XmlAttribute(AttributeName = "buildingStoreyIdRef")]
        public string BuildingStoreyIDRef { get; set; } = "StoreyID";
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "SpaceID";
        [XmlElement("Area")]
        public double Area { get; set; } = 0;
        [XmlElement("Volume")]
        public double Volume { get; set; } = 0;
        [XmlElement("PlanarGeometry")]
        public PlanarGeometry PlanarGeoemtry { get; set; } = new PlanarGeometry();
        [XmlElement("ShellGeometry")]
        public ShellGeometry ShellGeometry { get; set; } = new ShellGeometry();
        [XmlElement("SpaceBoundary")]
        public SpaceBoundary[] SpaceBoundary { get; set; } = new List<SpaceBoundary> { new SpaceBoundary() }.ToArray();
        [XmlElement("Name")]
        public string Name { get; set; } = "Space";
        [XmlElement("Description")]
        public string Description { get; set; } = "None";
        [XmlElement("CADObjectId")]
        public string CADObjectID { get; set; } = "xxxxxx";
    }
}