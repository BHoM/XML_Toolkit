using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.XML;
using BH.oM.Base;
using BHE = BH.oM.Environment.Elements;
using BHP = BH.oM.Environment.Properties;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.Engine.Environment;

namespace BH.Engine.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static List<BH.oM.XML.Polyloop> ClosedShellGeometry(this BHE.Space bHoMSpace)
        {
            /*List<BH.oM.XML.Polyloop> ploopsShell = new List<Polyloop>();

            List<BHG.Polyline> mergedPolyLines = BH.Engine.Environment.Query.ClosedShellGeometry(bHoMSpace);

            //Ensure that all of the surface coordinates are listed in a counterclockwise order.
            //This is a requirement of gbXML Polyloop definitions. If this is inconsistent or wrong we end up with a corrupt gbXML file.
            foreach (BHG.Polyline pline in mergedPolyLines)
            {
                if (!BH.Engine.Environment.Query.NormalAwayFromSpace(pline, bHoMSpace))
                    ploopsShell.Add(BH.Engine.XML.Convert.ToGBXML(pline.Flip()));
                else
                    ploopsShell.Add(BH.Engine.XML.Convert.ToGBXML(pline));
            }

            return ploopsShell;*/
            return null;
        }

        public static List<Polyloop> ClosedShellGeometry(this List<BHE.BuildingElement> spaceBoundaries)
        {
            List<Polyloop> shell = new List<Polyloop>();

            List<BHG.Polyline> polylines = BH.Engine.Environment.Query.ClosedShellGeometry(spaceBoundaries);

            //Ensure that all of the surface coordinates are listed in a clockwise order
            foreach(BHG.Polyline pLine in polylines)
            {
                if (BH.Engine.Environment.Query.NormalAwayFromSpace(pLine, spaceBoundaries))
                    shell.Add(BH.Engine.XML.Convert.ToGBXML(pLine));
                else
                    shell.Add(BH.Engine.XML.Convert.ToGBXML(pLine.Flip()));
            }

            return shell;
        }
    }
}




