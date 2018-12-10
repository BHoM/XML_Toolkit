using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Base;
using BHE = BH.oM.Environment;
using BH.oM.XML;
using BH.oM.XML.Enums;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            string fileName = FileName;
            switch(ExportType)
            {
                case ExportType.gbXMLIES:
                    fileName += "_IES";
                    break;
                case ExportType.gbXMLTAS:
                    fileName += "_TAS";
                    break;
                default :
                    fileName += "";
                    break;
            }

            GBXML gbx = new GBXML();
            if (typeof(IBHoMObject).IsAssignableFrom(typeof(T)))
            {
                XML.GBXMLSerializer.Serialize(objects, gbx, ExportType);
                XMLWriter.Save(FilePath, fileName, gbx);
            }

            return true;
        }

        /***************************************************/

        /*private bool Create(BHE.Elements.Panel bHoMPanel, BHX.GBXML gbx, bool isIES)
        {
            XML.GBXMLSerializer.Serialize(new List<IBHoMObject> { bHoMPanel as IBHoMObject }, gbx, isIES);

            return true;
        }*/

        /***************************************************/

        private bool Create(BHE.Elements.Space bHoMSpace, GBXML gbx, ExportType exportType)
        {
            XML.GBXMLSerializer.Serialize(new List<IBHoMObject> { bHoMSpace as IBHoMObject }, gbx, exportType);

            return true;
        }

        /***************************************************/

        private bool Create(BHE.Elements.Building bHoMBuilding, GBXML gbx, ExportType exportType)
        {
            XML.GBXMLSerializer.Serialize(new List<IBHoMObject> { bHoMBuilding as IBHoMObject }, gbx, exportType);

            return true;
        }

        /***************************************************/

        private bool Create(BHE.Elements.BuildingElement bHoMBuildingElement, GBXML gbx, ExportType exportType)
        {
            XML.GBXMLSerializer.Serialize(new List<IBHoMObject> { bHoMBuildingElement as IBHoMObject }, gbx, exportType);

            return true;
        }
    }
}