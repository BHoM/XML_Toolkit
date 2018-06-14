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

    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static bool IsClosed(BHE.Space space, double tolerance = BHG.Tolerance.MacroDistance)
        {
            //There will be no unique discontinuity points for a closed space. List length of unique points should therefore be 0. There is one known edge case where we have a hole surrounded by surfaces though....            
            return (PointsMissingPals(space, tolerance).Count == 0);
        }
        
        /***************************************************/

        public static List<BHG.Point> PointsMissingPals(BHE.Space space, double tolerance = BHG.Tolerance.MacroDistance)
        {
            List<BHG.Point> nonMatches = new List<BHG.Point>();

            foreach (BHE.BuildingElement be in space.BuildingElements)
            {
                if (be == null) continue;

                foreach (BHG.Point pt in be.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).IDiscontinuityPoints())
                {
                    List<BHE.BuildingElement> beCompare = space.BuildingElements.Where(x => x != null).ToList().FindAll(x => x.BHoM_Guid != be.BHoM_Guid);
                    List<BHG.Point> p = new List<BHG.Point> { pt };

                    BHE.BuildingElement matchingBe = beCompare.Find(x => x.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).DiscontinuityPoints().ClosestDistance(p) < tolerance && (x.BHoM_Guid != be.BHoM_Guid));

                    if (matchingBe == null)
                        nonMatches.Add(pt);
                }
            }

            return nonMatches;
        }
    }
}




