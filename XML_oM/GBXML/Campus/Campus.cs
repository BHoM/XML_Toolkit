using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Campus : GBXMLObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "CampusID";
        [XmlElement("Location")]
        public Location Location { get; set; } = new Location();
        [XmlElement("Building")]
        public Building[] Building { get; set; } = new List<Building> { new Building() }.ToArray();
        [XmlElement("Surface")]
        public List<Surface> Surface { get; set; } = new List<Surface>();
        [XmlElement("DaylightSavings")]
        public string DaylightSavings { get; set; } = "false";
        [XmlElement("Name")]
        public string Name { get; set; } = "Campus";
    }
}