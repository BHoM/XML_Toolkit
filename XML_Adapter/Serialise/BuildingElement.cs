using BH.oM.Environment.Elements;
using System;
using System.Collections.Generic;

using System.Linq;
using BH.Engine.Environment;

using BH.oM.XML;
using BH.Engine.XML;

using BH.oM.Geometry;
using BH.Engine.Geometry;

namespace BH.Adapter.XML
{
    public partial class GBXMLSerializer
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static void SerializeCollection(IEnumerable<IEnumerable<BuildingElement>> inputElements, BH.oM.XML.GBXML gbx, bool isIES = false)
        {
            List<List<BuildingElement>> elementsAsSpaces = new List<List<BuildingElement>>();

            foreach (IEnumerable<BuildingElement> input in inputElements)
                elementsAsSpaces.Add(input.ToList());

            List<BuildingElement> uniqueBuildingElements = elementsAsSpaces.UniqueBuildingElements();

            List<BH.oM.Environment.Elements.Space> spaces = elementsAsSpaces.Spaces();

            List<BuildingElement> usedBEs = new List<BuildingElement>();

            foreach(List<BuildingElement> space in elementsAsSpaces)
            {
                //For each collection of BuildingElements that define a space, convert the panels to XML surfaces and add to the GBXML
                List<Surface> spaceSurfaces = new List<Surface>();

                for(int x = 0; x < space.Count; x++)
                {
                    if (usedBEs.Where(i => i.BHoM_Guid == space[x].BHoM_Guid).FirstOrDefault() != null) continue;

                    List<BH.oM.Environment.Elements.Space> adjacentSpaces = BH.Engine.Environment.Query.AdjacentSpaces(space[x], elementsAsSpaces, spaces);

                    Surface srf = new Surface();
                    srf.Name = "Panel-" + gbx.Campus.Surface.Count.ToString();
                    srf.SurfaceType = BH.Engine.XML.Convert.ToGBXMLType(space[x], adjacentSpaces, isIES);

                    if (space[x].BuildingElementProperties != null)
                        srf.CADObjectID = BH.Engine.XML.Query.CadObjectId(space[x], isIES);

                    srf.ID = srf.Name;
                    srf.ExposedToSun = BH.Engine.Environment.Query.ExposedToSun(srf.SurfaceType).ToString();

                    if (isIES)
                        srf.ConstructionIDRef = BH.Engine.XML.Query.IdRef(space[x]);

                    RectangularGeometry srfGeom = BH.Engine.XML.Convert.ToGBXML(space[x]);
                    PlanarGeometry plGeom = new PlanarGeometry();
                    plGeom.ID = "PlanarGeometry-" + x.ToString();

                    // Ensure that all of the surface coordinates are listed in a counterclockwise order
                    // This is a requirement of GBXML Polyloop definitions 

                    Polyline pline = new Polyline() { ControlPoints = space[x].PanelCurve.IControlPoints() }; //TODO: Change to ToPolyline method
                    Polyline srfBound = new Polyline();

                    if (!BH.Engine.Environment.Query.NormalAwayFromSpace(pline, space))
                    {
                        plGeom.PolyLoop = BH.Engine.XML.Convert.ToGBXML(pline.Flip());
                        srfBound = pline.Flip();

                        srfGeom.Tilt = Math.Round(BH.Engine.Environment.Query.Tilt(srfBound), 3);
                        srfGeom.Azimuth = Math.Round(BH.Engine.Environment.Query.Azimuth(srfBound, Vector.YAxis), 3);

                    }
                    else
                    {
                        plGeom.PolyLoop = BH.Engine.XML.Convert.ToGBXML(pline);
                        srfBound = pline;
                    }

                    srf.PlanarGeometry = plGeom;
                    srf.RectangularGeometry = srfGeom;

                    //Adjacent Spaces
                    List<AdjacentSpaceId> adjIDs = new List<AdjacentSpaceId>();
                    foreach (BH.oM.Environment.Elements.Space sp in adjacentSpaces)
                        adjIDs.Add(sp.GetAdjacentSpaceID());
                    srf.AdjacentSpaceID = adjIDs.ToArray();

                    //Openings
                    if (space[x].Openings.Count > 0)
                        srf.Opening = Serialize(space[x].Openings, space, elementsAsSpaces, spaces, gbx, isIES).ToArray();

                    gbx.Campus.Surface.Add(srf);

                    usedBEs.Add(space[x]);
                }

                //Create the space in
                BH.oM.XML.Space xmlSpace = new oM.XML.Space();
                xmlSpace.Name = "NEEDSCHANGING";
                xmlSpace.ID = "NEEDSCHANGINGNUMBER";
                xmlSpace.ShellGeometry.ClosedShell.PolyLoop = BH.Engine.XML.Query.ClosedShellGeometry(space).ToArray();
                xmlSpace.SpaceBoundary = BH.Engine.XML.Query.SpaceBoundaries(space, uniqueBuildingElements);
                if (BH.Engine.Environment.Query.FloorGeometry(space) != null)
                    xmlSpace.PlanarGeoemtry.PolyLoop = BH.Engine.XML.Convert.ToGBXML(BH.Engine.Environment.Query.FloorGeometry(space));

                gbx.Campus.Building[0].Space.Add(xmlSpace);
            }
        }

        

        /*public static void SerializeCollection(IEnumerable<BuildingElement> inputElements, BH.oM.XML.GBXML gbx, bool isIES)
        {
            List<BuildingElement> buildingElements = inputElements.ToList();
        }*/
    }
}