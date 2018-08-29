using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.XML;
using BH.oM.Base;
using BHE = BH.oM.Environment.Elements;
using BHP = BH.oM.Environment.Properties;
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

        public static SpaceBoundary[] SpaceBoundaries(this BHE.Space bHoMSpace, List<BHE.BuildingElement> be)
        {
            List<BH.oM.XML.Polyloop> ploops = new List<Polyloop>();
            IEnumerable<BHG.PolyCurve> bePanel = bHoMSpace.BuildingElements.Select(x => x.BuildingElementGeometry.ICurve() as BHG.PolyCurve);

            foreach (BHG.PolyCurve pCrv in bePanel)
            {
                /* Ensure that all of the surface coordinates are listed in a counterclockwise order
                * This is a requirement of gbXML Polyloop definitions */
                BHG.Polyline pline = new BHG.Polyline() { ControlPoints = pCrv.ControlPoints() };

                if (!BH.Engine.XML.Query.NormalAwayFromSpace(pline, bHoMSpace))
                    ploops.Add(BH.Engine.XML.Convert.ToGbXML(pline.Flip()));
                else
                    ploops.Add(BH.Engine.XML.Convert.ToGbXML(pline));
            }

            SpaceBoundary[] spaceBound = new SpaceBoundary[ploops.Count()];

            for (int i = 0; i < ploops.Count(); i++)
            {
                PlanarGeometry planarGeom = new PlanarGeometry();
                planarGeom.PolyLoop = ploops[i];
                SpaceBoundary bound = new SpaceBoundary { PlanarGeometry = planarGeom };
                spaceBound[i] = bound;

                //Get the id from the referenced panel
                string refPanel = "Panel-" + be.FindIndex(x => x.BHoM_Guid.ToString() == bHoMSpace.BuildingElements[i].BHoM_Guid.ToString()).ToString();
                spaceBound[i].SurfaceIDRef = refPanel;
            }

            return spaceBound;

            /***************************************************/
        }
    }
}




