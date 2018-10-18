using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class ProjectEntity : GBXMLObject
    {
        [XmlElement("URI")]
        public string URI { get; set; } = "Unknown";
        [XmlElement("GUID")]
        public string GUID { get; set; } = "Unknown";
    }
}