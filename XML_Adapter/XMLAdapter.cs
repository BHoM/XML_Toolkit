using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.Engine;
using BH.oM.Base;
using System.Reflection;

using BH.oM.XML.Enums;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        public XMLAdapter(String xmlFileName = "BHoM_gbXML_Export", String xmlDirectoryPath = null, ExportType exportType = ExportType.gbXMLTAS)
        {
            FilePath = (xmlDirectoryPath == null ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : xmlDirectoryPath);
            FileName = xmlFileName;
            ExportType = exportType;

            AdapterId = "XML_Adapter";
            Config.MergeWithComparer = false;   //Set to true after comparers have been implemented
            Config.ProcessInMemory = false;
            Config.SeparateProperties = false;  //Set to true after Dependency types have been implemented
            Config.UseAdapterId = false;        //Set to true when NextId method and id tagging has been implemented
        }

        public override List<IObject> Push(IEnumerable<IObject> objects, String tag = "", Dictionary<String, object> config = null)
        {
            bool success = true;

            MethodInfo methodInfos = typeof(Enumerable).GetMethod("Cast");
            foreach(var typeGroup in objects.GroupBy(x => x.GetType()))
            {
                MethodInfo mInfo = methodInfos.MakeGenericMethod(new[] { typeGroup.Key });
                var list = mInfo.Invoke(typeGroup, new object[] { typeGroup });
                success &= Create(list as dynamic, false);
            }

            return success ? objects.ToList() : new List<IObject>();
        }

        private String FilePath { get; set; }
        private String FileName { get; set; }
        private ExportType ExportType { get; set; }
    }
}
