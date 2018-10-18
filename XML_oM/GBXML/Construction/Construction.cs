using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Construction : GBXMLObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "ConstructionID";
        [XmlElement("U-value")]
        public UValue UValue { get; set; } = new UValue();
        [XmlElement("Absorptance")]
        public Absorptance Absorptance { get; set; } = new Absorptance();
        [XmlElement("Roughness")]
        public Roughness Roughness { get; set; } = new Roughness();
        [XmlElement("LayerId")]
        public LayerId LayerID { get; set; } = new LayerId();
        [XmlElement("Name")]
        public string Name { get; set; } = "Construction";
    }
}