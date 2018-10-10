using BH.oM.Environment.Elements;
using System;
using System.Collections.Generic;

using System.Linq;

namespace BH.Adapter.XML
{
    public static partial class XMLSerializer
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static void SerializeCollection(IEnumerable<Building> inputBuildings, BH.oM.XML.GBXML gbx, bool isIES)
        {
            List<Building> buildings = inputBuildings.ToList();
            for(int x = 0; x < buildings.Count; x++)
            {
                gbx.Campus.Location = BH.Engine.XML.Convert.ToGBXML(buildings[x]);

                if (buildings[x].CustomData.ContainsKey("Place Name"))
                    gbx.Campus.Building[x].StreetAddress = (buildings[x].CustomData["Place Name"]).ToString();
                if (buildings[x].CustomData.ContainsKey("Building Name"))
                    gbx.Campus.Building[x].BuildingType = (buildings[x].CustomData["Building Name"]).ToString();
            }
        }
    }
}