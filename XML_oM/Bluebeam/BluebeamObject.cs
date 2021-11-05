using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Base;
using System.Xml.Serialization;

namespace BH.oM.XML.Bluebeam
{
    public class BluebeamObject : IBHoMObject
    {
        [XmlIgnore]
        public virtual Guid BHoM_Guid { get; set; }
        [XmlIgnore]
        public virtual Dictionary<string, object> CustomData { get; set; }
        [XmlIgnore]
        public virtual string Name { get; set; }
        [XmlIgnore]
        public virtual FragmentSet Fragments { get; set; }
        [XmlIgnore]
        public virtual HashSet<string> Tags { get; set; }
    }
}
