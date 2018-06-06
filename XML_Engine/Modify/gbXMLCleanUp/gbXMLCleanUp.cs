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

using BH.Engine.Serialiser;

using BH.Engine.XML;

namespace XML_Engine.Modify
{
    public static partial class Modify
    {
        public static Dictionary<BuildingElement, List<BuildingElement>> gbXMLCleanUp_IdentifyOverLaps(this Building building)
        {
            building = building.BreakReferenceClone();

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

        public static List<BuildingElement> gbXMLCleanUp_OverLapsListIndex(this Dictionary<BuildingElement, List<BuildingElement>> dic, int index)
        {
            return dic.ElementAt(index).Value;
        }

        public static Building gbXMLCleanUp_Step1(this Building building, Dictionary<BuildingElement, List<BuildingElement>> overlaps)
        {
            foreach(KeyValuePair<BuildingElement, List<BuildingElement>> kvp in overlaps)
            {
                //Split the key by the list of overlaps
                Dictionary<BuildingElement, List<BuildingElement>> replacements = kvp.Key.SplitElement(kvp.Value);

                foreach (KeyValuePair<BuildingElement, List<BuildingElement>> kvp2 in replacements)
                {
                    if (kvp2.Value.Count > 1)
                        building = building.ChangeBuildingElements(kvp2.Key, kvp2.Value);
                    else if (kvp2.Value.Count == 1)
                        building = building.RemovePerfectOverlaps(kvp.Value[0]);
                }
            }

            /*List<BuildingElement> elements = building.GetBuildingElements();
            foreach (BuildingElement be in elements)
                building = building.CleanBuildingDupAdj(be);*/
            /*foreach (KeyValuePair<BuildingElement, List<BuildingElement>> kvp in overlaps)
                building = building.CleanBuildingDupAdj(kvp.Key);*/

            /*foreach (BuildingElement be in building.BuildingElements)
                building = building.CleanBEs(be);*/

            return building;
        }

        public static Building gbXMLCleanUp_Step2(this Building building)
        {
            building = building.BreakReferenceClone();

            for (int x = 0; x < building.BuildingElements.Count; x++)
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
            building = building.BreakReferenceClone();

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
            building = building.BreakReferenceClone();

            for (int x = 0; x < building.BuildingElements.Count; x++)
            {
                BuildingElement beTest = building.BuildingElements[x].AmendCadObjectID(building);
                if (beTest == null)
                    continue;

                building.BuildingElements[x] = beTest;

                for (int y = 0; y < building.Spaces.Count; y++)
                {
                    for (int z = 0; z < building.Spaces[y].BuildingElements.Count; z++)
                    {
                        if (building.Spaces[y].BuildingElements[z].BHoM_Guid == beTest.BHoM_Guid)
                            building.Spaces[y].BuildingElements[z] = beTest;
                    }
                }
            }

            return building;
        }
        
        public static Building gbxmlCleanUp_SpaceCleanse(this Building building)
        {
            building = building.BreakReferenceClone();

            for (int x = 0; x < building.Spaces.Count; x++)
                building.Spaces[x] = building.Spaces[x].CleanSpace();
                
            return building;
        }

        public static Building gbXMLCleanUp_MatchSingleAdjacencies(this Building building)
        {
            building = building.BreakReferenceClone();

            List<BuildingElement> allBEs = building.GetBuildingElements();
            List<BuildingElement> errorBEs = allBEs.Where(x => x.AdjacentError() != null).ToList();

            foreach(BuildingElement be in errorBEs)
            {
                BuildingElement be2 = be.AmendSingleAdjacencies(allBEs);

                for (int x = 0; x < building.BuildingElements.Count; x++)
                {
                    if (building.BuildingElements[x].BHoM_Guid == be2.BHoM_Guid)
                        building.BuildingElements[x] = be2;

                    for (int y = 0; y < building.Spaces.Count; y++)
                    {
                        for (int z = 0; z < building.Spaces[y].BuildingElements.Count; z++)
                        {
                            if (building.Spaces[y].BuildingElements[z].BHoM_Guid == building.BuildingElements[x].BHoM_Guid)
                                building.Spaces[y].BuildingElements[z] = building.BuildingElements[x];
                        }
                    }
                }
            }

            return building;
        }

        public static Building gbXMLCleanUp_FindAdjacencies(this Building building)
        {
            building = building.BreakReferenceClone();

            List<BuildingElement> errorBEs = building.GetBuildingElements().Where(x => x.AdjacentError() != null).ToList();

            foreach (Space s in building.Spaces)
            {
                List<BuildingElement> besToAdd = new List<BuildingElement>();

                foreach (BuildingElement be in s.BuildingElements)
                {
                    //Check if the building element verticies match at least one other building element in this space - if it doesn't then it is a problem and is missing some geometry - try to find the missing geometry from the errorBEs

                    Polyline pLine = be.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);
                    bool allMatch = true;
                    List<Point> nonMatches = new List<Point>();

                    foreach (Point pt in pLine.DiscontinuityPoints())
                    {
                        BuildingElement be2 = s.BuildingElements.Where(x => x.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).DiscontinuityPoints().Contains(pt) && x.BHoM_Guid != be.BHoM_Guid).FirstOrDefault();

                        if(be2 == null)
                        {
                            allMatch = false;
                            nonMatches.Add(pt);
                        }
                    }

                    if(!allMatch)
                    {
                        //This BE has a vertex that does not match - find the missing geometry from the error BEs
                        foreach(Point pt in nonMatches)
                        {
                            BuildingElement be2 = errorBEs.Where(x => x.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).DiscontinuityPoints().Contains(pt) && x.BHoM_Guid != be.BHoM_Guid).FirstOrDefault();

                            if(be2 != null)
                            {
                                if (!be2.AdjacentSpaces.Contains(s.BHoM_Guid))
                                    be2.AdjacentSpaces.Add(s.BHoM_Guid);
                                if (!s.BuildingElements.Contains(be2))
                                    besToAdd.Add(be2);
                            }
                        }
                    }
                }

                if(besToAdd.Count > 0)
                    s.BuildingElements.AddRange(besToAdd);
            }

            /*List<BuildingElement> errorBEs = building.GetBuildingElements().Where(x => x.AdjacentError() != null).ToList();

            foreach(BuildingElement be in errorBEs)
            {
                BuildingElement be2 = be.AmendSingleAdjacencies(building.Spaces.Where(x => x.BHoM_Guid == be.AdjacentSpaces[0]).FirstOrDefault());

                for (int x = 0; x < building.BuildingElements.Count; x++)
                {
                    if (building.BuildingElements[x].BHoM_Guid == be2.BHoM_Guid)
                        building.BuildingElements[x] = be2;

                    for (int y = 0; y < building.Spaces.Count; y++)
                    {
                        for (int z = 0; z < building.Spaces[y].BuildingElements.Count; z++)
                        {
                            if (building.Spaces[y].BuildingElements[z].BHoM_Guid == building.BuildingElements[x].BHoM_Guid)
                                building.Spaces[y].BuildingElements[z] = building.BuildingElements[x];
                        }
                    }
                }
            }*/

            return building;
        }

        public static Building gbXMLCleanUp_RemoveDuplicatesWithIC(this Building building)
        {
            building = building.BreakReferenceClone();

            List<BuildingElement> besToRemove = new List<BuildingElement>();
            List<BuildingElement> allBEs = building.GetBuildingElements();

            foreach(BuildingElement be in allBEs)
            {
                //Find any BE's that contain the centre point of this BE
                Point cPt = be.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).Centre();
                List<BuildingElement> foundBEs = allBEs.Where(x => x.BHoM_Guid != be.BHoM_Guid && x.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).IsContaining(new List<Point> { cPt })).ToList();
                foreach(BuildingElement be2 in foundBEs)
                {
                    //Check the adjacency is ok on be and then we'll look to remove be2
                }
            }


            return building;
        }

        public static Building ReferenceIssueTest(this Building building)
        {
            building = building.BreakReferenceClone();
            building.BuildingElements = new List<BuildingElement>();
            return building;
        }

        public static Building BreakReferenceClone(this Building building)
        {
            //Serialise to BSON and back to break the pass by reference issue
            MongoDB.Bson.BsonDocument bd = BH.Engine.Serialiser.Convert.ToBson(building);

            return (Building)BH.Engine.Serialiser.Convert.FromBson(bd);
        }
    }
}
