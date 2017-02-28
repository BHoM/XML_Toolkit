using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XML_Adapter;
using BHoM.Base;
using System.Xml.Serialization;


namespace XML_Adapter
{


    public class gbXMLSerializer
    {
        public static gbXML Serialize(List<string> Input)
        {
            gbXML bhomGbXML = new gbXML();
            bhomGbXML.input = Input;
            return bhomGbXML;
        }
    }
}
