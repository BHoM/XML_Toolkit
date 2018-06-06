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

        public static List<BHE.BuildingElement> IdentifyOverlaps(this BHE.BuildingElement be, List<BHE.BuildingElement> bes)
        {
            List<BHE.BuildingElement> rtn = new List<BHE.BuildingElement>();

            BHG.Polyline be1P = be.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);

            foreach (BHE.BuildingElement be2 in bes)
            {
                if (be2.BHoM_Guid != be.BHoM_Guid)
                {
                    BHG.Polyline be2P = be2.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);
                    if (be1P.IsCoplanar(be2P))
                    {
                        List<BHG.Polyline> intersections = be1P.BooleanIntersection(be2P);
                        if (intersections.Count > 0)
                            rtn.Add(be2);
                    }
                }
            }

            return rtn;
        }

        /***************************************************/

        public static List<BHG.Polyline> CullDuplicates(this List<BHG.Polyline> plines, double tolerance = 0.01)
        {
            List<BHG.Polyline> unique = new List<oM.Geometry.Polyline>();
            for (int i = 0; i < plines.Count; i++)
            {
                List<BHG.Polyline> compareItems = plines;
                compareItems.RemoveAt(i);

                for (int j = 0; j < compareItems.Count; j++)
                {
                    if (plines[i].Centre().Distance(compareItems[j].Centre()) > tolerance)
                        continue;

                    if (plines[i].ControlPoints.Count != compareItems[j].ControlPoints.Count)
                        continue;

                    // remove if they have the same control points
                    foreach (BHG.Point p in plines[i].ControlPoints)
                    {
                        if (compareItems[j].ControlPoints.Contains(p))
                            continue;
                        {
                            plines.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            return plines;
        }

        /***************************************************/

    }
}




