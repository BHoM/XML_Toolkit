using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XML_Adapter.gbXML;
using BH.oM.Base;
using BHE = BH.oM.Environmental.Elements;
using BHG = BH.oM.Geometry;
using System.Xml.Serialization;
using BH.Engine.Geometry;
using BH.Engine.Environment;
using BH.Engine.XML;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods - Geometry                 ****/
        /***************************************************/

        public static CartesianPoint ToGbXML(BHG.Point pt)
        {
            CartesianPoint cartpoint = new CartesianPoint();
            List<string> coord = new List<string>();

            coord.Add(Math.Round(pt.X, 6).ToString());
            coord.Add(Math.Round(pt.Y, 6).ToString());
            coord.Add(Math.Round(pt.Z, 6).ToString());

            cartpoint.Coordinate = coord.ToArray();

            return cartpoint;
        }

        /***************************************************/

        public static Polyloop ToGbXML(List<BHG.Point> pts)
        {
            int count = pts.First().SquareDistance(pts.Last()) < BH.oM.Geometry.Tolerance.SqrtDist ? pts.Count - 1 : pts.Count;
            Polyloop ploop = new Polyloop();
            List<CartesianPoint> cartpoint = new List<CartesianPoint>();
            for (int i = 0; i < count; i++)
            {
                CartesianPoint cpt = ToGbXML(pts[i]);
                List<string> coord = new List<string>();
                cartpoint.Add(cpt);
            }
            ploop.CartesianPoint = cartpoint.ToArray();
            return ploop;
        }

        /***************************************************/



        /***************************************************/
    }
}
