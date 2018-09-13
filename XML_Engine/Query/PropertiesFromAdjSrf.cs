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
        public static BHE.BuildingElement PropertiesFromAdjSrf(this BHE.BuildingElement bHoMBuildingElement, List<BHE.BuildingElement> refElement, BHE.Building building)
        {
            BHE.BuildingElement newBe = new BHE.BuildingElement();
            newBe = bHoMBuildingElement.GetShallowClone() as BHE.BuildingElement;

            if (refElement == null || refElement.Count == 0)
                return newBe;

            //if (refElement.Count == 1) //Get properites for CADObjectID and Type from this element. No selection needed. TODO: add method for selection where we have many adjacent surfaces. 
            //{
            //    if (refElement[0].BuildingElementProperties != null && refElement[0].AdjacentSpaces.Count == newBe.AdjacentSpaces.Count)
            //    {
            //        //1. Properties for CADObjectID
            //        if (refElement[0].BuildingElementProperties != null && newBe.BuildingElementProperties != null)
            //            newBe.BuildingElementProperties.Name = refElement[0].BuildingElementProperties.Name;
            //        if (newBe.BuildingElementProperties == null)
            //        {
            //            newBe.BuildingElementProperties = new BHP.BuildingElementProperties();
            //            newBe.BuildingElementProperties.Name = refElement[0].BuildingElementProperties.Name;
            //        }


            //        if (refElement[0].CustomData.ContainsKey("Revit_elementId"))
            //        {
            //            if (newBe.CustomData.ContainsKey("Revit_elementId"))
            //                newBe.CustomData["Revit_elementId"] = refElement[0].CustomData["Revit_elementId"];
            //            else
            //                newBe.CustomData.Add("Revit_elementId", refElement[0].CustomData["Revit_elementId"]);
            //        }

            //        if (refElement[0].BuildingElementProperties.CustomData.ContainsKey("Family Name"))
            //        {
            //            if (newBe.CustomData.ContainsKey("Family Name"))
            //                newBe.BuildingElementProperties.CustomData["Family Name"] = refElement[0].CustomData["Family Name"];
            //            else
            //                newBe.BuildingElementProperties.CustomData.Add("Family Name", refElement[0].CustomData["Family Name"]);
            //        }

            //        //2.Type
            //        if (refElement[0].BuildingElementProperties.CustomData.ContainsKey("SAM_BuildingElementType"))
            //        {
            //            if (newBe.BuildingElementProperties.CustomData.ContainsKey("SAM_BuildingElementType"))
            //                newBe.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = refElement[0].BuildingElementProperties.CustomData["SAM_BuildingElementType"];
            //            else
            //                newBe.BuildingElementProperties.CustomData.Add("SAM_BuildingElementType", refElement[0].BuildingElementProperties.CustomData["SAM_BuildingElementType"]);
            //        }
            //    }


            //}

            //if (refElement.Count > 1) // We need to do a selection when we have more than one adjacent surface
            //{
            //Choose the surface with the same adjacent space (or spaces). What if the adjacencies are wrong?

            BHE.BuildingElement be2 = refElement.Where(x => x.AdjacentSpaces.Count == bHoMBuildingElement.AdjacentSpaces.Count && x.AdjacentSpaces.AdjacencyMatch(bHoMBuildingElement.AdjacentSpaces)).FirstOrDefault();


            //1. Test with be2. If this fails - use be3. 
            if (be2 == null)
                be2 = refElement.Where(x => x.AdjacentSpaces.Count == bHoMBuildingElement.AdjacentSpaces.Count && x.AdjacentSpaces.AdjacencyIsIn(bHoMBuildingElement.AdjacentSpaces)).FirstOrDefault();

            if (be2 != null)
            {
                //1. Properties for CADObjectID
                if (be2.BuildingElementProperties != null && newBe.BuildingElementProperties != null)
                    newBe.BuildingElementProperties.Name = be2.BuildingElementProperties.Name;
                if (newBe.BuildingElementProperties == null)
                {
                    newBe.BuildingElementProperties = new BHP.BuildingElementProperties();
                    newBe.BuildingElementProperties.Name = be2.BuildingElementProperties.Name;
                }


                if (be2.CustomData.ContainsKey("Revit_elementId"))
                {
                    if (newBe.CustomData.ContainsKey("Revit_elementId"))
                        newBe.CustomData["Revit_elementId"] = be2.CustomData["Revit_elementId"];
                    else
                        newBe.CustomData.Add("Revit_elementId", be2.CustomData["Revit_elementId"]);
                }

                if (be2.BuildingElementProperties.CustomData.ContainsKey("Family Name"))
                {
                    if (newBe.CustomData.ContainsKey("Family Name"))
                        newBe.BuildingElementProperties.CustomData["Family Name"] = be2.CustomData["Family Name"];
                    else
                        newBe.BuildingElementProperties.CustomData.Add("Family Name", be2.CustomData["Family Name"]);
                }

                //2.Type
                if (be2.BuildingElementProperties.CustomData.ContainsKey("SAM_BuildingElementType"))
                {
                    if (newBe.BuildingElementProperties.CustomData.ContainsKey("SAM_BuildingElementType"))
                        newBe.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = be2.BuildingElementProperties.CustomData["SAM_BuildingElementType"];
                    else
                        newBe.BuildingElementProperties.CustomData.Add("SAM_BuildingElementType", be2.BuildingElementProperties.CustomData["SAM_BuildingElementType"]);
                }

            }
            else
            {
                //Find the space for the building element that we want to update.
                BHE.Space space = building.Spaces.Find(x => x.BHoM_Guid.ToString() == bHoMBuildingElement.AdjacentSpaces.First().ToString());

                BHE.BuildingElement be3 = space.BuildingElements.Find((x => x.AdjacentSpaces.Count == 1 && x.BHoM_Guid != bHoMBuildingElement.BHoM_Guid && x.BuildingElementGeometry.Tilt() == newBe.BuildingElementGeometry.Tilt()));

                if (be3 != null)
                {

                    //1. Properties for CADObjectID
                    if (be3.BuildingElementProperties != null && newBe.BuildingElementProperties != null)
                        newBe.BuildingElementProperties.Name = be3.BuildingElementProperties.Name;
                    if (newBe.BuildingElementProperties == null)
                    {
                        newBe.BuildingElementProperties = new BHP.BuildingElementProperties();
                        newBe.BuildingElementProperties.Name = be3.BuildingElementProperties.Name;
                    }


                    if (be3.CustomData.ContainsKey("Revit_elementId"))
                    {
                        if (newBe.CustomData.ContainsKey("Revit_elementId"))
                            newBe.CustomData["Revit_elementId"] = be3.CustomData["Revit_elementId"];
                        else
                            newBe.CustomData.Add("Revit_elementId", be3.CustomData["Revit_elementId"]);
                    }

                    if (be3.BuildingElementProperties.CustomData.ContainsKey("Family Name"))
                    {
                        if (newBe.CustomData.ContainsKey("Family Name"))
                            newBe.BuildingElementProperties.CustomData["Family Name"] = be3.CustomData["Family Name"];
                        else
                            newBe.BuildingElementProperties.CustomData.Add("Family Name", be3.CustomData["Family Name"]);
                    }

                    //2.Type
                    if (be3.BuildingElementProperties.CustomData.ContainsKey("SAM_BuildingElementType"))
                    {
                        if (newBe.BuildingElementProperties.CustomData.ContainsKey("SAM_BuildingElementType"))
                            newBe.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = be3.BuildingElementProperties.CustomData["SAM_BuildingElementType"];
                        else
                            newBe.BuildingElementProperties.CustomData.Add("SAM_BuildingElementType", be3.BuildingElementProperties.CustomData["SAM_BuildingElementType"]);
                    }
                }

            }



            //}
            return newBe;
        }

        public static bool AdjacencyIsIn(this List<Guid> g, List<Guid> b)
        {
            bool match = false;
            foreach (Guid a in g)
                if (b.Contains(a))
                    match = true;
            return match;
        }

        public static bool AdjacencyMatch(this List<Guid> g, List<Guid> b)
        {
            bool match = true;
            foreach (Guid a in g)
                match &= b.Contains(a);
            return match;
        }
        /***************************************************/
    }
}




