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

    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static BHE.BuildingElement PropertiesFromAdjSrf(this BHE.BuildingElement bHoMBuildingElement, List<BHE.BuildingElement> refElement)
        {
            BHE.BuildingElement newBe = bHoMBuildingElement.GetShallowClone() as BHE.BuildingElement;

            if (refElement.Count == 0)
                return newBe;

            if (refElement.Count == 1) //Get properites for CADObjectID and Type from this element. No selection needed. TODO: add method for selection where we have many adjacent surfaces. 
            {
                if (refElement[0].BuildingElementProperties != null)
                {
                    //1. Properties for CADObjectID
                    if (refElement[0].BuildingElementProperties != null && newBe.BuildingElementProperties != null)
                        newBe.BuildingElementProperties.Name = refElement[0].BuildingElementProperties.Name;
                     if (newBe.BuildingElementProperties == null)
                    {
                        newBe.BuildingElementProperties = new BHP.BuildingElementProperties();
                        newBe.BuildingElementProperties.Name = refElement[0].BuildingElementProperties.Name;
                    }


                    if (refElement[0].CustomData.ContainsKey("Revit_elementId"))
                    {
                        if (newBe.CustomData.ContainsKey("Revit_elementId"))
                            newBe.CustomData["Revit_elementId"] = refElement[0].CustomData["Revit_elementId"];
                        else
                            newBe.CustomData.Add("Revit_elementId", refElement[0].CustomData["Revit_elementId"]);
                    }

                    if (refElement[0].BuildingElementProperties.CustomData.ContainsKey("Family Name"))
                    {
                        if (newBe.CustomData.ContainsKey("Family Name"))
                            newBe.BuildingElementProperties.CustomData["Family Name"] = refElement[0].CustomData["Family Name"];
                        else
                            newBe.BuildingElementProperties.CustomData.Add("Family Name", refElement[0].CustomData["Family Name"]);
                    }

                    //2.Type
                    if (refElement[0].BuildingElementProperties.CustomData.ContainsKey("SAM_BuildingElementType"))
                    {
                        if (newBe.BuildingElementProperties.CustomData.ContainsKey("SAM_BuildingElementType"))
                            newBe.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = refElement[0].BuildingElementProperties.CustomData["SAM_BuildingElementType"];
                        else
                            newBe.BuildingElementProperties.CustomData.Add("SAM_BuildingElementType", refElement[0].BuildingElementProperties.CustomData["SAM_BuildingElementType"]);
                    }
                }
            }
            return newBe;
        }

        /***************************************************/
    }
}




