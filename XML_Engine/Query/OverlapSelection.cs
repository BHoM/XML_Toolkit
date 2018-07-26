//using System;
//using System.Collections.Generic;
//using System.Linq;
//using BH.oM.XML;
//using BH.oM.Base;
//using BHE = BH.oM.Environment.Elements;
//using BHP = BH.oM.Environment.Properties;
//using BHG = BH.oM.Geometry;
//using BH.Engine.Geometry;
//using BH.Engine.Environment;


//namespace BH.Engine.XML
//{

//    public static partial class Query
//    {
//        /***************************************************/
//        /**** Public Methods                            ****/
//        /***************************************************/

//        //This is the check if the surfaces have the correct adjacency number

//        public static List<BHE.BuildingElement> OverlapSelection(this List<BHE.BuildingElement> bHoMBuildingElements)
//        {
//            List<BHE.BuildingElement> selection = new List<BHE.BuildingElement>();

//            if (bHoMBuildingElements.Count == 0 || bHoMBuildingElements.Count > 2)
//                return null;
//            if (bHoMBuildingElements.Count == 1)
//                return bHoMBuildingElements;

//            //Check if the elements are the same
//            if (bHoMBuildingElements[0].ToGbXMLType() == bHoMBuildingElements[1].ToGbXMLType())
//            {
//                //if (bHoMBuildingElements[0].ToGbXMLType() == "Air"))
//                // look at CAD object ID and set type from this

//                selection.Add(bHoMBuildingElements[0]);
//            }

//            else if (bHoMBuildingElements.Select(x => x.ToGbXMLType()).Contains("Air"))
//                selection.Add(bHoMBuildingElements.Find(x => x.ToGbXMLType() != "Air"));

//            //if shade: keep the other element and change type to external
//            else if (bHoMBuildingElements.Select(x => x.ToGbXMLType()).Contains("Shade"))
//            {
//                BHE.BuildingElement updatedElement = bHoMBuildingElements.Find(x => x.ToGbXMLType() != "Shade");
//                if (updatedElement.ToGbXMLType() == "InteriorWall")
//                    updatedElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "External Wall";


//                if (updatedElement.ToGbXMLType() == "InteriorFloor")
//                    updatedElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "External Floor";

//                //Update CADobjectID:
//                updatedElement.CustomData["Revit_elementId"] = bHoMBuildingElements.Find(x => x.ToGbXMLType() == "Shade").CustomData["Revit_elementId"].ToString();
//                updatedElement.CustomData["Family Name"] = bHoMBuildingElements.Find(x => x.ToGbXMLType() == "Shade").BuildingElementProperties.CustomData["Family Name"].ToString();
//                selection.Add(updatedElement);
//            }

//            //Check Adjacencies
//            List<Guid> adj1 = bHoMBuildingElements[0].AdjacentSpaces;
//            List<Guid> adj2 = bHoMBuildingElements[1].AdjacentSpaces;
//            List<Guid> newAdjSpaces = new List<Guid>();

//            //Check if they contain the same information
//            if (adj1.Count == 1 && adj2.Count == 1 && adj1[0].ToString() == adj2[0].ToString())
//                newAdjSpaces = adj1;
//            else if (adj1.Count == 1 && adj2.Count == 1 && adj1[0].ToString() != adj2[0].ToString())
//            {
//                newAdjSpaces.AddRange(adj1);
//                newAdjSpaces.AddRange(adj2);
//            }
//            else if (adj1.Count > adj2.Count)
//                newAdjSpaces = adj1;
//            else
//                newAdjSpaces = bHoMBuildingElements.SelectMany(x => x.AdjacentSpaces).ToList();

//            selection[0].AdjacentSpaces.AddRange(newAdjSpaces);

//            return selection;


//            /***************************************************/
//        }
//    }
//}



