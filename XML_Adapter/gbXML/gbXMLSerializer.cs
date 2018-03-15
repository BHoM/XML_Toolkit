using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XML = XML_Adapter.gbXML;
using BH.oM.Base;
using BHE = BH.oM.Environmental.Elements;
using BHG = BH.oM.Geometry;
using System.Xml.Serialization;
using BH.Engine.Geometry;
using BH.Engine.Environment;

namespace XML_Adapter.gbXML
{
    public class gbXMLSerializer
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static void Serialize(List<IBHoMObject> bhomObjects, gbXML gbx)
        {

            foreach (IBHoMObject obj in bhomObjects)
            {

                List<BHE.BuildingElementPanel> bHoMPanels = new List<BHE.BuildingElementPanel>();
                List<BHE.BuildingElement> bHoMBuildingElement = new List<BHE.BuildingElement>();

                if (obj.GetType() == typeof(BHE.Space))
                {
                    BHE.Space bHoMSpace = obj as BHE.Space;
                    bHoMPanels.AddRange(bHoMSpace.BuildingElements.Select(x => x.BuildingElementGeometry as BHE.BuildingElementPanel));
                    bHoMBuildingElement.AddRange(bHoMSpace.BuildingElements);
                }

                BHG.Point spaceCentrePoint = BH.Engine.Environment.Query.Centre(obj as BHE.Space);

                List<BHE.Space> spaces = bhomObjects.Where(x => x is BHE.Space).Select(x => x as BHE.Space).ToList();


                // Generate gbXMLSurfaces
                /***************************************************/
                if (bHoMPanels != null)
                {
                    List<Surface> srfs = new List<Surface>();
                    for (int i = 0; i < bHoMPanels.Count; i++)
                    {
                        Surface xmlPanel = new Surface();
                        xmlPanel.Name = bHoMPanels[i].Name;
                        xmlPanel.surfaceType = Convert.ToGbXMLSurfaceType(bHoMPanels[i]);
                        xmlPanel.id = "Panel-" + bHoMPanels[i].BHoM_Guid.ToString();
                        RectangularGeometry xmlRectangularGeom = new RectangularGeometry();
                        xmlRectangularGeom.Tilt = BH.Engine.Environment.Query.IInclination(bHoMPanels[i]);
                        xmlRectangularGeom.Azimuth = BH.Engine.Environment.Query.IOrientation(bHoMPanels[i]);
                        xmlRectangularGeom.Height = BH.Engine.Environment.Query.IAltitudeRange(bHoMPanels[i]);
                       
                        PlanarGeometry plGeo = new PlanarGeometry();
                        plGeo.id = "PlanarGeometry" + i.ToString();

                        /* Ensure that all of the surface coordinates are listed in a counterclockwise order
                         * This is a requirement of gbXML Polyloop definitions */
                        //TODO: Update to a ToPolyline() method once an appropriate version has been implemented

                        BHG.Polyline pline = new BHG.Polyline() { ControlPoints = bHoMPanels[i].PolyCurve.ControlPoints() }; //TODO: Change to ToPolyline method

                        if (BH.Engine.Geometry.Query.IsClockwise(pline, spaceCentrePoint))
                        {
                            var pts = new List<BHG.Point>(pline.DiscontinuityPoints());
                            pts.Reverse();
                            plGeo.PolyLoop = MakePolyloop(pts);
                            xmlRectangularGeom.Polyloop = MakePolyloop(pts); //TODO: for boundingbox
                        }
                        else
                        {
                            plGeo.PolyLoop = MakePolyloop(pline.DiscontinuityPoints());
                            xmlRectangularGeom.Polyloop = MakePolyloop(pline.DiscontinuityPoints()); //TODO: for boundingbox
                        }

                        xmlRectangularGeom.CartesianPoint = ToCartesian(BH.Engine.Geometry.Query.Centre(pline));

                        xmlPanel.PlanarGeometry = plGeo;
                        xmlPanel.RectangularGeometry = xmlRectangularGeom;


                        // Openings
                        /***************************************************/
                        if (bHoMPanels[i].Openings != null)
                        {
                            Opening xmlOpening = new Opening();
                            foreach (BHE.BuildingElementOpening opening in bHoMPanels[i].Openings)
                            {
                                xmlOpening.PlanarGeometry.PolyLoop = MakePolyloop(opening.PolyCurve.ControlPoints());
                            }
                            xmlPanel.Opening = xmlOpening; 
                        }


                        // Adjacent Spaces
                        List<AdjacentSpaceId> adspace = new List<AdjacentSpaceId>();
                        //foreach (string adjSpace in bHoMPanels[i].adjSpaces)
                        //{
                        //    AdjacentSpaceId adjId = new AdjacentSpaceId();
                        //    adjId.spaceIdRef = "Space-" + adjSpace;
                        //    adspace.Add(adjId);
                        //}

                        xmlPanel.AdjacentSpaceId = adspace.ToArray();
                        gbx.Campus.Surface.Add(xmlPanel);
                    }

                }

                // Generate gbXMLSpaces
                /***************************************************/
                if (spaces != null)
                {
                    List<XML.Space> xspaces = new List<Space>();
                    foreach (BHE.Space space in spaces)
                    {
                        XML.Space xspace = new XML.Space();
                        xspace.Name = space.Name;
                        xspace.Area = BH.Engine.Environment.Query.FloorArea(space);
                        xspace.Volume = BH.Engine.Environment.Query.Volume(space);
                        xspace.id = "Space-" + space.BHoM_Guid.ToString();
                        List<XML.Polyloop> ploops = new List<Polyloop>();

                        //Just works for polycurves at the moment. ToDo: fix this for all type of curves
                        IEnumerable<BHG.PolyCurve> bePanel = space.BuildingElements.Select(x => x.BuildingElementGeometry.ICurve() as BHG.PolyCurve);
                            

                        foreach (BHG.PolyCurve pCrv in bePanel)
                        {
                            /* Ensure that all of the surface coordinates are listed in a counterclockwise order
                            * This is a requirement of gbXML Polyloop definitions */

                            BHG.Polyline pline = new BHG.Polyline() { ControlPoints = pCrv.ControlPoints() }; //TODO: Change to ToPolyline method

                            if (BH.Engine.Geometry.Query.IsClockwise(pline, spaceCentrePoint))
                            {
                                var pts = new List<BHG.Point>(pline.DiscontinuityPoints());
                                pts.Reverse();
                                ploops.Add(MakePolyloop(pts));
                            }
                            else
                                ploops.Add(MakePolyloop(pline.DiscontinuityPoints()));


                        }
                        xspace.ShellGeometry.ClosedShell.PolyLoop = ploops.ToArray();

                        gbx.Campus.Building[0].Space.Add(xspace);
                    }
                }
            }



            // Document History                          
            /***************************************************/

            DocumentHistory DocumentHistory = new DocumentHistory();
            DocumentHistory.CreatedBy.date = DateTime.Now.ToString();

            gbx.DocumentHistory = DocumentHistory;
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/
        private static Polyloop MakePolyloop(List<BHG.Point> pts)
        {
            int count = pts.First().SquareDistance(pts.Last()) < BH.oM.Geometry.Tolerance.SqrtDist ? pts.Count-1 : pts.Count;
            Polyloop ploop = new Polyloop();
            List<CartesianPoint> cartpoint = new List<CartesianPoint>();
            for (int i = 0; i < count; i++)
            {
                CartesianPoint cpt = new CartesianPoint();
                List<string> coord = new List<string>();
                coord.Add(Math.Round(pts[i].X, 6).ToString());
                coord.Add(Math.Round(pts[i].Y, 6).ToString());
                coord.Add(Math.Round(pts[i].Z, 6).ToString());
                cpt.Coordinate = coord.ToArray();
                cartpoint.Add(cpt);
            }
            ploop.CartesianPoint = cartpoint.ToArray();
            return ploop;
        }

        /***************************************************/

        private static CartesianPoint ToCartesian(BHG.Point pt)
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

    }
}
