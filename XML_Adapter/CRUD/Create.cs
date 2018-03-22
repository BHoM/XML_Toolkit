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

namespace XML_Adapter.gbXML
{
    public class gbXMLSerializer
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static void Serialize(List<IBHoMObject> bhomObjects, BH.oM.XML.gbXML gbx)
        {
            foreach (IBHoMObject obj in bhomObjects)
            {
                List<BHE.BuildingElementPanel> bHoMPanels = new List<BHE.BuildingElementPanel>();
                List<BHE.BuildingElement> bHoMBuildingElement = new List<BHE.BuildingElement>();
                List<BHP.BuildingElementProperties> bHoMBuildingElementProperties = new List<BHP.BuildingElementProperties>();

                if (obj.GetType() == typeof(BHE.Space))
                {
                    BHE.Space bHoMSpace = obj as BHE.Space;
                    bHoMPanels.AddRange(bHoMSpace.BuildingElements.Select(x => x.BuildingElementGeometry as BHE.BuildingElementPanel));
                    bHoMBuildingElement.AddRange(bHoMSpace.BuildingElements);
                    bHoMBuildingElementProperties.AddRange(bHoMSpace.BuildingElements.Select(x => x.BuildingElementProperties as BHP.BuildingElementProperties));
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
                        xmlPanel.Name = bHoMBuildingElement[i].BuildingElementProperties.Name;
                        //xmlPanel.surfaceType = BH.Engine.XML.Convert.ToGbXMLSurfaceType(bHoMPanels[i]);
                        xmlPanel.surfaceType = BH.Engine.XML.Convert.ToGbXMLSurfaceType(bHoMBuildingElement[i].BuildingElementProperties.CustomData["SAM_BuildingElementType"].ToString());
                        xmlPanel.id = "Panel-" + bHoMPanels[i].BHoM_Guid.ToString();

                        RectangularGeometry xmlRectangularGeom = BH.Engine.XML.Convert.ToGbXML(bHoMPanels[i]);
                        PlanarGeometry plGeo = new PlanarGeometry();
                        plGeo.id = "PlanarGeometry" + i.ToString();

                        /* Ensure that all of the surface coordinates are listed in a counterclockwise order
                         * This is a requirement of gbXML Polyloop definitions */
                        //TODO: Update to a ToPolyline() method once an appropriate version has been implemented

                        BHG.Polyline pline = new BHG.Polyline() { ControlPoints = bHoMPanels[i].PolyCurve.ControlPoints() }; //TODO: Change to ToPolyline method

                        if (BH.Engine.Geometry.Query.IsClockwise(pline, spaceCentrePoint))
                        {
                            plGeo.PolyLoop = BH.Engine.XML.Convert.ToGbXML(pline.Flip());
                            xmlRectangularGeom.Polyloop = BH.Engine.XML.Convert.ToGbXML(pline.Flip()); //TODO: for bounding curve
                        }
                        else
                        {
                            plGeo.PolyLoop = BH.Engine.XML.Convert.ToGbXML(pline);
                            xmlRectangularGeom.Polyloop = BH.Engine.XML.Convert.ToGbXML(pline); //TODO: for bounding curve
                        }

                        xmlRectangularGeom.CartesianPoint = BH.Engine.XML.Convert.ToGbXML(BH.Engine.Geometry.Query.Centre(pline));

                       
                        xmlPanel.PlanarGeometry = plGeo;
                        xmlPanel.RectangularGeometry = xmlRectangularGeom;
                        

                        // Create openings
                        if (bHoMPanels[i].Openings.Count > 0)
                        {
                            List<Opening> xmlOpenings = new List<Opening>();

                            foreach (BHE.BuildingElementOpening opening in bHoMPanels[i].Openings)
                            {
                                Opening gbXMLOpening = BH.Engine.XML.Convert.ToGbXML(opening);
                                xmlOpenings.Add(gbXMLOpening);
                            }
                            xmlPanel.Opening = xmlOpenings.ToArray();
                        }


                        // Adjacent Spaces
                        /***************************************************/
                        List<AdjacentSpaceId> adspace = new List<AdjacentSpaceId>();

                        foreach (Guid adjSpace in bHoMBuildingElement[i].AdjacentSpaces)
                        {
                            AdjacentSpaceId adjId = new AdjacentSpaceId();
                            adjId.spaceIdRef = "Space-" + adjSpace;
                            adspace.Add(adjId);
                        }

                        xmlPanel.AdjacentSpaceId = adspace.ToArray();
                        gbx.Campus.Surface.Add(xmlPanel);
                    }

                }


                // Generate gbXMLSpaces
                /***************************************************/
                if (spaces != null)
                {
                    List<BH.oM.XML.Space> xspaces = new List<Space>();
                    foreach (BHE.Space space in spaces)
                    {
                        BH.oM.XML.Space xspace = BH.Engine.XML.Convert.ToGbXML(space);
                        List<BH.oM.XML.Polyloop> ploops = new List<Polyloop>();

                        //Just works for polycurves at the moment. ToDo: fix this for all type of curves
                        IEnumerable<BHG.PolyCurve> bePanel = space.BuildingElements.Select(x => x.BuildingElementGeometry.ICurve() as BHG.PolyCurve);

                        foreach (BHG.PolyCurve pCrv in bePanel)
                        {
                            /* Ensure that all of the surface coordinates are listed in a counterclockwise order
                            * This is a requirement of gbXML Polyloop definitions */
                            BHG.Polyline pline = new BHG.Polyline() { ControlPoints = pCrv.ControlPoints() }; //TODO: Change to ToPolyline method

                            if (BH.Engine.Geometry.Query.IsClockwise(pline, spaceCentrePoint))
                                ploops.Add(BH.Engine.XML.Convert.ToGbXML(pline.Flip()));
                            else
                                ploops.Add(BH.Engine.XML.Convert.ToGbXML(pline));
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
        
    }

}
