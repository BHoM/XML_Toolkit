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

        public static string ToGbXMLSurfaceType(this BHE.Elements.BuildingElement bHoMBuildingElement)
        {
            string type = "Air";
            if (bHoMBuildingElement.BuildingElementProperties != null)
            {
                if (bHoMBuildingElement.BuildingElementProperties.CustomData.ContainsKey("SAM_BuildingElementType"))
                {
                    object aObject = bHoMBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"];
                    if (aObject != null)
                        type = ToGbXMLSurfaceType(aObject.ToString()); //modifies the string
                }
                    return type;
            }
            else
            {
                type = ToGbXMLSurfaceType((bHoMBuildingElement.BuildingElementGeometry as BHE.Elements.BuildingElementPanel).ElementType);
                return type;
            }
        }

        /***************************************************/
        //String modification for surface types
        public static string ToGbXMLSurfaceType(this string type)
        {
            switch (type)
            {
                //Strings from Revit
                case "Rooflight":
                    return "Rooflight";
                case "Roof":
                    return "Roof";
                case "External Wall":
                    return "ExteriorWall";
                case "Internal Wall":
                    return "InteriorWall";
                case "Internal Floor":
                    return "InteriorFloor";
                case "Shade":
                    return "Shade";
                case "Underground Wall":
                    return "UndergroundWall";
                case "Underground Slab":
                    return "UndergroundSlab";
                case "Internal Ceiling":
                    return "Ceiling";
                case "Underground Ceiling":
                    return "UndergroundCeiling";
                case "Raised Floor":
                    return "RaisedFloor";
                case "Slab on Grade":
                    return "SlabOnGrade";
                //case "Glazing":
                //case "Door":
                //case "Frame":
                //case "Curtain Wall":
                //case "Solar / PV Panel":
                case "Exposed Floor":
                    return "ExposedFloor";
                case "Vehicle Door":
                case "No Type":
                    return "Air";

                //strings from TAS
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
                    return "Air"; //Adiabatic
            }
        }

        /***************************************************/
    }
}
