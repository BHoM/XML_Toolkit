using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    public abstract class GBXMLObject : IObject
    {
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class GBXML : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; } = "GBXML http://www.GBXML.org/schema";
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

    /***************************************************/
    /**** Document History Objects                  ****/
    /***************************************************/

    

    

    

    

    /***************************************************/
    /**** Zone Objects                              ****/
    /***************************************************/

    

    /***************************************************/
    /**** Material Objects                          ****/
    /***************************************************/

    

    /***************************************************/
    /**** Layer Objects                             ****/
    /***************************************************/

    

    

    /***************************************************/
    /**** Construction Objects                      ****/
    /***************************************************/

    

    

    /***************************************************/
    /**** Campus Objects                            ****/
    /***************************************************/

    

    

    

    

    

    

    

    

    

    

    

    

    

    

    

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class SpaceBoundary : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "isSecondLevelBoundary")]
        public string IsSecondLevelBoundary { get; set; } = "false";
        [XmlAttribute(AttributeName = "surfaceIdRef")]
        public string SurfaceIDRef { get; set; } = "";
        [XmlElement("PlanarGeometry")]
        public PlanarGeometry PlanarGeometry { get; set; } = new PlanarGeometry();
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class ProjectEntity : GBXMLObject, IObject
    {
        [XmlElement("URI")]
        public string URI { get; set; } = "Unknown";
        [XmlElement("GUID")]
        public string GUID { get; set; } = "Unknown";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class StationId : GBXMLObject, IObject
    {
        [XmlAttribute("IDType")]
        public string IDType { get; set; } = "Unknown";
        [XmlAttribute("ID")]
        public string ID { get; set; } = "";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class OAFlowPerArea : GBXMLObject, IObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "LPerSecPerSquareM";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class OAFlowPerPerson : GBXMLObject, IObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "LPerSec";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class DesignHeatT : GBXMLObject, IObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "C";
    }

    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class DesignCoolT : GBXMLObject, IObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "C";
    }

    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class UValue : GBXMLObject, IObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "WPerSquareMeterK";
    }

    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Absorptance : GBXMLObject, IObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "Fraction";
        [XmlAttribute("type")]
        public string Type { get; set; } = "ExtIR";
    }

    public class Roughness : GBXMLObject, IObject
    {
        [XmlAttribute("value")]
        public string Value { get; set; } = "VeryRough"; //TODO: what should the default value be?
    }
}
