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
    public class XMLReader
    {

        public static BH.oM.XML.gbXML Load(string filePath, string Name)
        {
            Name += ".xml";
            TextReader reader = new StreamReader(Path.Combine(filePath, Name));
            XmlSerializer szer = new XmlSerializer(typeof(BH.oM.XML.gbXML));
            return (BH.oM.XML.gbXML)szer.Deserialize(reader);
        }
    }
}
