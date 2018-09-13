using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace BH.Adapter.XML
{
    public class XMLReader
    {
        public static BH.oM.XML.GBXML Load(string filePath, string Name)
        {
            Name += ".xml";
            TextReader reader = new StreamReader(Path.Combine(filePath, Name));
            XmlSerializer szer = new XmlSerializer(typeof(BH.oM.XML.GBXML));
            return (BH.oM.XML.GBXML)szer.Deserialize(reader);
        }
    }
}
