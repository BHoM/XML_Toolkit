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

        public static BHG.Line GetLine(this BHG.Polyline pline, BHG.Point pt)
        {
            
            List<BHG.Point> controlPoints = pline.ControlPoints.CullDuplicates();
            BHG.Line line = new BHG.Line();

            BHG.Line segment = new BHG.Line();
            
            for (int i = 0; i < controlPoints.Count()-1; i++)
            {
                segment = BH.Engine.Geometry.Create.Line(controlPoints[i], controlPoints[i + 1]);
                if (XML.Query.IsContaining(segment, pt))
                    line = segment;
            }

            segment = BH.Engine.Geometry.Create.Line(controlPoints.Last(), controlPoints[0]);
            if (XML.Query.IsContaining(segment, pt))
                line = segment;


            if (line == null)
                return null;

            return line; 

            /***************************************************/
        }
    }
}




