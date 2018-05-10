using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.XML;
using BH.oM.Base;
using BHE = BH.oM.Environmental.Elements;
using BHP = BH.oM.Environmental.Properties;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.Engine.Environment;


namespace BH.Engine.XML
{

    public static partial class Modify
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static BHE.Building AssignAdjacentSpace(this BHE.Building building, List<BHE.BuildingElement> buildingElements)
        {
            //Find the adjacent spaces for building elements that only have one
            //Building aBuilding = building.GetShallowClone

            //Get all space building elements
            List<BHE.BuildingElement> spaceBEs = new List<BHE.BuildingElement>();
            foreach (BHE.Space sp in building.Spaces)
                spaceBEs.AddRange(sp.BuildingElements);

            foreach (BHE.BuildingElement be in buildingElements)
            {
                var centerPt = be.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).Centre();

                List<BHE.BuildingElement> foundElements = spaceBEs.Where(x => x.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).Centre().Distance(centerPt) < 0.01).ToList();

                foreach (BHE.BuildingElement be2 in foundElements)
                {
                    if (be2.BHoM_Guid != be.BHoM_Guid)
                    {
                        //Add the adjacent spaces of this be2 to be
                        foreach (Guid g in be2.AdjacentSpaces)
                        {
                            if (!be.AdjacentSpaces.Contains(g))
                                be.AdjacentSpaces.Add(g);
                        }
                    }
                }
            }

            return building;
        }

        /***************************************************/

        //public static BHE.BuildingElement AssignAdjacentSpace(this BHE.Building building, BHE.BuildingElement be)
        //{
        //    //Get all space building elements
        //    List<BHE.BuildingElement> spaceBEs = new List<BHE.BuildingElement>();
        //    foreach (BHE.Space sp in building.Spaces)
        //        spaceBEs.AddRange(sp.BuildingElements);


        //    var centerPt = be.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).Centre();
        //        List<BHG.Point> controlPts = be.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).DiscontinuityPoints();

        //        List<BHE.BuildingElement> foundElements = spaceBEs.Where(x => x.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).Centre().Distance(centerPt) < 0.01).ToList();

        //        foreach (BHE.BuildingElement be2 in foundElements)
        //        {
        //            if (be2.BHoM_Guid != be.BHoM_Guid)
        //            {
        //                var cpt2 = be2.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).Centre();
        //                var distance = centerPt.Distance(cpt2);

        //                var dPt = be2.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).DiscontinuityPoints();

        //                //Add the adjacent spaces of this be2 to be
        //                foreach (Guid g in be2.AdjacentSpaces)
        //                {
        //                    if (!be.AdjacentSpaces.Contains(g))
        //                        be.AdjacentSpaces.Add(g);
        //                }
        //            }
        //        }

        //    return be;
        //}

        /***************************************************/
    }
}




