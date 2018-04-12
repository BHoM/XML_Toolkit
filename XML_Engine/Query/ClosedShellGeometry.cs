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

        public static List<BH.oM.XML.Polyloop> ClosedShellGeometry(this BHE.Space bHoMSpace)
        {

            List<BH.oM.XML.Polyloop> ploopsShell = new List<Polyloop>();
            List<BHG.PolyCurve> shellBound = new List<BHG.PolyCurve>();
            List<BHG.Polyline> pLinesCurtainWall = new List<BHG.Polyline>();
            List<BHG.Polyline> pLinesOther = new List<BHG.Polyline>();

            //1. Merge curtain panels
            foreach (BHE.BuildingElement element in bHoMSpace.BuildingElements)
            {
                BH.oM.Environmental.Properties.BuildingElementProperties beProperty = element.BuildingElementProperties;
                List<BHG.Point> ctrlPts = element.BuildingElementGeometry.ICurve().IControlPoints();
                BHG.Polyline pline = new BHG.Polyline() { ControlPoints = ctrlPts };

                if (beProperty != null && beProperty.CustomData.ContainsKey("Family Name") && (beProperty.CustomData["Family Name"].ToString() == "Curtain Wall"))
                    pLinesCurtainWall.Add(pline);
                else
                    pLinesOther.Add(pline);
            }

            List<BHG.Polyline> mergedPlines = Compute.BooleanUnion(pLinesCurtainWall);

            //2. Add the rest of the geometries
            mergedPlines.AddRange(pLinesOther);

            // 3. Ensure that all of the surface coordinates are listed in a counterclockwise order.
            // This is a requirement of gbXML Polyloop definitions. If this is inconsistent or wrong we end up with a corrupt gbXML file.
            foreach (BHG.Polyline pline in mergedPlines)
            {
                if (!BH.Engine.Geometry.Query.IsClockwise(pline, bHoMSpace.Centre()))
                    ploopsShell.Add(BH.Engine.XML.Convert.ToGbXML(pline.Flip()));
                else
                    ploopsShell.Add(BH.Engine.XML.Convert.ToGbXML(pline));
            }

            return ploopsShell;

            /***************************************************/
        }
    }
}




