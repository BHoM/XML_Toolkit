using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace XML_Adapter
{
    public class XMLWriter
    {

        public static string Save(string filePath, string Name, BH.oM.XML.GBXML gbx)
        {
            try
            {
                Name += ".xml";
                XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
                XmlSerializer szer = new XmlSerializer(typeof(BH.oM.XML.GBXML));
                TextWriter ms = new StreamWriter(Path.Combine(filePath, Name));
                szer.Serialize(ms, gbx, xns);
                ms.Close();
                return "Written Ok";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }


}
