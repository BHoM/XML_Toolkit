using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHE = BH.oM.Environmental;

namespace XML_Adapter.gbXML
{
    public static partial class Convert
    {
        /***************************************************/

        public static string ToGbXMLSurfaceType(BHE.Elements.BuildingElementPanel bHoMPanel)
        {

            if (bHoMPanel.ElementType == "EXTERNALWALL")
                return "ExteriorWall";
            else if (bHoMPanel.ElementType == "INTERNALWALL")
                return "InteriorWall";
            else if (bHoMPanel.ElementType == "ROOFELEMENT")
                return "Roof";
            else if (bHoMPanel.ElementType == "INTERNALFLOOR")
                return "InteriorFloor";
            else if (bHoMPanel.ElementType == "EXPOSEDFLOOR")
                return "ExposedFloor";
            else if (bHoMPanel.ElementType == "SHADEELEMENT")
                return "Shade";
            else if (bHoMPanel.ElementType == "UNDERGROUNDWALL")
                return "UndergroundWall";
            else if (bHoMPanel.ElementType == "UNDERGROUNDSLAB")
                return "UndergroundSlab";
            else if (bHoMPanel.ElementType == "CEILING")
                return "Ceiling";
            else if (bHoMPanel.ElementType == "UNDERGROUNDCEILING")
                return "UndergroundCeiling";
            else if (bHoMPanel.ElementType == "RAISEDFLOOR")
                return "RaisedFloor";
            else if (bHoMPanel.ElementType == "SLABONGRADE")
                return "SlabOnGrade";
            else
                return ""; //Adiabatic


        }

        /***************************************************/
    }
}
