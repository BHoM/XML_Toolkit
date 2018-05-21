using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.XML;
using BH.oM.Base;
using BHE = BH.oM.Environmental.Elements;
using BHP = BH.oM.Environmental.Properties;
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

        public static string ConstructionIdRef(this BHE.BuildingElement bHoMBuildingElement, List<BHE.BuildingElement> buildingElementList)
        {
            List<BHP.BuildingElementProperties> props = buildingElementList.Select(x => x.BuildingElementProperties).Distinct(new BH.Engine.Base.Objects.BHoMObjectNameComparer()).Select(x => x as BHP.BuildingElementProperties).ToList();

            string refId = (props.IndexOf(props.Find(x => x.Name == bHoMBuildingElement.Name)) + 1000).ToString();

            return refId;
        }

        /***************************************************/

    }
}




