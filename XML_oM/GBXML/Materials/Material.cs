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
        public RValue RValue { get; set; } = new RValue();
        [XmlElement("Thickness")]
        public double Thickness { get; set; } = 0.001;
        [XmlElement("Conductivity")]
        public Conductivity Conductivity { get; set; } = new Conductivity();
        [XmlElement("Density")]
        public Density Density { get; set; } = new Density();
        [XmlElement("SpecificHeat")]
        public SpecificHeat SpecificHeat { get; set; } = new SpecificHeat();
    }
}