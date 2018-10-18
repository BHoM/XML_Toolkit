using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class DocumentHistory : GBXMLObject
    {
        [XmlElement("ProgramInfo")]
        public ProgramInfo ProgramInfo { get; set; } = new ProgramInfo();
        [XmlElement("CreatedBy")]
        public CreatedBy CreatedBy { get; set; } = new CreatedBy();
        [XmlElement("PersonInfo")]
        public PersonInfo PersonInfo { get; set; } = new PersonInfo();
    }
}