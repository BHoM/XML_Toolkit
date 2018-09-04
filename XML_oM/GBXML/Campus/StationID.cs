﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class StationId : GBXMLObject
    {
        [XmlAttribute("IDType")]
        public string IDType { get; set; } = "Unknown";
        [XmlAttribute("ID")]
        public string ID { get; set; } = "";
    }
}