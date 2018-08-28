using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Base;
using BHE = BH.oM.Environment;
using BHX = BH.oM.XML;
using XML_Adapter;

namespace BH.Adapter.gbXML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            BHX.gbXML gbx = new BHX.gbXML();

            if(typeof(IBHoMObject).IsAssignableFrom(typeof(T)))
            {
                gbXML.gbXMLSerializer.Serialize(objects, gbx, false);
                XMLWriter.Save(FilePath, FileName + "_TAS", gbx);
                gbx = new BHX.gbXML();
                gbXML.gbXMLSerializer.Serialize(objects, gbx, true);
                XMLWriter.Save(FilePath, FileName + "_IES", gbx);
            }

            return true;
        }

        /***************************************************/

        private bool Create(BHE.Elements.BuildingElementPanel bHoMBuildingElementPanel, BHX.gbXML gbx, bool isIES)
        {
            gbXML.gbXMLSerializer.Serialize(new List<IBHoMObject> { bHoMBuildingElementPanel as IBHoMObject }, gbx, isIES);

            return true;
        }

        /***************************************************/

        private bool Create(BHE.Elements.Space bHoMSpace, BHX.gbXML gbx, bool isIES)
        {
            gbXML.gbXMLSerializer.Serialize(new List<IBHoMObject> { bHoMSpace as IBHoMObject }, gbx, isIES);

            return true;
        }

        /***************************************************/

        private bool Create(BHE.Elements.Building bHoMBuilding, BHX.gbXML gbx, bool isIES)
        {
            gbXML.gbXMLSerializer.Serialize(new List<IBHoMObject> { bHoMBuilding as IBHoMObject }, gbx, isIES);

            return true;
        }

        /***************************************************/

        private bool Create(BHE.Elements.BuildingElement bHoMBuildingElement, BHX.gbXML gbx, bool isIES)
        {
            gbXML.gbXMLSerializer.Serialize(new List<IBHoMObject> { bHoMBuildingElement as IBHoMObject }, gbx, isIES);

            return true;
        }
    }
}
