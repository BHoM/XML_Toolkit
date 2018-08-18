using System;
using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using System.Linq;
using BH.oM.Environment.Elements;


namespace BH.Engine.XML
{

    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static double Width(BHG.Polyline pline, double length)
        {
            double area = pline.Area();
            double width = area / length;

            return width;
        }

        /***************************************************/
    }
}




