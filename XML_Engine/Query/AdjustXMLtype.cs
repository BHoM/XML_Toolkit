/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

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

        //TODO: We are updating the reference atm. Make sure we clone the object and modify the copy!

        public static BHE.BuildingElement AdjustXMLType(this BHE.BuildingElement buildingElement)
        {

            BHE.BuildingElement newBuildingElement = buildingElement.GetShallowClone() as BHE.BuildingElement;
           

            /*if (newBuildingElement.BuildingElementProperties == null) //Use geometry
            {
                newBuildingElement.BuildingElementProperties = new oM.Environment.Properties.BuildingElementProperties();
                if (buildingElement.AdjacentSpaces.Count == 0)
                    newBuildingElement.BuildingElementProperties.CustomData.Add("SAM_BuildingElementType", "Shade");

                else if (buildingElement.AdjacentSpaces.Count == 1)
                {
                    if (Environment.Query.Tilt(buildingElement.BuildingElementGeometry) >= 70 && Environment.Query.Tilt(buildingElement.BuildingElementGeometry) <= 120)
                        newBuildingElement.BuildingElementProperties.CustomData.Add("SAM_BuildingElementType", "External Wall");
                    else //If not wall it is a floor
                        newBuildingElement.BuildingElementProperties.CustomData.Add("SAM_BuildingElementType", "Exposed Floor");
                }
                else if (buildingElement.AdjacentSpaces.Count == 2)
                {
                    if (Environment.Query.Tilt(buildingElement.BuildingElementGeometry) >= 70 && Environment.Query.Tilt(buildingElement.BuildingElementGeometry) <= 120)
                        newBuildingElement.BuildingElementProperties.CustomData.Add("SAM_BuildingElementType", "Internal Wall");
                    else //If not wall it is a floor
                        newBuildingElement.BuildingElementProperties.CustomData.Add("SAM_BuildingElementType","Internal Floor");
                }
                return newBuildingElement; //Throwing a null reference exception on line below if BEP is equal to null stops execution of anything else
            }


            newBuildingElement.BuildingElementProperties.CustomData = new Dictionary<string, object>(buildingElement.BuildingElementProperties.CustomData);

            if (buildingElement.AdjacentSpaces.Count == 0 && buildingElement.BuildingElementProperties.CustomData.ContainsKey("SAM_BuildingElementType")) //Shade
                newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "Shade";

            else if (buildingElement.AdjacentSpaces.Count == 1 && buildingElement.BuildingElementProperties.CustomData.ContainsKey("SAM_BuildingElementType"))// External
            {
                if (buildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"].ToString() == "Internal Wall")
                    newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "External Wall";
                else if (buildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"].ToString() == "Internal Floor")
                    newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "Exposed Floor";
                else if (buildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"].ToString() == "Air" && buildingElement.BuildingElementProperties.BuildingElementType == BHE.BuildingElementType.Wall) //External surfaces cannot be air
                    newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "External Wall";
                else if (buildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"].ToString() == "Air" && buildingElement.BuildingElementProperties.BuildingElementType == BHE.BuildingElementType.Floor) //External surfaces cannot be air
                    newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "Exposed Floor";
            }


            else if (buildingElement.AdjacentSpaces.Count == 2 && buildingElement.BuildingElementProperties.CustomData.ContainsKey("SAM_BuildingElementType")) // Internal
            {
                if (buildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"].ToString() == "Roof")
                    newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "Ceiling";
                else if (buildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"].ToString() == "External Wall")
                    newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "Internal Wall";
                else if (buildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"].ToString() == "External Floor")
                    newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "Internal Floor";
                else if (buildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"].ToString() == "Raised Floor")
                    newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "Internal Floor";
                else if (buildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"].ToString() == "Curtain Wall")
                    newBuildingElement.BuildingElementProperties.CustomData["SAM_BuildingElementType"] = "Internal Wall";
            }*/

            return newBuildingElement;


        }

        /***************************************************/

        public static List<BHE.BuildingElement> AdjustXMLType(this List<BHE.BuildingElement> bHoMBuildingElement)
        {
            /*List<BHE.BuildingElement> bHoMBuildingElements = new List<oM.Environment.Elements.BuildingElement>();

            foreach (BHE.BuildingElement element in bHoMBuildingElement)
            {
                BHE.BuildingElement newBuildingElement = element.GetShallowClone() as BHE.BuildingElement;
                bHoMBuildingElements.Add(AdjustXMLType(newBuildingElement));
            }

            return bHoMBuildingElements;*/
            List<BHE.BuildingElement> b = new List<BHE.BuildingElement>(bHoMBuildingElement);

            //foreach(BHE.BuildingElement be in b)
            for (int x = 0; x < b.Count; x++)
            {
                b[x] = AdjustXMLType(b[x]);
            }

            return b;
        }

        /***************************************************/
    }
}



