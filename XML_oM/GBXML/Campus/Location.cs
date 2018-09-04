using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Location : GBXMLObject
    {
        [XmlElement("StationId")]
        public StationId StationID { get; set; } = new StationId();
        [XmlElement("ZipcodeOrPostalCode")]
        public string ZipcodeOrPostalCode { get; set; } = "";
        [XmlElement("Longitude")]
        public double Longitude { get; set; } = 0;
        [XmlElement("Latitude")]
        public double Latitude { get; set; } = 0;
        [XmlElement("Elevation")]
        public double Elevation { get; set; } = 0;
        [XmlElement("CADModelAzimuth")]
        public double CADModelAzimuth { get; set; } = 0;
        [XmlElement("Name")]
        public string Name { get; set; } = "Location";
    }
}