using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.XML;
using BHE = BH.oM.Environmental.Elements;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods - Geometry                 ****/
        /***************************************************/

        public static CartesianPoint ToGbXML(this BHG.Point pt)
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

        public static Polyloop ToGbXML(this BHG.Polyline polyLine)
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

        //public static Surface ToGbXML(BHE.BuildingElementPanel bHoMPanel)
        //{
        //    Surface xmlPanel = new Surface();

        //    xmlPanel.Name = bHoMPanel.Name;
        //    xmlPanel.surfaceType = ToGbXMLSurfaceType(bHoMPanel);

        //    return xmlPanel;
        //}

        /***************************************************/

        public static Opening ToGbXML(this BHE.BuildingElementOpening opening)
        {
            Opening gbXMLOpening = new Opening();

            //gbXMLOpening.id = opening.BHoM_Guid.ToString();
            //gbXMLOpening.Name = opening.Name;
            // TODO: Add openings types


            BHG.Polyline pline = new BHG.Polyline() { ControlPoints = opening.PolyCurve.ControlPoints() };

            gbXMLOpening.PlanarGeometry.PolyLoop = ToGbXML(pline);
            gbXMLOpening.RectangularGeometry.CartesianPoint = Geometry.Query.Centre(pline).ToGbXML();
            gbXMLOpening.RectangularGeometry.Width = Math.Round(Query.Width(pline), 3);
            gbXMLOpening.RectangularGeometry.Height = Math.Round(Query.Length(pline), 3);

            return gbXMLOpening;
        }

        /***************************************************/

        public static RectangularGeometry ToGbXML(this BHE.BuildingElementPanel bHoMPanel) //TODO: change to PolyCurve. Add query methods in Environment engine for PolyCurves
        {
            RectangularGeometry rectangularGeometry = new RectangularGeometry();

            BHG.Polyline pline = new BHG.Polyline() { ControlPoints = bHoMPanel.PolyCurve.ControlPoints() };

            rectangularGeometry.Tilt = Math.Round(Environment.Query.Inclination(bHoMPanel), 3);
            rectangularGeometry.Azimuth = Math.Round(Environment.Query.Orientation(bHoMPanel), 3);
            rectangularGeometry.Height = Math.Round(Query.Length(pline), 3);
            rectangularGeometry.Width = Math.Round(Query.Width(pline), 3);
            rectangularGeometry.CartesianPoint = Geometry.Query.Centre(pline).ToGbXML();
            rectangularGeometry.Polyloop = pline.ToGbXML();

            return rectangularGeometry;
        }

        /***************************************************/

        public static Space ToGbXML(this BHE.Space bHoMSpace)
        {
            Space xmlSpace = new Space();

            xmlSpace.Name = bHoMSpace.Name + " Space";
            xmlSpace.id = "Space-" + bHoMSpace.Name.ToString();

            //xmlSpace.Area = Environment.Query.FloorArea(bHoMSpace);
            if (bHoMSpace.CustomData.ContainsKey("Area"))
                xmlSpace.Area = Math.Round((double)bHoMSpace.CustomData["Area"], 3);
            //xmlSpace.Volume = Environment.Query.Volume(bHoMSpace);
            if (bHoMSpace.CustomData.ContainsKey("Volume"))
                xmlSpace.Volume = Math.Round((double)bHoMSpace.CustomData["Volume"], 3);
            if (bHoMSpace.CustomData.ContainsKey("Volume"))
                xmlSpace.CADobjectId = (bHoMSpace.CustomData["Revit_elementId"]).ToString();


            if (bHoMSpace.Level != null)
                xmlSpace.buildingStoreyIdRef = bHoMSpace.Level.Name;

            return xmlSpace;
        }

        /***************************************************/

        public static BuildingStorey ToGbXML(this BH.oM.Architecture.Elements.Level bHoMLevel)
        {
            BuildingStorey xmlStorey = new BuildingStorey();

            xmlStorey.Name = bHoMLevel.Name;
            xmlStorey.id = bHoMLevel.Name;
            xmlStorey.Level = (float)bHoMLevel.Elevation;

            return xmlStorey;
        }

        /***************************************************/

        public static Building ToGbXML(this BHE.Building bHoMBuilding)
        {
            Building xmlBuilding = new Building();

            xmlBuilding.Name = bHoMBuilding.Name;
            xmlBuilding.id = bHoMBuilding.BHoM_Guid.ToString();

            List<double> area = new List<double>();
            foreach (BHE.Space space in bHoMBuilding.Spaces)
            {
                area.Add(BH.Engine.Environment.Query.FloorArea(space));
                xmlBuilding.Area = (float)area.Sum();
            }
            foreach (BH.oM.Architecture.Elements.Level level in bHoMBuilding.Levels)
            {
                xmlBuilding.BuildingStorey.ToList().Add(level.ToGbXML());
            }
            return xmlBuilding;
        }

        /***************************************************/
    }
}
