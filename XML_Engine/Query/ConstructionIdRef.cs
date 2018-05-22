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

        public static string IdRef(this BHE.BuildingElement bHoMBuildingElement, List<BHE.BuildingElement> buildingElementList)
        {
            List<BHP.BuildingElementProperties> props = buildingElementList.Select(x => x.BuildingElementProperties).Distinct(new BH.Engine.Base.Objects.BHoMObjectNameComparer()).Select(x => x as BHP.BuildingElementProperties).ToList();

            return bHoMBuildingElement.BuildingElementProperties.IdRef(props);
        }

        /***************************************************/

        public static string IdRef(this BHP.BuildingElementProperties bHoMprop, List<BHP.BuildingElementProperties> bHoMprops)
        {
            if (bHoMprop == null) return "-1";

            Guid g = bHoMprop.BHoM_Guid;
            String g2 = g.ToString().Split('-')[0];
            int[] c = new int[g2.Length];

            for (int x = 0; x < g2.Length; x++)
                c[x] = (int)g2[x];

            String s = "";
            foreach (int i in c)
                s += i.ToString();

            return s;

            /*List<BHP.BuildingElementProperties> props = bHoMprops.Distinct(new BH.Engine.Base.Objects.BHoMObjectNameComparer()).Select(x => x as BHP.BuildingElementProperties).ToList();

            props = props.Where(x => x != null).ToList();

            string refId = (props.IndexOf(props.Find(x => x.Name == bHoMprop.Name)) + 1000).ToString();

            return refId;*/
        }

        /***************************************************/
    }
}




