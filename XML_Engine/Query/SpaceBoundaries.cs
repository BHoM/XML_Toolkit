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

        public static SpaceBoundary[] SpaceBoundaries(this BHE.Space bHoMSpace)
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

                //spaceBound[i].surfaceIdRef = bHoMSpace.BuildingElements[i].BHoM_Guid.ToString();
                //TODO: create surface and get its ID
            }

            return spaceBound;

            /***************************************************/
        }
    }
}




