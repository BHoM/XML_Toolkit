using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Material : GBXMLObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "MaterialID";
        [XmlElement("Name")]
        public string Name { get; set; } = "Material";
        [XmlElement("R-value")]
        public double RValue { get; set; } = 0;
        [XmlElement("Thickness")]
        public double Thickness { get; set; } = 0.001;
        [XmlElement("Conductivity")]
        public double Conductivity { get; set; } = 0;
        [XmlElement("Density")]
        public double Density { get; set; } = 0;
        [XmlElement("SpecificHeat")]
        public double SpecificHeat { get; set; } = 0;
    }
}