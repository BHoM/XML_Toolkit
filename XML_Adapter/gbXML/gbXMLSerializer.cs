using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XML = XML_Adapter.gbXML;
using BHoM.Base;
using BHE = BHoM.Environmental.Elements;
using BHG = BHoM.Geometry;
using System.Xml.Serialization;


namespace XML_Adapter.gbXML
{


    public class gbXMLSerializer
    {
        private static XML.Polyloop MakePolyloop(List<BHG.Point> pts)
        {
            XML.Polyloop ploop = new Polyloop();
            ploop.CartesianPoint.Clear();
            foreach (BHG.Point pt in pts )
            {
                CartesianPoint Cpt = new CartesianPoint();
                Cpt.Coordinate.Clear();
                Cpt.Coordinate.Add(Math.Round(pt.X, 9));
                Cpt.Coordinate.Add(Math.Round(pt.Y, 9));
                Cpt.Coordinate.Add(Math.Round(pt.Z, 9));
                ploop.CartesianPoint.Add(Cpt);
            }
            return ploop;
        } 
        public static gbXML Serialize(List<BHoMObject> bhomObjects)
        {


            gbXML gbx = new gbXML();
            List<BHE.Panel> pans = bhomObjects.Where(x => x is BHE.Panel).Select(x => x as BHE.Panel).ToList();
            List<BHE.Space> spaces = bhomObjects.Where(x => x is BHE.Space).Select(x => x as BHE.Space).ToList();
            // Generate gbXMLSurfaces
            if (pans !=null)
                {
                gbx.Campus.Surface.Clear();
                for (int i = 0; i < pans.Count; i++)
                {

                    XML.Surface xpanel = new XML.Surface();
                    xpanel.Name = pans[i].Name;
                    xpanel.surfaceType = pans[i].Type;
                    xpanel.id = "Panel-" + pans[i].BHoM_Guid.ToString();
                    PlanarGeometry plGeo = new PlanarGeometry();
                    plGeo.id = "PlanarGeometry" + i.ToString();
                    plGeo.PolyLoop = MakePolyloop(pans[i].External_Contours[0].ControlPoints);
                    xpanel.PlanarGeometry = plGeo;

                    xpanel.AdjacentSpaceId.Clear();
                    foreach (string adjSpace in pans[i].adjSpaces)
                    {
                        AdjacentSpaceId adjId = new AdjacentSpaceId();
                        adjId.spaceIdRef = "Space-" + adjSpace;
                        xpanel.AdjacentSpaceId.Add(adjId);
                    }
                    gbx.Campus.Surface.Add(xpanel);
                    }

                }



        // Generate gbXMLSpaces
            if (spaces != null)
            {
                gbx.Campus.Building[0].Space.Clear();
                foreach (BHE.Space space in spaces)
                {
                    XML.Space xspace = new XML.Space();
                    xspace.Name = space.Name;
                    xspace.ShellGeometry.ClosedShell.PolyLoop.Clear();
                    xspace.id = "Space-" + space.BHoM_Guid.ToString();
                    foreach (BHG.Polyline pline in space.Polylines)
                    {
                        xspace.ShellGeometry.ClosedShell.PolyLoop.Add(MakePolyloop(pline.ControlPoints));
                    }
                    gbx.Campus.Building[0].Space.Add(xspace);
                }
            }
            return gbx;
        }
    }
}
