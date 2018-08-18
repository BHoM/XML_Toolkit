using System;
using System.Collections.Generic;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using System.Linq;
using BHE = BH.oM.Environment.Elements;


namespace BH.Engine.XML
{

    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static double BuildingArea(BHE.Building bHoMBuilding)
        {
            List<double> area = new List<double>();
            foreach (BHE.Space space in bHoMBuilding.Spaces)
            {
                area.Add(BH.Engine.Environment.Query.FloorArea(space));
            }
            return area.Sum();
        }

        /***************************************************/

    }
}




