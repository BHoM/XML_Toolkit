using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace BH.oM.XML.Bluebeam
{
    [Serializable]
    [XmlRoot(ElementName = "Markup", IsNullable = false)]
    public class Markup : BluebeamObject
    {
        [XmlAttribute("Page_Label")]
        public virtual int PageLabel { get; set; }

        [XmlAttribute("Subject")]
        public virtual string Subject { get; set; }

        [XmlAttribute("Space")]
        public virtual string Space { get; set; }

        [XmlAttribute("Author")]
        public virtual string Author { get; set; }

        [XmlAttribute("Date")]
        public virtual DateTime Date { get; set; }

        [XmlAttribute("Colour")]
        public virtual string Colour { get; set; }

        [XmlAttribute("Comments")]
        public virtual string Comments { get; set; }

        [XmlAttribute("Length")]
        public virtual double Length { get; set; }

        [XmlAttribute("Area")]
        public virtual double Area { get; set; }

        [XmlAttribute("Label")]
        public virtual string Label { get; set; }

        [XmlAttribute("Depth")]
        public virtual double Depth { get; set; }

        [XmlAttribute("Layer")]
        public virtual string Layer { get; set; }
    }
}
