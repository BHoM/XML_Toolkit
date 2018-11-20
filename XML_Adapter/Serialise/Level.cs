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

        public static void Serialize(List<BH.oM.Architecture.Elements.Level> levels, List<List<BuildingElement>> spaces, BH.oM.XML.GBXML gbx, bool isIES)
        {
            List<BH.oM.XML.BuildingStorey> xmlLevels = new List<BuildingStorey>();

            foreach(BH.oM.Architecture.Elements.Level level in levels)
            {
                BuildingStorey storey = BH.Engine.XML.Convert.ToGBXML(level);
                Polyline storeyGeometry = BH.Engine.Environment.Query.StoreyGeometry(level, spaces);
                if (storeyGeometry == null)
                    continue;
                storey.PlanarGeometry.PolyLoop = BH.Engine.XML.Convert.ToGBXML(storeyGeometry);
                storey.PlanarGeometry.ID = "Level" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);
                //storey.Name = "Level-" + level.Elevation.ToString();
                storey.Name = level.Name;
                //storey.ID = "Level-" + level.Elevation.ToString();
                storey.ID = level.Name.Replace(" ", "").ToLower();
                storey.Level = (float)level.Elevation;
                xmlLevels.Add(storey);
            }

            gbx.Campus.Building[0].BuildingStorey = xmlLevels.ToArray();
        }
    }
}