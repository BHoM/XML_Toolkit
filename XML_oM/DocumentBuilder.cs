using System;
using System.Collections.Generic;
using BH.oM.Base;
using BHoME = BH.oM.Environment.Elements;

namespace BH.oM.XML.Environment
{
    public class DocumentBuilder : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public BHoME.Building Building { get; set; } = new BHoME.Building();
        public List<List<BHoME.BuildingElement>> ElementsAsSpaces { get; set; } = new List<List<BHoME.BuildingElement>>();
        public List<BHoME.Space> Spaces { get; set; } = new List<BHoME.Space>();

        /***************************************************/
    }
}
