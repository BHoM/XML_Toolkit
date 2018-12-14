using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.XML;
using BHE = BH.oM.Environment.Elements;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;

using BH.Engine.Environment;

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
            GBXMLOpening.RectangularGeometry.Height = Math.Round(opening.Height(), 3);
            GBXMLOpening.RectangularGeometry.Width = Math.Round(opening.Width(), 3);
            GBXMLOpening.RectangularGeometry.ID = "rGeomOpening" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);

            return GBXMLOpening;
        }

        /***************************************************/

        public static RectangularGeometry ToGBXML(this BHE.Panel bHoMPanel) //TODO: change to PolyCurve. Add query methods in Environment engine for PolyCurves
        {
            RectangularGeometry rectangularGeometry = new RectangularGeometry();
            rectangularGeometry.ID = "rGeom" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);

            BHG.Polyline pline = new BHG.Polyline() { ControlPoints = bHoMPanel.PanelCurve.IControlPoints() };

            rectangularGeometry.Tilt = Math.Round(Environment.Query.Tilt(bHoMPanel), 3);
            rectangularGeometry.Azimuth = Math.Round(Environment.Query.Azimuth(bHoMPanel, BHG.Vector.YAxis), 3);
            rectangularGeometry.Height = Math.Round(bHoMPanel.Height(), 3);
            rectangularGeometry.Width = Math.Round(bHoMPanel.Width(), 3);
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
            geom.Height = Math.Round(buildingElement.Height(), 3);
            geom.Width = Math.Round(buildingElement.Width(), 3);
            geom.CartesianPoint = ToGBXML(pline.ControlPoints.First());
            geom.ID = "Geom" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);

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

        public static Construction ToGBXMLConstruction(this BH.oM.Environment.Elements.BuildingElement buildingElement)
        {
            Construction xmlConstruction = new Construction();

            BHE.Construction construction = buildingElement.BuildingElementProperties.Construction;
            if (construction == null) return xmlConstruction;

            xmlConstruction.ID = "conc-" + construction.BHoM_Guid.ToString().Replace("-", "").Substring(0, 5);
            xmlConstruction.Absorptance = construction.ToGBXML();
            xmlConstruction.Name = construction.Name;
            xmlConstruction.Roughness = construction.Roughness.ToGBXML();
            xmlConstruction.UValue.Value = buildingElement.UValue().ToString();

            return xmlConstruction;
        }

        /***************************************************/

        public static Roughness ToGBXML(this BHE.ConstructionRoughness roughness)
        {
            Roughness r = new Roughness();

            switch(roughness)
            {
                case BHE.ConstructionRoughness.MediumRough:
                    r.Value = "MediumRough";
                    break;
                case oM.Environment.Elements.ConstructionRoughness.MediumSmooth:
                    r.Value = "MediumSmooth";
                    break;
                case oM.Environment.Elements.ConstructionRoughness.Rough:
                    r.Value = "Rough";
                    break;
                case oM.Environment.Elements.ConstructionRoughness.Smooth:
                    r.Value = "Smooth";
                    break;
                case oM.Environment.Elements.ConstructionRoughness.VeryRough:
                    r.Value = "VeryRough";
                    break;
                case oM.Environment.Elements.ConstructionRoughness.VerySmooth:
                    r.Value = "VerySmooth";
                    break;
            }

            return r;
        }

        public static Absorptance ToGBXML(this BHE.Construction construction)
        {
            Absorptance a = new Absorptance();
            a.Unit = construction.AbsorptanceUnit.ToGBXML();
            a.Type = construction.AbsorptanceType.ToGBXML();
            return a;
        }

        public static string ToGBXML(this BHE.AbsorptanceUnit aUnit)
        {
            switch(aUnit)
            {
                case BHE.AbsorptanceUnit.Fraction:
                    return "Fraction";
                case BHE.AbsorptanceUnit.Percent:
                    return "Percent";
                default: return "Fraction";
            }
        }

        public static string ToGBXML(this BHE.AbsorptanceType aType)
        {
            switch(aType)
            {
                case BHE.AbsorptanceType.ExtIR:
                    return "ExtIR";
                case BHE.AbsorptanceType.ExtSolar:
                    return "ExtSolar";
                case BHE.AbsorptanceType.ExtTotal:
                    return "ExtTotal";
                case BHE.AbsorptanceType.ExtVisible:
                    return "ExtVisible";
                case BHE.AbsorptanceType.IntIR:
                    return "IntIR";
                case BHE.AbsorptanceType.IntSolar:
                    return "IntSolar";
                case BHE.AbsorptanceType.IntTotal:
                    return "IntTotal";
                case BHE.AbsorptanceType.IntVisible:
                    return "IntVisible";
                default:
                    return "ExtIR";
            }
        }

        public static Material ToGBXML(this BH.oM.Environment.Materials.Material material)
        {
            Material m = new Material();
            if (material == null || material.MaterialProperties == null) return m;

            m.ID = "material-" + material.BHoM_Guid.ToString().Replace("-", "").Substring(0, 5);
            m.Name = material.Name;
            m.RValue = (material.Thickness / material.MaterialProperties.Conductivity);
            m.Thickness = material.Thickness;
            m.Conductivity = material.MaterialProperties.Conductivity;
            m.Density = material.MaterialProperties.Density;
            m.SpecificHeat = material.MaterialProperties.SpecificHeat;

            return m;
        }

        public static Layer ToGBXML(this List<BH.oM.XML.Material> materials)
        {
            Layer l = new Layer();

            l.ID = "layer-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);

            List<MaterialId> materialIDs = new List<MaterialId>();
            foreach(Material m in materials)
            {
                MaterialId id = new MaterialId();
                id.MaterialIDRef = m.ID;
                materialIDs.Add(id);
            }

            l.MaterialID = materialIDs.ToArray();

            return l;
        }
    }
}
