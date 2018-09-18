using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.XML;
using BH.oM.Base;
using BHE = BH.oM.Environment.Elements;
using BHB = BH.oM.Base;
using BHP = BH.oM.Environment.Properties;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.Engine.Environment;

namespace BH.Engine.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static List<List<BHE.BuildingElement>> MapByGUID(this List<BHE.BuildingElement> bHoMBuildingElement)
        {
            //Map incoming building elements to their GUID - each returned list contains Building Elements which share the same GUID
            if (bHoMBuildingElement == null)
                return null;

            List<List<BHE.BuildingElement>> duplictatedElements = new List<List<BHE.BuildingElement>>();

            IEnumerable<IGrouping<Guid, BHE.BuildingElement>> groups = bHoMBuildingElement.GroupBy(x => x.BHoM_Guid);

            foreach (var group in groups)
            {
                List<BHE.BuildingElement> duplictatedElement = new List<BHE.BuildingElement>();

                foreach (var element in group)
                {
                    duplictatedElement.Add(element);
                }

                duplictatedElements.Add(duplictatedElement);
            }

          
            return duplictatedElements;

        }
        /***************************************************/
    }
}




