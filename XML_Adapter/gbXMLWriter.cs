using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace XML_Adapter
{
    public class gbXMLWriter
    {
        public static void Save(string filePath, string Name, gbXML gbx)
        {
            Name += ".xml";
            XmlSerializer sr = new XmlSerializer(gbx.GetType());
            TextWriter writer = new StreamWriter(Path.Combine(filePath, Name));
            sr.Serialize(writer, gbx);
            writer.Close();
        }
    }
    //public class gbXMLReader
    //{


    //    public static LoadFromFile(string filePath, string Name)
    //    {
    //        using (FileStream stream = new FileStream(filePath, FileMode.Create))
    //        {
    //            var XML = new XmlSerializer(typeof(BHoMgbXML));
    //            return (BHoMgbXML)XML.Deserialize(stream);
    //        }

    //    }

    //}
}
