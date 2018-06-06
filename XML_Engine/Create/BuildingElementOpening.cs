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
    //TODO: move these methods to the Environment Engine

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

        public static BHE.BuildingElementOpening BuildingElementOpening(BHG.ICurve curve)
        {
            return BuildingElementOpening(curve as dynamic);
        }

        /***************************************************/

        public static BHE.BuildingElementOpening BuildingElementOpening(IEnumerable<BHG.Polyline> polylines)
        {
            return new BHE.BuildingElementOpening
            {
                PolyCurve = Geometry.Create.PolyCurve(polylines)
            };
        }
        /***************************************************/


        public static BHE.BuildingElementOpening BuildingElementOpening(BHG.Polyline polyline)
        {
            return new BHE.BuildingElementOpening
            {
                PolyCurve = Geometry.Create.PolyCurve(new BHG.Polyline[] { polyline })
            };
        }

        /***************************************************/

        public static BHE.BuildingElement BuildingElementOpening(this BHE.BuildingElement be, BHG.ICurve bound)
        {
            if (be == null || be.BuildingElementProperties == null || !be.BuildingElementProperties.CustomData.ContainsKey("Revit_elementId"))
                return be;

            string revitElementID = (be.BuildingElementProperties.CustomData["Revit_elementId"]).ToString();
            BHG.PolyCurve pCrv = bound as BHG.PolyCurve;
            BHE.BuildingElementOpening opening = BuildingElementOpening(pCrv);

            BHE.BuildingElementPanel panel = be.BuildingElementGeometry as BHE.BuildingElementPanel;
            if (panel == null) // if be isn't of type buildingElementPanel
                return be;

            //Use the same properties as the wall
            opening.Name = be.Name;
            opening.CustomData.Add("Revit_elementId", revitElementID);

            panel.Openings.Add(opening);
            be.BuildingElementGeometry = panel;

            return be;
        }

        /***************************************************/

        public static BHE.BuildingElement BuildingElementOpening(this BHE.BuildingElement be, List<BHG.ICurve> bounds)
        {
            if (be == null || be.BuildingElementProperties == null || !be.BuildingElementProperties.CustomData.ContainsKey("Revit_elementId"))
                return be;

            BHE.BuildingElementPanel panel = be.BuildingElementGeometry as BHE.BuildingElementPanel;
            if (panel == null) // if be isn't of type buildingElementPanel
                return be;

            foreach (BHG.ICurve bound in bounds)
            {
                string revitElementID = (be.BuildingElementProperties.CustomData["Revit_elementId"]).ToString();
                //BHG.PolyCurve pCrv = bound as BHG.PolyCurve; //How can I cast a pl to a pc?
               
                BHE.BuildingElementOpening opening = BuildingElementOpening(bound);

                //Use the same properties as the wall
                opening.Name = be.Name;
                opening.CustomData.Add("Revit_elementId", revitElementID);

                panel.Openings.Add(opening);
                be.BuildingElementGeometry = panel;
            }
            return be;
        }

        /***************************************************/

    }
}
