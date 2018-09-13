using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

using BH.oM.Base;

namespace BH.Adapter.XML
{
    public class XMLWriter
    {

        public static string Save(string filePath, string Name, BH.oM.XML.GBXML gbx)
        {
            try
            {
                System.Reflection.PropertyInfo[] bhomProperties = typeof(BHoMObject).GetProperties();
                XmlAttributeOverrides overrides = new XmlAttributeOverrides();

                foreach(System.Reflection.PropertyInfo pi in bhomProperties)
                    overrides.Add(typeof(BHoMObject), pi.Name, new XmlAttributes { XmlIgnore = true });

                Name += ".xml";
                XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
                XmlSerializer szer = new XmlSerializer(typeof(BH.oM.XML.GBXML), overrides);
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
