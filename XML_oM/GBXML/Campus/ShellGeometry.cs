using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class ShellGeometry : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "ShellGeometryID";
        [XmlElement("ClosedShell")]
        public ClosedShell ClosedShell { get; set; } = new ClosedShell();
    }
}