using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Environmental.Elements;
using BH.Engine.Geometry;
using XML_Engine.Modify.gbXMLCleanUp;

using BH.oM.Geometry;

using BH.Engine.Environment;

namespace XML_Engine.Modify
{
    public static partial class Modify
    {
        public static Dictionary<BuildingElement, List<BuildingElement>> gbXMLCleanUp_IdentifyOverLaps(this Building building)
        {
            List<BuildingElement> bes = building.GetBuildingElements();
            Dictionary<BuildingElement, List<BuildingElement>> dic = new Dictionary<BuildingElement, List<BuildingElement>>();

            foreach (BuildingElement be in bes)
            {
                Dictionary<BuildingElement, List<BuildingElement>> m = be.IdentifyOverlaps(bes);
                if(m[be].Count > 0)
                    dic.Add(be, m[be]);
            }

            return dic;
        }

        public static List<BuildingElement> gbXMLCleanUp_OverLapsList(this Dictionary<BuildingElement, List<BuildingElement>> dic)
        {
            List<BuildingElement> rtn = new List<BuildingElement>();
            foreach (KeyValuePair<BuildingElement, List<BuildingElement>> kvp in dic)
            {
                if (kvp.Value.Count > 0)
                {
                    rtn.Add(kvp.Key);
                    rtn.AddRange(kvp.Value);
                }
            }

            return rtn;
        }

        public static Building gbXMLCleanUp_Step1(this Building building, Dictionary<BuildingElement, List<BuildingElement>> overlaps)
        {
            foreach(KeyValuePair<BuildingElement, List<BuildingElement>> kvp in overlaps)
            {
                //Split the key by the list of overlaps
                Dictionary<BuildingElement, List<BuildingElement>> replacements = kvp.Key.SplitElement(kvp.Value);

                foreach(KeyValuePair<BuildingElement, List<BuildingElement>> kvp2 in replacements)
                    if(kvp2.Value.Count > 1)
                        building = building.ChangeBuildingElements(kvp2.Key, kvp2.Value);
            }

            return building;
        }

        public static Building gbXMLCleanUp_Step2(this Building building)
        {
            for(int x = 0; x < building.BuildingElements.Count; x++)
            {
                building.BuildingElements[x] = building.BuildingElements[x].AmendSingleAdjacencies(building);

                for(int y = 0; y < building.Spaces.Count; y++)
                {
                    for(int z = 0; z < building.Spaces[y].BuildingElements.Count; z++)
                    {
                        if (building.Spaces[y].BuildingElements[z].BHoM_Guid == building.BuildingElements[x].BHoM_Guid)
                            building.Spaces[y].BuildingElements[z] = building.BuildingElements[x];
                    }
                }
            }

            return building;
        }

        public static Building gbXMLCleanUp_Step3(this Building building)
        {
            for (int x = 0; x < building.BuildingElements.Count; x++)
            {
                building.BuildingElements[x] = building.BuildingElements[x].AmendXMLType();

                for (int y = 0; y < building.Spaces.Count; y++)
                {
                    for (int z = 0; z < building.Spaces[y].BuildingElements.Count; z++)
                    {
                        if (building.Spaces[y].BuildingElements[z].BHoM_Guid == building.BuildingElements[x].BHoM_Guid)
                            building.Spaces[y].BuildingElements[z] = building.BuildingElements[x];
                    }
                }
            }

            return building;
        }

        public static Building gbXMLCleanUp_Step4(this Building building)
        {
            for (int x = 0; x < building.BuildingElements.Count; x++)
            {
                building.BuildingElements[x] = building.BuildingElements[x].AmendCadObjectID();

                for (int y = 0; y < building.Spaces.Count; y++)
                {
                    for (int z = 0; z < building.Spaces[y].BuildingElements.Count; z++)
                    {
                        if (building.Spaces[y].BuildingElements[z].BHoM_Guid == building.BuildingElements[x].BHoM_Guid)
                            building.Spaces[y].BuildingElements[z] = building.BuildingElements[x];
                    }
                }
            }

            return building;
        }

        public static List<Polyline> gbXMLTestSplit(this BH.oM.Geometry.ICurve crv, ICurve crv2)
        {
            Polyline p = crv.ICollapseToPolyline(1e-06);
            Polyline p2 = crv2.ICollapseToPolyline(1e-06);

            if (p.IsCoplanar(p2))
            {
                Dictionary<Polyline, List<Polyline>> ori = new Dictionary<Polyline, List<Polyline>>();

                List<Polyline> intersections = p.BooleanIntersection(p2);
                List<Polyline> rtn = new List<Polyline>();
                foreach(Polyline p3 in intersections)
                {
                    if (!ori.ContainsKey(p))
                        ori.Add(p, new List<Polyline>());
                    if (!ori.ContainsKey(p2))
                        ori.Add(p2, new List<Polyline>());

                    ori[p].AddRange(p.SplitAtPoints(p3.ControlPoints));
                    ori[p2].AddRange(p2.SplitAtPoints(p3.ControlPoints));
                    //rtn.AddRange(p.SplitAtPoints(p3.ControlPoints));
                    //rtn.AddRange(p2.SplitAtPoints(p3.ControlPoints));
                }

                foreach(KeyValuePair<Polyline, List<Polyline>> kvp in ori)
                {
                    List<Polyline> remove = new List<Polyline>();
                    foreach(Polyline p5 in kvp.Value)
                    {
                        bool isI = true;
                        foreach (Point px in p5.ControlPoints)
                            if (!intersections[0].ControlPoints.Contains(px))
                                isI = false;

                        if (isI)
                            remove.Add(p5);
                    }

                    foreach (Polyline l5 in remove)
                        kvp.Value.Remove(l5);
                }

                foreach(KeyValuePair<Polyline, List<Polyline>> kvp in ori)
                {
                    foreach(Polyline p5 in kvp.Value)
                    {
                        while (p5.ControlPoints.Last().Distance(p5.ControlPoints.First()) > 0.01)
                        {
                            bool addedPoint = false;
                            foreach (Point px in intersections[0].ControlPoints)
                            {
                                if (!p5.ControlPoints.Contains(px) && !kvp.Key.ControlPoints.Contains(px) && px.Match2Of3(p5.ControlPoints.Last()))
                                {
                                    p5.ControlPoints.Add(px);
                                    addedPoint = true;
                                }
                            }
                            if (!addedPoint)
                                p5.ControlPoints.Add(p5.ControlPoints[0]);
                        }
                    }
                }

                List<Polyline> rtn2 = new List<Polyline>();
                foreach (KeyValuePair<Polyline, List<Polyline>> kvp in ori)
                    rtn2.AddRange(kvp.Value);

                return rtn2;

                /*List<Polyline> nonMatch = new List<Polyline>();
                foreach(Polyline p4 in rtn)
                {
                    bool isI = true;
                    foreach (Point px in p4.ControlPoints)
                        if (!intersections[0].ControlPoints.Contains(px))
                            isI = false;

                    if (!isI)
                        nonMatch.Add(p4);
                }*/

                /*foreach(Polyline p5 in nonMatch)
                {
                    while (p5.ControlPoints.Last().Distance(p5.ControlPoints.First()) > 0.01)
                    {
                        bool addedPoint = false;
                        foreach (Point px in intersections[0].ControlPoints)
                        {
                            if (!p5.ControlPoints.Contains(px) && !p.ControlPoints.Contains(px) && px.Match2Of3(p5.ControlPoints.Last()))
                            {
                                p5.ControlPoints.Add(px);
                                addedPoint = true;
                            }
                        }
                        if (!addedPoint)
                            p5.ControlPoints.Add(p5.ControlPoints[0]);
                    }
                }

                return nonMatch;*/

                //return true;
            }
            return new List<Polyline>();
            //return false;
        }

        public static bool Match2Of3(this Point pt, Point comp)
        {
            bool match2 = false;
            if (pt.X == comp.X)
            {
                if (pt.Y == comp.Y)
                    match2 = true;
                else if (pt.Z == comp.Z)
                    match2 = true;
            }
            else if (pt.Y == comp.Y && pt.Z == comp.Z)
                match2 = true;

            return match2;
        }

        public static List<Polyline> gbXMLTSplit(this Building building)
        {
            List<Polyline> rtn = new List<Polyline>();

            List<BuildingElement> allElements = new List<BuildingElement>();

            foreach(Space s in building.Spaces)
            {
                allElements.AddRange(s.BuildingElements);
            }

            foreach(Space s in building.Spaces)
            {
                foreach(BuildingElement be in s.BuildingElements)
                {
                    foreach(BuildingElement be2 in allElements)
                    {
                        if(be.BHoM_Guid != be2.BHoM_Guid)
                        {
                            Polyline be1P = be.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);
                            Polyline be2P = be2.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);

                            if (be1P.IsCoplanar(be2P))
                            {
                                if (be1P.BooleanIntersection(be2P).Count > 0)
                                {
                                    rtn.AddRange(be1P.BooleanIntersection(be2P));
                                }
                            }
                        }
                    }
                }
            }

            return rtn;
        }
        
    }
}
