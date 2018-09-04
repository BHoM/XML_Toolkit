using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class ClosedShell : GBXMLObject
    {
        [XmlElement("PolyLoop")]
        public Polyloop[] PolyLoop { get; set; } = new List<Polyloop> { new Polyloop() }.ToArray();
    }
}