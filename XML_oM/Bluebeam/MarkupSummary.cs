using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Base;
using System.Xml.Serialization;

namespace BH.oM.XML.Bluebeam
{
    [Serializable]
    [XmlRoot(ElementName = "MarkupSummary", IsNullable = false)]
    public class MarkupSummary : BluebeamObject
    {
        [XmlElement("Markup")]
        public virtual List<Markup> Markup { get; set; }
    }
}
