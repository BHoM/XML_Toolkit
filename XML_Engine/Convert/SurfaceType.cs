using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHE = BH.oM.Environmental;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        /***************************************************/

        public static string ToGbXMLSurfaceType(BHE.Elements.BuildingElementPanel bHoMPanel)
        {

            switch (bHoMPanel.ElementType)
            {
                case "EXTERNALWALL":
                    return "ExteriorWall";
                case "INTERNALWALL":
                        return "InteriorWall";
                case "ROOFELEMENT":
                        return "Roof";
                case "INTERNALFLOOR":
                        return "InteriorFloor";
                case "EXPOSEDFLOOR":
                        return "ExposedFloor";
                case "SHADEELEMENT":
                        return "Shade";
                case "UNDERGROUNDWALL":
                        return "UndergroundWall";
                case "UNDERGROUNDSLAB":
                        return "UndergroundSlab";
                case "CEILING":
                        return "Ceiling";
                case "UNDERGROUNDCEILING":
                        return "UndergroundCeiling";
                case "RAISEDFLOOR":
                        return "RaisedFloor";
                case "SLABONGRADE":
                        return "SlabOnGrade";
                default:
                        return ""; //Adiabatic
            }
        }

        /***************************************************/

        //public static string ToGbXMLSurfaceType(BHE.Elements.BuildingElement bHoMBuildingElement)
        //{
        //    switch (bHoMBuildingElement.BuildingElementProperties.BuildingElementType)
        //    {
        //        case BHE.Elements.BuildingElementType.Ceiling:
        //            return "Ceiling";
        //        case BHE.Elements.BuildingElementType.Roof:
        //            return "Roof";
        //        default:
        //            return "";
        //    }
        //}

        /***************************************************/
    }
}
