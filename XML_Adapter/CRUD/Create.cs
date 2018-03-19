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
using BH.Engine.XML;

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
                        xmlPanel.surfaceType = BH.Engine.XML.Convert.ToGbXMLSurfaceType(bHoMPanels[i]);
                        xmlPanel.id = "Panel-" + bHoMPanels[i].BHoM_Guid.ToString();
                        RectangularGeometry xmlRectangularGeom = new RectangularGeometry();
                        xmlRectangularGeom.Tilt = BH.Engine.Environment.Query.Inclination(bHoMPanels[i]);
                        xmlRectangularGeom.Azimuth = BH.Engine.Environment.Query.Orientation(bHoMPanels[i]);
                        xmlRectangularGeom.Height = BH.Engine.Environment.Query.AltitudeRange(bHoMPanels[i]);
                       
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
                            plGeo.PolyLoop = BH.Engine.XML.Convert.ToGbXML(pts);
                            xmlRectangularGeom.Polyloop = BH.Engine.XML.Convert.ToGbXML(pts); //TODO: for boundingbox
                        }
                        else
                        {
                            plGeo.PolyLoop = BH.Engine.XML.Convert.ToGbXML(pline.DiscontinuityPoints());
                            xmlRectangularGeom.Polyloop = BH.Engine.XML.Convert.ToGbXML(pline.DiscontinuityPoints()); //TODO: for boundingbox
                        }

                        xmlRectangularGeom.CartesianPoint = BH.Engine.XML.Convert.ToGbXML(BH.Engine.Geometry.Query.Centre(pline));

                        xmlPanel.PlanarGeometry = plGeo;
                        xmlPanel.RectangularGeometry = xmlRectangularGeom;


                        // Openings
                        /***************************************************/

                        if (bHoMPanels[i].Openings.Count > 0)
                        {
                            xmlPanel.Opening = AddOpening(bHoMPanels[i].Openings, spaceCentrePoint).ToArray();
                        }


                        // Adjacent Spaces
                        /***************************************************/
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
                                ploops.Add(BH.Engine.XML.Convert.ToGbXML(pts));
                            }
                            else
                                ploops.Add(BH.Engine.XML.Convert.ToGbXML(pline.DiscontinuityPoints()));

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


      
        //Openings
        /***************************************************/

        private static List<Opening> AddOpening(List<BHE.BuildingElementOpening> openings, BHG.Point spaceCentrePoint)
        {
             List<Opening> xmlOpening = new List<Opening>();

             foreach (BHE.BuildingElementOpening opening in openings)
             {
                xmlOpening.Add(new Opening());

                if (BH.Engine.Geometry.Query.IsClockwise(opening.PolyCurve, spaceCentrePoint))
                {
                    var pts = new List<BHG.Point>(opening.PolyCurve.ControlPoints());
                    pts.Reverse();
                    xmlOpening.Last().PlanarGeometry.PolyLoop = BH.Engine.XML.Convert.ToGbXML(pts);
                    xmlOpening.Last().RectangularGeometry.Polyloop = BH.Engine.XML.Convert.ToGbXML(pts);
                }
                else
                {
                    xmlOpening.Last().PlanarGeometry.PolyLoop = BH.Engine.XML.Convert.ToGbXML(opening.PolyCurve.ControlPoints());
                    xmlOpening.Last().RectangularGeometry.Polyloop = BH.Engine.XML.Convert.ToGbXML(opening.PolyCurve.ControlPoints());
                }

                    xmlOpening.Last().id = "Opening" + xmlOpening.Count().ToString();
                    xmlOpening.Last().Name = "OpeningName" + xmlOpening.Count().ToString();

             }

            return xmlOpening;
        }

        
        /***************************************************/

    }
}
