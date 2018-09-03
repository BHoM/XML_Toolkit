using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Building : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "buildingType")]
        public string BuildingType { get; set; } = "Unknown";
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "BuildingID";
        [XmlElement("StreetAddress")]
        public string StreetAddress { get; set; } = "Unknown";
        [XmlElement("Area")]
        public float Area { get; set; } = 0;
        [XmlElement("Space")]
        public List<Space> Space { get; set; } = new List<Space>();
        [XmlElement("BuildingStorey")]
        public BuildingStorey[] BuildingStorey { get; set; } = new List<BuildingStorey> { new BuildingStorey() }.ToArray();
        [XmlElement("Name")]
        public string Name { get; set; } = "Building";
        [XmlElement("Description")]
        public string Description { get; set; } = "None";
    }
}