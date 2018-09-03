using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Zone : GBXMLObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "ZoneID";
        [XmlElement("AirChangesperHour")]
        public double AirChangesPerHour { get; set; } = 0;
        [XmlElement("OAFlowPerArea")]
        public OAFlowPerArea OAFlowPerArea { get; set; } = new OAFlowPerArea();
        [XmlElement("OAFlowPerPerson")]
        public OAFlowPerPerson OAFlowPerPerson { get; set; } = new OAFlowPerPerson();
        [XmlElement("DesignHeatT")]
        public DesignHeatT DesignHeatT { get; set; } = new DesignHeatT();
        [XmlElement("DesignCoolT")]
        public DesignCoolT DesignCoolT { get; set; } = new DesignCoolT();
        [XmlElement("TypeCode")]
        public double TypeCode { get; set; } = 0;
        [XmlElement("Name")]
        public string Name { get; set; } = "Zone";
        [XmlElement("CADObjectId")]
        public string CADObjectID { get; set; } = "Unknown";
    }
}