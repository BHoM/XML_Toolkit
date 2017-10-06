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


namespace XML_Adapter.gbXML
{


    public class gbXMLDeserializer
    {
        private static BHG.Group<BHG.Curve> MakeCurveGroup(Polyloop ploop)
        {
            List<BHG.Point> pts = new List<BH.oM.Geometry.Point>();
            if ( 1 <= ploop.CartesianPoint.Count())
            {
                foreach (CartesianPoint Cpt in ploop.CartesianPoint)
                {
                    BHG.Point pt = new BHG.Point();
                    pt.X = Convert.ToDouble(Cpt.Coordinate[0]);
                    pt.Y = Convert.ToDouble(Cpt.Coordinate[1]);
                    pt.Z = Convert.ToDouble(Cpt.Coordinate[2]);
                    pts.Add(pt);
                }
                pts.Add((BHG.Point)pts[0].Duplicate());
            }
            BHG.Polyline pline = new BHG.Polyline(pts);
            BHG.Group<BHG.Curve> crvs = new BHG.Group<BH.oM.Geometry.Curve>();
            crvs.Add(pline);
            return crvs;
        }
        private static BHG.Polyline MakePolyline(Polyloop ploop)
        {
            List<BHG.Point> pts = new List<BH.oM.Geometry.Point>();
            if (1 <= ploop.CartesianPoint.Count())
            {
                foreach (CartesianPoint Cpt in ploop.CartesianPoint)
                {
                    BHG.Point pt = new BHG.Point();
                    pt.X = Convert.ToDouble(Cpt.Coordinate[0]);
                    pt.Y = Convert.ToDouble(Cpt.Coordinate[1]);
                    pt.Z = Convert.ToDouble(Cpt.Coordinate[2]);
                    pts.Add(pt);
                }
                pts.Add((BHG.Point)pts[0].Duplicate());
            }
            BHG.Polyline pline = new BHG.Polyline(pts);
            return pline;
        }
        public static List<BHoMObject> Deserialize(gbXML gbx)
        { 
            List<BHoMObject> bhomObjects = new List<BHoMObject>();
            List<XML.Surface> srfs = gbx.Campus.Surface.ToList();
            List<XML.Space> spaces = gbx.Campus.Building[0].Space.ToList();
            // Generate gbXMLSurfaces
            if (srfs !=null)
            {
                foreach (Surface srf in srfs)
                {

                    BHE.Panel pan = new BHE.Panel();
                    pan.Name = srf.Name;
                    pan.Type = srf.surfaceType;
                    pan.CustomData.Add("gbXML-ID", srf.id);
                    pan.External_Contours = MakeCurveGroup(srf.PlanarGeometry.PolyLoop);
                    bhomObjects.Add(pan);
                }
            }

        // Generate gbXMLSpaces
            if (spaces != null)
            {
                foreach (XML.Space space in spaces)
                {
                    BHE.Space xspace = new BHE.Space();
                    xspace.Name = space.Name;
                    xspace.CustomData.Add("gbXML-ID", space.id);
                    if (4<=space.ShellGeometry.ClosedShell.PolyLoop.Count())
                    {
                        List<BHG.Polyline> plines = new List<BHG.Polyline>();
                        foreach (XML.Polyloop ploop in space.ShellGeometry.ClosedShell.PolyLoop)
                        {
                            if (3 <= ploop.CartesianPoint.Count())
                            {
                                plines.Add(MakePolyline(ploop));
                            }
                        }
                        xspace.Polylines = plines;
                    }
                    bhomObjects.Add(xspace);
                }
            }
            return bhomObjects;
        }
    }
}
