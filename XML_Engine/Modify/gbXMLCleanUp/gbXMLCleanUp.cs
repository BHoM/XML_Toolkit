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
                building.BuildingElements[x] = building.BuildingElements[x].AmendXMLType();
            }

            return building;
        }
    }
}
