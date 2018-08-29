using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Base;
using BH.oM.Environment.Elements;
using BH.oM.Environment.Properties;
using BH.oM.Environment.Interface;
using BHG = BH.oM.Geometry;
using BH.Engine;
using BHE = BH.oM.Environment;
using XML_Adapter;

namespace BH.Adapter.GBXML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        protected override IEnumerable<IBHoMObject> Read(Type type, IList indices = null)
        {
            if (type == typeof(Space))
                return ReadSpaces();
            if (type == typeof(BuildingElementPanel))
                return ReadPanels();
            return null;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<Space> ReadSpaces(List<String> ids = null)
        {
            BH.oM.XML.GBXML gbx = XMLReader.Load(FilePath, FileName);
            IEnumerable<IBHoMObject> bHoMObject = GBXMLDeserializer.Deserialize(gbx);
            return bHoMObject.Where(x => x is BHE.Elements.Space).Cast<Space>().ToList();
        }

        /***************************************************/

        private List<BuildingElementPanel> ReadPanels(List<string> ids = null)
        {
            BH.oM.XML.GBXML gbx = XMLReader.Load(FilePath, FileName);
            IEnumerable<IBHoMObject> bHoMObject = GBXMLDeserializer.Deserialize(gbx);
            return bHoMObject.Where(x => x is BHE.Elements.BuildingElementPanel).Cast<BuildingElementPanel>().ToList();
        }
    }
}