using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class ProgramInfo : GBXMLObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "BHoMGBXML";
        [XmlElement("CompanyName")]
        public string CompanyName { get; set; } = "BuroHappold Engineering";
        [XmlElement("ProductName")]
        public string ProductName { get; set; } = "Autodesk Revit 2018 BEES";
        [XmlElement("Version")]
        public string Version { get; set; } = "2018 20170223_1515(x64)";
        [XmlElement("Platform")]
        public string Platform { get; set; } = "Microsoft Windows";
        [XmlElement("ProjectEntity")]
        public ProjectEntity ProjectEntity { get; set; } = new ProjectEntity();
    }
}