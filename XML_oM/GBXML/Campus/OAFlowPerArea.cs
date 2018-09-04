using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class OAFlowPerArea : GBXMLObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "LPerSecPerSquareM";
    }
}