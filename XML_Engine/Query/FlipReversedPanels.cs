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

        public static BHE.Panel FlipReversedPanels(this BHE.Panel panel, BHE.Space space)
        {
            /* Ensure that all of the surface coordinates are listed in a counterclockwise order. This is a requirement of GBXML Polyloop definitions */

            BHG.Polyline pline = new BHG.Polyline() { ControlPoints = panel.PanelCurve.IControlPoints() };
            BHG.Polyline bound = new BHG.Polyline();

            if (!BH.Engine.Geometry.Query.IsClockwise(pline, space.Centre()))
                bound = pline.Flip();
            else
                bound = pline;
            

            BHE.Panel pan = BH.Engine.Environment.Create.Panel(bound);
            return pan;

            /***************************************************/
        }
    }
}




