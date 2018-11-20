using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Environment.Elements;
using BH.oM.XML.Environment;

namespace BH.Engine.XML
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static DocumentBuilder DocumentBuilder(List<Building> building = null, List<List<BuildingElement>> elementsAsSpaces = null, List<BuildingElement> shadingElements = null, List<Space> spaces = null, List<BH.oM.Architecture.Elements.Level> levels = null, List<BuildingElement> openings = null)
        {
            return new DocumentBuilder
            {
                Buildings = building,
                ElementsAsSpaces = elementsAsSpaces,
                ShadingElements = shadingElements,
                Spaces = spaces,
                Levels = levels,
                Openings = openings,
            };         
        }
    }
}