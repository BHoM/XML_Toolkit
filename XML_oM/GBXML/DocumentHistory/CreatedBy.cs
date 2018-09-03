using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class CreatedBy : GBXMLObject
    {
        [XmlAttribute(AttributeName = "personId")]
        public string PersonID { get; set; } = "BuroHappold";
        [XmlAttribute(AttributeName = "programId")]
        public string ProgramID { get; set; } = "BHoMGBXML";
        [XmlAttribute(AttributeName = "date")]
        public string Date { get; set; } = "00-00-00";
        [XmlElement("CADModelId")]
        public string CADModelID { get; set; } = "Unknown";
    }
}