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
        public static BHE.Building AmendSingleAdjacencies(this BHE.Building building)
        {
            foreach (BHE.BuildingElement be in building.BuildingElements)
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
                                    building.BuildingElements.Where(x => x.BHoM_Guid == be.BHoM_Guid).First().AdjacentSpaces.Add(g);
                        }
                    }
                }
            }

            return building;
        }
    }
}
