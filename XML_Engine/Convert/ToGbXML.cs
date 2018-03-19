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

        public static Polyloop ToGbXML(BHG.Polyline polyLine)
        {
            List<BHG.Point> pts = polyLine.DiscontinuityPoints();

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

        public static Opening ToGbXML(BHE.BuildingElementOpening opening)
        {
            Opening gbXMLOpening = new Opening();

            gbXMLOpening.id = opening.BHoM_Guid.ToString();
            gbXMLOpening.Name = opening.Name;

            BHG.Polyline pline = new BHG.Polyline() { ControlPoints = opening.PolyCurve.ControlPoints() };

            gbXMLOpening.PlanarGeometry.PolyLoop = ToGbXML(pline);
            gbXMLOpening.RectangularGeometry.Polyloop = ToGbXML(pline);

            return gbXMLOpening;
        }

        /***************************************************/

        public static RectangularGeometry ToGbXML(BHE.BuildingElementPanel bHoMPanel)
        {
            RectangularGeometry rectangularGeometry = new RectangularGeometry();

            rectangularGeometry.Tilt = Environment.Query.Inclination(bHoMPanel);
            rectangularGeometry.Azimuth = Environment.Query.Orientation(bHoMPanel);
            rectangularGeometry.Height = Environment.Query.AltitudeRange(bHoMPanel);

            return rectangularGeometry;
        }

        /***************************************************/
      
    }
}
