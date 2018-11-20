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

        public List<BHoME.Building> Buildings { get; set; } = new List<BHoME.Building>();
        public List<List<BHoME.BuildingElement>> ElementsAsSpaces { get; set; } = new List<List<BHoME.BuildingElement>>();
        public List<BHoME.BuildingElement> ShadingElements { get; set; } = new List<BHoME.BuildingElement>();
        public List<BHoME.Space> Spaces { get; set; } = new List<BHoME.Space>();
        public List<BH.oM.Architecture.Elements.Level> Levels { get; set; } = new List<Architecture.Elements.Level>();
        public List<BHoME.BuildingElement> Openings { get; set; } = new List<BHoME.BuildingElement>();

        /***************************************************/
    }
}
