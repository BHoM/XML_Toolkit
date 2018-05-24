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

        public static BHG.Polyline StoreyGeometry(this BH.oM.Architecture.Elements.Level bHoMLevel, List<BHE.Space> bHoMSpaces)
        {
            List<BHE.Space> spacesAtLevel = bHoMSpaces.FindAll(x => x.Level.Elevation == bHoMLevel.Elevation).ToList();

            if (spacesAtLevel.Count == 0)
                return null;

            List<BHE.BuildingElement> bHoMBuildingElement = spacesAtLevel.SelectMany(x => x.BuildingElements).ToList();
            List<BHG.Point> ctrlPoints = new List<BHG.Point>();

            foreach (BHE.BuildingElement element in bHoMBuildingElement)
            {
                foreach (BHG.Point pt in element.BuildingElementGeometry.ICurve().IControlPoints())
                {
                    if (pt.Z > bHoMLevel.Elevation - BH.oM.Geometry.Tolerance.Distance && pt.Z < bHoMLevel.Elevation + BH.oM.Geometry.Tolerance.Distance)
                        ctrlPoints.Add(pt);

                }
            }

            BHG.Polyline boundary = convexHull(ctrlPoints.CullDuplicates());

            return boundary;
        }

        /***************************************************/

    }
}




