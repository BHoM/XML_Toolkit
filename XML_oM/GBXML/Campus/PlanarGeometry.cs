using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class PlanarGeometry : GBXMLObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "PlanarGeometryID";
        [XmlElement("PolyLoop")]
        public Polyloop PolyLoop { get; set; } = new Polyloop();
    }
}