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
            foreach (KeyValuePair<BuildingElement, List<BuildingElement>> kvp in overlaps)
                building = building.CleanBuildingDupAdj(kvp.Key);

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
            /*for (int x = 0; x < building.Spaces.Count; x++)
                building.Spaces[x] = building.Spaces[x].CleanSpace();
                */
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
