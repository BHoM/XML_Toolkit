using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHE = BH.oM.Environment.Elements;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        /***************************************************/

        public static string ToGBXMLType(this BHE.BuildingElement bHoMBuildingElement, List<BHE.Space> adjacentSpaces = null, bool isIES = false)
        {
            if (adjacentSpaces == null) adjacentSpaces = new List<oM.Environment.Elements.Space>();

            string type = "Air";
            if (bHoMBuildingElement == null)
                return type;
            else if (adjacentSpaces.Count == 0 && bHoMBuildingElement.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Window && bHoMBuildingElement.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Door)
                type = "Shade";
            else if (bHoMBuildingElement.BuildingElementProperties != null)
            {
                if (bHoMBuildingElement.BuildingElementProperties.CustomData.ContainsKey("SAM_BuildingElementType"))
                {
                    object aObject = bHoMBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"];

                    if (aObject != null)
                        type = ToGBXMLSurfaceType(aObject.ToString()); //modifies the string

                    if ((isIES && type.Contains("Window") || bHoMBuildingElement.BuildingElementProperties.BuildingElementType == BHE.BuildingElementType.Window) && bHoMBuildingElement.BuildingElementProperties.Name.Contains("SLD")) //Change windows with SLD construction into doors for IES
                        type = "NonSlidingDoor";
                }
                else
                {
                    //THIS IS A HACKY FIX FOR COMPLIANCE COMPLETION PURPOSES AND WILL NEED RE-EXAMINING
                    if (adjacentSpaces.Count == 1)
                        type = "ExteriorWall";
                    else if (adjacentSpaces.Count == 2)
                        type = "InteriorWall";
                }
            }
            else if (bHoMBuildingElement != null)
            {
                //type = ToGBXMLSurfaceType((bHoMBuildingElement.BuildingElementGeometry as BHE.Panel).ElementType);
                type = "ExteriorWall"; //TODO: Fix building element types to include all of the other types we could have/the element type like we had with the panel...
            }
            else
                type = "Air";

            return type;
        }

        /***************************************************/
        //String modification for surface types
        public static string ToGBXMLSurfaceType(this string type)
        {
            switch (type)
            {
                //Strings from Revit
                case "Rooflight":
                    return "OperableSkylight";
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
                //case "Solar / PV Panel":
                case "Curtain Wall":
                    return "ExteriorWall";
                case "Exposed Floor":
                    //    return "ExposedFloor";
                    return "RaisedFloor";
                //case "Vehicle Door":
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

                //Openings
                case "Glazing":
                    return "FixedWindow";
                case "Door":
                    return "NonSlidingDoor";

                default:
                    return "Air"; //Adiabatic
            }
        }

        /***************************************************/
    }
}
