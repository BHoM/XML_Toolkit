using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Layer : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "LayerID";
        [XmlElement("MaterialId")]
        public MaterialId MaterialID { get; set; } = new MaterialId();
    }
}