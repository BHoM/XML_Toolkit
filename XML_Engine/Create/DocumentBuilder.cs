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

        public static DocumentBuilder DocumentBuilder(Building building = null, List<List<BuildingElement>> elementsAsSpaces = null, List<Space> spaces = null)
        {
            return new DocumentBuilder
            {
                Building = building,
                ElementsAsSpaces = elementsAsSpaces,
                Spaces = spaces,
            };         
        }
    }
}