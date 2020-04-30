using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BH.oM.External.XML.KMLSchema
{
    public enum AltitudeMode
    {
        //todo implement gx:altitudeMode: clampToSeaFloor, relativeToSeaFloor
        [XmlEnum(Name = "clampToGround")]
        ClampToGround,
        [XmlEnum(Name = "relativeToGround")]
        RelativeToGround,
        [XmlEnum(Name = "absolute")]
        Absolute
    }
}
