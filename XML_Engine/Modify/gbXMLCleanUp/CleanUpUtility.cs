using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHE = BH.oM.Environmental.Elements;
using BH.Engine.Geometry;
using BH.Engine.Environment;

namespace XML_Engine.Modify.gbXMLCleanUp
{
    public static class CleanUpUtility
    {
        public static List<BHE.BuildingElement> GetBuildingElements(this BHE.Building building)
        {
            List<BHE.BuildingElement> rtn = new List<BHE.BuildingElement>();

            rtn.AddRange(building.BuildingElements.FindAll(x => x.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Window && x.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Door)); //Add only shade elements
        
           

            foreach(BHE.Space s in building.Spaces)
            {
                foreach(BHE.BuildingElement be in s.BuildingElements)
                {
                    if (rtn.Where(x => x.BHoM_Guid == be.BHoM_Guid).FirstOrDefault() == null && be.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Window && be.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Door) //Add only shade elements
                        rtn.Add(be);
                }
            }

            return rtn;
        }

        public static BHE.BuildingElement AmendSingleAdjacencies(this BHE.BuildingElement be, BHE.Building building)
        {
            if (be.AdjacentSpaces.Count == 1)
            {
                List<BHE.BuildingElement> foundElements = building.BuildingElements.Where(x => x.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).Centre().Distance(be.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).Centre()) < 0.01).ToList();

                foreach (BHE.BuildingElement be2 in foundElements)
                {
                    if (be2.BHoM_Guid != be.BHoM_Guid)
                    {
                        foreach (Guid g in be2.AdjacentSpaces)
                            if (!be.AdjacentSpaces.Contains(g))
                                be.AdjacentSpaces.Add(g);
                    }
                }
            }

            return be;
        }

        public static BHE.BuildingElement AmendXMLType(this BHE.BuildingElement be)
        {
            String dictionaryKey = "SAM_BuildingElementType";

            if (be.BuildingElementProperties == null)
                be.BuildingElementProperties = new BH.oM.Environmental.Properties.BuildingElementProperties();

            if(be.BuildingElementProperties.BuildingElementType == BHE.BuildingElementType.Window || be.BuildingElementProperties.BuildingElementType == BHE.BuildingElementType.Door)
                return be;

            if (!be.BuildingElementProperties.CustomData.ContainsKey(dictionaryKey))
                be.BuildingElementProperties.CustomData.Add(dictionaryKey, "");

            if (be.AdjacentSpaces.Count == 0)
                be.BuildingElementProperties.CustomData[dictionaryKey] = "Shade";
            else
            {
                double tilt = BH.Engine.Environment.Query.Tilt(be.BuildingElementGeometry);

                if (be.AdjacentSpaces.Count == 1)
                {
                    //Elements with 1 adjacent space are either exterior walls or floors
                    if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("", StringComparison.CurrentCultureIgnoreCase))
                    {
                        //New building element type - calculate wall or floor
                        if (tilt >= 70 && tilt <= 120)
                            be.BuildingElementProperties.CustomData[dictionaryKey] = "External Wall";
                        else
                            be.BuildingElementProperties.CustomData[dictionaryKey] = "Exposed Floor";
                    }
                    else if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Internal Wall", StringComparison.CurrentCultureIgnoreCase) || (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Air", StringComparison.CurrentCultureIgnoreCase) && be.BuildingElementProperties.BuildingElementType == BHE.BuildingElementType.Wall)) //Incorrectly categorised as internal wall or air - should be external wall
                        be.BuildingElementProperties.CustomData[dictionaryKey] = "External Wall";
                    else if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Internal Floor", StringComparison.CurrentCultureIgnoreCase) || (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Air", StringComparison.CurrentCultureIgnoreCase) && be.BuildingElementProperties.BuildingElementType == BHE.BuildingElementType.Floor) )//Incorrectly categorised as internal floor or air - should be external (exposed) floor
                        be.BuildingElementProperties.CustomData[dictionaryKey] = "Exposed Floor";
                }
                else if (be.AdjacentSpaces.Count == 2)
                {
                    //Elements with 2 adjacent spaces are either internal walls or floors
                    if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (tilt >= 70 && tilt <= 120)
                            be.BuildingElementProperties.CustomData[dictionaryKey] = "Internal Wall";
                        else
                            be.BuildingElementProperties.CustomData[dictionaryKey] = "Internal Floor";
                    }
                    else
                    {
                        if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Contains("External"))
                            be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Replace("External", "Internal");
                        else if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Roof", StringComparison.CurrentCultureIgnoreCase))
                            be.BuildingElementProperties.CustomData[dictionaryKey] = "Ceiling";
                        else if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Raised Floor", StringComparison.CurrentCultureIgnoreCase) || be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Exposed Floor", StringComparison.CurrentCultureIgnoreCase))
                            be.BuildingElementProperties.CustomData[dictionaryKey] = "Internal Floor";
                        else if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Curtain Wall", StringComparison.CurrentCultureIgnoreCase))
                            be.BuildingElementProperties.CustomData[dictionaryKey] = "Internal Wall";
                    }
                }
            }

            return be;
        }

        public static BHE.BuildingElement AmendCadObjectID(this BHE.BuildingElement be)
        {
            String dictionaryKey = "Family Name";
            if (be.BuildingElementProperties == null)
                be.BuildingElementProperties = new BH.oM.Environmental.Properties.BuildingElementProperties();

            if (be.BuildingElementProperties.Name.Equals("", StringComparison.CurrentCultureIgnoreCase))
                be.BuildingElementProperties.Name = "SIM_INT_SLD";

            if (!be.BuildingElementProperties.CustomData.ContainsKey(dictionaryKey))
                be.BuildingElementProperties.CustomData.Add(dictionaryKey, "");

            //Missing family name
            if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("", StringComparison.CurrentCultureIgnoreCase))
            {
                double tilt = BH.Engine.Environment.Query.Tilt(be.BuildingElementGeometry);
                if (tilt >= 70 && tilt <= 120)
                    be.BuildingElementProperties.CustomData[dictionaryKey] = "Basic Wall";
                else if (tilt == 0)
                    be.BuildingElementProperties.CustomData[dictionaryKey] = "Floor";
                else if (tilt == 180)
                    be.BuildingElementProperties.CustomData[dictionaryKey] = "Basic Roof";

                if (!be.BuildingElementProperties.CustomData.ContainsKey("Revit_elementId"))
                    be.BuildingElementProperties.CustomData.Add("Revit_elementId", "");

                be.BuildingElementProperties.CustomData["Revit_elementId"] = "CADObjectID";
            }

            //Wrong CADObjectID
            if (be.BuildingElementProperties.Name.Contains("EXT") && be.AdjacentSpaces.Count != 1)
                be.BuildingElementProperties.Name.Replace("EXT", "INT");
            else if (be.BuildingElementProperties.Name.Contains("INT") && be.AdjacentSpaces.Count != 2)
                be.BuildingElementProperties.Name.Replace("INT", "EXT");

            return be;
        }
    }
}
