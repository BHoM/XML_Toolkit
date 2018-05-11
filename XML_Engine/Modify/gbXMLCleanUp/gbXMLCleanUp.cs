using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Environmental.Elements;
using BH.Engine.Geometry;
using XML_Engine.Modify.gbXMLCleanUp;

namespace XML_Engine.Modify
{
    public static partial class Modify
    {
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
    }
}
