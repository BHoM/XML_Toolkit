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

        public static BHE.BuildingElementPanel FlipReversedPanels(this BHE.BuildingElementPanel panel, BHE.Space space)
        {
            /* Ensure that all of the surface coordinates are listed in a counterclockwise order. This is a requirement of gbXML Polyloop definitions */

            //RectangularGeometry xmlRectangularGeom = BH.Engine.XML.Convert.ToGbXML(panel);
            //PlanarGeometry plGeo = new PlanarGeometry();
           

            BHG.Polyline pline = new BHG.Polyline() { ControlPoints = panel.PolyCurve.ControlPoints() };
            BHG.Polyline bound = new BHG.Polyline();

            if (!BH.Engine.Geometry.Query.IsClockwise(pline, space.Centre()))
                bound = pline.Flip();
            else
                bound = pline;
            

            BHE.BuildingElementPanel pan = BH.Engine.Environment.Create.BuildingElementPanel(bound);
            return pan;

            /***************************************************/
        }
    }
}




