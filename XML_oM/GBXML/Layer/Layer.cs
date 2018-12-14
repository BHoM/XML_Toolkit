using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Layer : GBXMLObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "LayerID";
        [XmlElement("MaterialId")]
        public MaterialId[] MaterialID { get; set; } = new List<MaterialId> { new MaterialId() }.ToArray();
    }
}