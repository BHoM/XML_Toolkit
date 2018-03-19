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

        public static BHG.Point ToBHoM(CartesianPoint cartesianPoint)
        {
            BHG.Point pt = new BHG.Point();
            pt.X = System.Convert.ToDouble(cartesianPoint.Coordinate[0]);
            pt.Y = System.Convert.ToDouble(cartesianPoint.Coordinate[1]);
            pt.Z = System.Convert.ToDouble(cartesianPoint.Coordinate[2]);

            return pt;
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
