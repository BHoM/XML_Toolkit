using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.XML;
using BH.oM.Base;
using BHE = BH.oM.Environmental.Elements;
using BHP = BH.oM.Environmental.Properties;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.Engine.Environment;


namespace BH.Engine.XML
{

    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static BHE.BuildingElementOpening BuildingElementOpening(BHG.PolyCurve polyCurve)
        {
            return new BHE.BuildingElementOpening
            {
                PolyCurve = polyCurve
            };
        }

        /***************************************************/

        public static BHE.BuildingElement BuildingElementOpening(this BHE.BuildingElement be, List<BHG.PolyCurve> polyCurve)
        {

            string revitElementID = (be.BuildingElementProperties.CustomData["Revit_elementId"]).ToString();

            foreach (BHG.PolyCurve pCrv in polyCurve)
            {
                BHE.BuildingElementOpening opening = BuildingElementOpening(pCrv);

                //Use the same properties as the wall
                opening.Name = be.Name;
                opening.CustomData.Add("Revit_elementId", revitElementID);

                (be.BuildingElementGeometry as BHE.BuildingElementPanel).Openings.Add(opening);
            }

            return be;
        }

        /***************************************************/

        public static BHE.BuildingElement BuildingElementOpening(this BHE.BuildingElement be, BHG.Polyline pline)
        {

            string revitElementID = (be.BuildingElementProperties.CustomData["Revit_elementId"]).ToString();
            BHG.PolyCurve pCrv = be.BuildingElementGeometry.ICurve() as BHG.PolyCurve;

            BHE.BuildingElementOpening opening = BuildingElementOpening(pCrv);

            //Use the same properties as the wall
            opening.Name = be.Name;
            opening.CustomData.Add("Revit_elementId", revitElementID);

            (be.BuildingElementGeometry as BHE.BuildingElementPanel).Openings.Add(opening);

            return be;
        }

        /***************************************************/

        public static BHE.BuildingElement BuildingElementOpening(this BHE.BuildingElement be, BHG.ICurve bound)
        {

            string revitElementID = (be.BuildingElementProperties.CustomData["Revit_elementId"]).ToString();
            BHG.PolyCurve pCrv = bound as BHG.PolyCurve;

            BHE.BuildingElementOpening opening = BuildingElementOpening(pCrv);

            //Use the same properties as the wall
            opening.Name = be.Name;
            opening.CustomData.Add("Revit_elementId", revitElementID);

            (be.BuildingElementGeometry as BHE.BuildingElementPanel).Openings.Add(opening);

            return be;
        }

        /***************************************************/

        public static BHE.BuildingElementPanel BuildingElementOpening(this BHE.BuildingElementPanel panel, BHE.BuildingElement be, BHG.ICurve bound)
        {

            string revitElementID = (be.BuildingElementProperties.CustomData["Revit_elementId"]).ToString();
            BHG.PolyCurve pCrv = bound as BHG.PolyCurve;

            BHE.BuildingElementOpening opening = BuildingElementOpening(pCrv);

            //Use the same properties as the wall
            opening.Name = be.Name;
            opening.CustomData.Add("Revit_elementId", revitElementID);

            panel.Openings.Add(opening);

            return panel;
        }

        /***************************************************/
    }
}
