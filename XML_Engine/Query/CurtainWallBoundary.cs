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

        public static List<BHG.PolyCurve> ClosedShellGeometry(this List<BHE.BuildingElement> bHoMBuildingElements)
        {

            List<BHG.PolyCurve> pCrv = new List<BHG.PolyCurve>();
            List<BHG.Polyline> pLinesCurtainWall = new List<BHG.Polyline>();
            List<BHG.Polyline> pLinesOther = new List<BHG.Polyline>();

            //Merge curtain panels
            foreach (BHE.BuildingElement element in bHoMBuildingElements)
            {
                BH.oM.Environmental.Properties.BuildingElementProperties beProperty = element.BuildingElementProperties;
                List<BHG.Point> ctrlPts = element.BuildingElementGeometry.ICurve().IControlPoints();
                BHG.Polyline pline = new BHG.Polyline() { ControlPoints = ctrlPts };

                if (beProperty != null && beProperty.CustomData.ContainsKey("Family Name") && (beProperty.CustomData["Family Name"].ToString() == "Curtain Wall"))
                    pLinesCurtainWall.Add(pline);
                else
                    pLinesOther.Add(pline);
            }

            List<BHG.Polyline> mergedPlines = BH.Engine.Geometry.Compute.BooleanUnion(pLinesCurtainWall);

            //Add the rest of the geometries
            mergedPlines.AddRange(pLinesOther);

            pCrv = mergedPlines.Select(x => new BHG.PolyCurve() { Curves = new List<BHG.ICurve> { x as BHG.ICurve } }).ToList();

            return pCrv;

            /***************************************************/
        }
    }
}




