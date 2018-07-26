using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.XML;
using BH.oM.Base;
using BHE = BH.oM.Environment.Elements;
using BHP = BH.oM.Environment.Properties;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.Engine.Environment;


namespace BH.Engine.XML
{

    public static partial class Modify
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static BHE.Space RemoveDuplicates(this BHE.Space space)
        {
            BHE.Space newSpace = new BHE.Space();
            newSpace = space.GetShallowClone() as BHE.Space;

            newSpace.BuildingElements.Clear();

            List<BHE.BuildingElement> uniqueElements = space.BuildingElements.GroupBy(x => x.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).Centre()).Select(x => x.First()).ToList();

            newSpace.BuildingElements.AddRange(uniqueElements);

            return newSpace;
        }
    }
}
