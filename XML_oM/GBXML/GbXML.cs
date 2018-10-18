using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class GBXML : GBXMLObject
    {
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; } = "gbxml http://www.gbxml.org/schema";
        [XmlAttribute(AttributeName = "temperatureUnit")]
        public string TemperatureUnit { get; set; } = "C";
        [XmlAttribute(AttributeName = "lengthUnit")]
        public string LengthUnit { get; set; } = "Meters";
        [XmlAttribute(AttributeName = "areaUnit")]
        public string AreaUnit { get; set; } = "SquareMeters";
        [XmlAttribute(AttributeName = "volumeUnit")]
        public string VolumeUnit { get; set; } = "CubicMeters";
        [XmlAttribute(AttributeName = "useSIUnitsForResults")]
        public string UseSIUnitsForResults { get; set; } = "true";
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; } = "0.37";
        [XmlElement("Campus")]
        public Campus Campus { get; set; } = new Campus();
        [XmlElement("Construction")]
        public Construction[] Construction { get; set; }
        [XmlElement("Layer")]
        public Layer[] Layer { get; set; }
        [XmlElement("Material")]
        public Material[] Material { get; set; }
        [XmlElement("Zone")]
        public Zone[] Zone { get; set; } = new List<Zone> { new Zone() }.ToArray();
        [XmlElement("DocumentHistory")]
        public DocumentHistory DocumentHistory { get; set; } = new DocumentHistory();
    }
}