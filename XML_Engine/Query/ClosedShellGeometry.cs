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
            List<BH.oM.XML.Polyloop> ploopsShell = new List<Polyloop>();

            List<BHG.Polyline> mergedPolyLines = BH.Engine.Environment.Query.ClosedShellGeometry(bHoMSpace);

<<<<<<< HEAD
            //2. Add the rest of the geometries
            mergedPlines.AddRange(pLinesOther);

            // 3. Ensure that all of the surface coordinates are listed in a counterclockwise order.
            // This is a requirement of GBXML Polyloop definitions. If this is inconsistent or wrong we end up with a corrupt GBXML file.
            foreach (BHG.Polyline pline in mergedPlines)
            {
                if (!BH.Engine.Environment.Query.NormalAwayFromSpace(pline, bHoMSpace))
                    ploopsShell.Add(BH.Engine.XML.Convert.ToGBXML(pline.Flip()));
=======
            //Ensure that all of the surface coordinates are listed in a counterclockwise order.
            //This is a requirement of gbXML Polyloop definitions. If this is inconsistent or wrong we end up with a corrupt gbXML file.
            foreach (BHG.Polyline pline in mergedPolyLines)
            {
                if (!BH.Engine.Environment.Query.NormalAwayFromSpace(pline, bHoMSpace))
                    ploopsShell.Add(BH.Engine.XML.Convert.ToGbXML(pline.Flip()));
>>>>>>> Moving a host of methods over to the Environment Engine
                else
                    ploopsShell.Add(BH.Engine.XML.Convert.ToGBXML(pline));
            }

            return ploopsShell;
        }
    }
}




