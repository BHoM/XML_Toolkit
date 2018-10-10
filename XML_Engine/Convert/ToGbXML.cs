using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.XML;
using BHE = BH.oM.Environment.Elements;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods - Geometry                 ****/
        /***************************************************/

        public static CartesianPoint ToGBXML(this BHG.Point pt)
        {
            CartesianPoint cartPoint = new CartesianPoint();
            List<string> coord = new List<string>();

            coord.Add(Math.Round(pt.X, 10).ToString());
            coord.Add(Math.Round(pt.Y, 10).ToString());
            coord.Add(Math.Round(pt.Z, 10).ToString());

            cartPoint.Coordinate = coord.ToArray();

            return cartPoint;
        }
        /***************************************************/

        public static Polyloop ToGBXML(this BHG.Polyline polyLine, double tolerance = BHG.Tolerance.Distance)
        {
            List<BHG.Point> pts = polyLine.DiscontinuityPoints();

            int count = pts.First().SquareDistance(pts.Last()) < tolerance * tolerance ? pts.Count - 1 : pts.Count;
            Polyloop ploop = new Polyloop();
            List<CartesianPoint> cartpoint = new List<CartesianPoint>();
            for (int i = 0; i < count; i++)
            {
                CartesianPoint cpt = ToGBXML(pts[i]);
                List<string> coord = new List<string>();
                cartpoint.Add(cpt);
            }
            ploop.CartesianPoint = cartpoint.ToArray();
            return ploop;
        }

        /***************************************************/

        //public static Surface ToGBXML(BHE.Panel bHoMPanel)
        //{
        //    Surface xmlPanel = new Surface();

        //    xmlPanel.Name = bHoMPanel.Name;
        //    xmlPanel.surfaceType = ToGBXMLSurfaceType(bHoMPanel);

        //    return xmlPanel;
        //}

        /***************************************************/

        public static Opening ToGBXML(this BHE.Opening opening)
        {
            Opening GBXMLOpening = new Opening();

            //GBXMLOpening.id = opening.BHoM_Guid.ToString();
            //GBXMLOpening.Name = opening.Name;

            BHG.Polyline pline = new BHG.Polyline() { ControlPoints = opening.OpeningCurve.IControlPoints() };

            GBXMLOpening.PlanarGeometry.PolyLoop = ToGBXML(pline);
            GBXMLOpening.RectangularGeometry.CartesianPoint = Geometry.Query.Centre(pline).ToGBXML();
            GBXMLOpening.RectangularGeometry.Height = Math.Round(BH.Engine.Environment.Query.LongestSegment(pline), 3);
            GBXMLOpening.RectangularGeometry.Width = Math.Round(BH.Engine.Environment.Query.Width(pline, GBXMLOpening.RectangularGeometry.Height), 3);

            return GBXMLOpening;
        }

        /***************************************************/

        public static RectangularGeometry ToGBXML(this BHE.Panel bHoMPanel) //TODO: change to PolyCurve. Add query methods in Environment engine for PolyCurves
        {
            RectangularGeometry rectangularGeometry = new RectangularGeometry();

            BHG.Polyline pline = new BHG.Polyline() { ControlPoints = bHoMPanel.PanelCurve.IControlPoints() };

            rectangularGeometry.Tilt = Math.Round(Environment.Query.Tilt(bHoMPanel), 3);
            rectangularGeometry.Azimuth = Math.Round(Environment.Query.Azimuth(bHoMPanel, BHG.Vector.YAxis), 3);
            rectangularGeometry.Height = Math.Round(BH.Engine.Environment.Query.LongestSegment(pline), 3);
            rectangularGeometry.Width = Math.Round(BH.Engine.Environment.Query.Width(pline, rectangularGeometry.Height), 3);
            rectangularGeometry.CartesianPoint = ToGBXML(pline.ControlPoints.First());

            return rectangularGeometry;
        }

        /***************************************************/

        public static RectangularGeometry ToGBXML(this BHE.BuildingElement buildingElement)
        {
            RectangularGeometry geom = new RectangularGeometry();

            BHG.Polyline pline = new BHG.Polyline() { ControlPoints = buildingElement.PanelCurve.IControlPoints() };

            geom.Tilt = Math.Round(Environment.Query.Tilt(buildingElement), 3);
            geom.Azimuth = Math.Round(Environment.Query.Azimuth(buildingElement, BHG.Vector.YAxis), 3);
            geom.Height = Math.Round(BH.Engine.Environment.Query.LongestSegment(pline), 3);
            geom.Width = Math.Round(BH.Engine.Environment.Query.Width(pline, geom.Height), 3);
            geom.CartesianPoint = ToGBXML(pline.ControlPoints.First());

            return geom;
        }

        /***************************************************/

        public static Space ToGBXML(this BHE.Space bHoMSpace)
        {
            Space xmlSpace = new Space();

            //xmlSpace.Name = bHoMSpace.Name + " Space";
            //xmlSpace.Name = bHoMSpace.Name + " " + bHoMSpace.Number;
            xmlSpace.Name = bHoMSpace.Number + " " + bHoMSpace.Name;
            //xmlSpace.id = "Space-" + bHoMSpace.Name.ToString();
            //xmlSpace.id = "Space-" + bHoMSpace.Number.ToString();
            xmlSpace.ID = bHoMSpace.Number + "-" + bHoMSpace.Name.ToString();

            //xmlSpace.Area = Environment.Query.FloorArea(bHoMSpace);
            if (bHoMSpace.CustomData.ContainsKey("Area"))
                xmlSpace.Area = Math.Round((double)bHoMSpace.CustomData["Area"], 3);
            //xmlSpace.Volume = Environment.Query.Volume(bHoMSpace);
            if (bHoMSpace.CustomData.ContainsKey("Volume"))
                xmlSpace.Volume = Math.Round((double)bHoMSpace.CustomData["Volume"], 3);
            if (bHoMSpace.CustomData.ContainsKey("Revit_elementId"))
                xmlSpace.CADObjectID = (bHoMSpace.CustomData["Revit_elementId"]).ToString();

            //added replacement to spaces to allow compliant with XML format
            /*if (bHoMSpace.Level != null)
                xmlSpace.BuildingStoreyIDRef = bHoMSpace.Level.Name.Replace(" ", "-");
                */
            return xmlSpace;
        }

        /***************************************************/

        public static BuildingStorey ToGBXML(this BH.oM.Architecture.Elements.Level bHoMLevel)
        {
            BuildingStorey xmlStorey = new BuildingStorey();

            xmlStorey.Name = bHoMLevel.Name;
            xmlStorey.ID = bHoMLevel.Name.Replace(" ", "-");
            xmlStorey.Level = (float)bHoMLevel.Elevation;
            

            return xmlStorey;
        }

        /***************************************************/

        //public static Building ToGBXML(this BHE.Building bHoMBuilding)
        //{
        //    Building xmlBuilding = new Building();

        //    xmlBuilding.Name = bHoMBuilding.Name;
        //    xmlBuilding.id = bHoMBuilding.BHoM_Guid.ToString();

        //    List<double> area = new List<double>();
        //    foreach (BHE.Space space in bHoMBuilding.Spaces)
        //    {
        //        area.Add(BH.Engine.Environment.Query.FloorArea(space));
        //        xmlBuilding.Area = (float)area.Sum();
        //    }
        //    foreach (BH.oM.Architecture.Elements.Level level in bHoMBuilding.Levels)
        //    {
        //        xmlBuilding.BuildingStorey.ToList().Add(level.ToGBXML());
        //    }
        //    return xmlBuilding;
        //}

        /***************************************************/

        public static Location ToGBXML(this BHE.Building bHoMBuilding)
        {
            Location xmlLocation = new Location();

            xmlLocation.ZipcodeOrPostalCode = "00000"; // TODO: Change from default value
            xmlLocation.Longitude = Math.Round(bHoMBuilding.Longitude, 5);
            xmlLocation.Latitude = Math.Round(bHoMBuilding.Latitude, 5);
            xmlLocation.Elevation = Math.Round(bHoMBuilding.Elevation, 5);
            xmlLocation.CADModelAzimuth = 0; // TODO: Change from default value

            //From Custom Data
            if (bHoMBuilding.CustomData.ContainsKey("Place Name"))
                xmlLocation.Name = (bHoMBuilding.CustomData["Place Name"]).ToString();
            if (bHoMBuilding.CustomData.ContainsKey("Weather Station Name"))
                xmlLocation.StationID.ID = (bHoMBuilding.CustomData["Weather Station Name"]).ToString();


            return xmlLocation;
        }

        /***************************************************/

        public static Construction ToGBXML(this BH.oM.Environment.Properties.BuildingElementProperties beProp)
        {
            Construction xmlConstruction = new Construction();

            //xmlConstruction.Absorptance = ;
            xmlConstruction.ID = "id";
            xmlConstruction.Name = beProp.Name;
            //xmlConstruction.Roughness = "VeryRough";
            //xmlConstruction.Uvalue = beProp.UValue;


            return xmlConstruction;
        }

        /***************************************************/
    }
}
