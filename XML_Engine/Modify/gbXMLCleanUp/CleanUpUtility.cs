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
                        else if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Raised Floor", StringComparison.CurrentCultureIgnoreCase))
                            be.BuildingElementProperties.CustomData[dictionaryKey] = "Internal Floor";
                        else if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Curtain Wall", StringComparison.CurrentCultureIgnoreCase))
                            be.BuildingElementProperties.CustomData[dictionaryKey] = "Internal Wall";
                    }
                }
            }

            return be;
        }
    }
}
