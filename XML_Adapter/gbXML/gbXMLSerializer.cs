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

namespace XML_Adapter.gbXML
{
    public class gbXMLSerializer
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static gbXML Serialize(List<IObject> bhomObjects)
        {
            gbXML gbx = new gbXML();

            List<BHE.BuildingElementPanel> bHoMPanels = bhomObjects.Where(x => x is BHE.BuildingElementPanel).Select(x => x as BHE.BuildingElementPanel).ToList();
            List<BHE.Space> spaces = bhomObjects.Where(x => x is BHE.Space).Select(x => x as BHE.Space).ToList();

            // Generate gbXMLSurfaces
            if (bHoMPanels !=null)
                {
                List<Surface> srfs = new List<Surface>();
                for (int i = 0; i < bHoMPanels.Count; i++)
                {
                    Surface xmlPanel = new Surface();
                    xmlPanel.Name = bHoMPanels[i].Name;
                    //xpanel.surfaceType = pans[i].type;
                    xmlPanel.id = "Panel-" + bHoMPanels[i].BHoM_Guid.ToString();
                    PlanarGeometry plGeo = new PlanarGeometry();
                    plGeo.id = "PlanarGeometry" + i.ToString();

                    plGeo.PolyLoop = MakePolyloop(bHoMPanels[i].PolyCurve.ControlPoints());

                    xmlPanel.PlanarGeometry = plGeo;
                    List<AdjacentSpaceId> adspace = new List<AdjacentSpaceId>();
                    //foreach (string adjSpace in pans[i].adjSpaces)
                    //{
                    //    AdjacentSpaceId adjId = new AdjacentSpaceId();
                    //    adjId.spaceIdRef = "Space-" + adjSpace;
                    //    adspace.Add(adjId);
                    //}
                    xmlPanel.AdjacentSpaceId = adspace.ToArray();
                    srfs.Add(xmlPanel);
                    }
                gbx.Campus.Surface = srfs.ToArray();

                }



        // Generate gbXMLSpaces
            if (spaces != null)
            {
                List<XML.Space> xspaces = new List<Space>();
                foreach (BHE.Space space in spaces)
                {
                    XML.Space xspace = new XML.Space();
                    xspace.Name = space.Name;
                    xspace.id = "Space-" + space.BHoM_Guid.ToString();
                    List<XML.Polyloop> ploops = new List<Polyloop>();
                    //foreach (BHG.Polyline pline in space.Polylines)
                    //{
                    //    ploops.Add(MakePolyloop(pline.ControlPoints));
                    //}
                    xspace.ShellGeometry.ClosedShell.PolyLoop = ploops.ToArray();
                    xspaces.Add(xspace);
                }
                gbx.Campus.Building[0].Space = xspaces.ToArray();
            }
            return gbx;
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/
        private static Polyloop MakePolyloop(List<BHG.Point> pts)
        {
            Polyloop ploop = new Polyloop();
            List<CartesianPoint> cartpoint = new List<CartesianPoint>();
            for (int i = 0; i < pts.Count - 1; i++)
            {
                CartesianPoint cpt = new CartesianPoint();
                List<string> coord = new List<string>();
                coord.Add(Math.Round(pts[i].X, 9).ToString());
                coord.Add(Math.Round(pts[i].Y, 9).ToString());
                coord.Add(Math.Round(pts[i].Z, 9).ToString());
                cpt.Coordinate = coord.ToArray();
                cartpoint.Add(cpt);
            }
            ploop.CartesianPoint = cartpoint.ToArray();
            return ploop;
        }
    }
}
