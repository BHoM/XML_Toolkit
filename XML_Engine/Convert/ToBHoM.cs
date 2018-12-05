using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.XML;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods - Geometry                 ****/
        /***************************************************/

        public static BHG.Point ToBHoM(CartesianPoint cartesianPoint)
        {
            List<double> coords = new List<double>();
            foreach (String s in cartesianPoint.Coordinate)
                coords.Add(System.Convert.ToDouble(s));

            for (int x = coords.Count; x < 3; x++)
                coords.Add(0); //Add additional elements in case the cartesian point had less than 3 points

            return BH.Engine.Geometry.Create.Point(coords[0], coords[1], coords[2]);
        }

        /***************************************************/

        public static BHG.Polyline ToBHoM(Polyloop ploop)
        {
            List<BHG.Point> pts = new List<BH.oM.Geometry.Point>();
            if (1 <= ploop.CartesianPoint.Count())
            {
                foreach (CartesianPoint Cpt in ploop.CartesianPoint)
                {
                    pts.Add(ToBHoM(Cpt));
                }
                pts.Add((BHG.Point)pts[0].Clone());
            }
            BHG.Polyline pline = BH.Engine.Geometry.Create.Polyline(pts);
            return pline;
        }



        /***************************************************/
    }
}
