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

        public static List<BHE.BuildingElement> ReplaceBuildingElement(this List<BHE.BuildingElement> oldBuildingElements, List<BHE.BuildingElement> newBuildingElements)
        {
            List<BHE.BuildingElement> bElement = new List<oM.Environment.Elements.BuildingElement>();

            foreach (Guid guid in newBuildingElements.Select(x => x.BHoM_Guid)) 
            {
                if (oldBuildingElements.Select(x => x.BHoM_Guid).Contains(guid))
                {
                    bElement.Add(newBuildingElements.Find(x => x.BHoM_Guid == guid));
                    bElement.AddRange(oldBuildingElements.FindAll(x => x.BHoM_Guid != guid));
                }
                else
                    bElement.AddRange(oldBuildingElements.FindAll(x => x.BHoM_Guid != guid));
            }

            return bElement;
        }
        /***************************************************/

        public static BHE.Building UpdateBuildingElement(this List<BHE.BuildingElement> bes, BHE.Building building)
        {
            //BHE.Building newBuilding = building.GetShallowClone() as BHE.Building;


            //Update the spaces
            /*foreach (BHE.BuildingElement be in bes)
            {
                BHE.Space space = building.Spaces.Find(x => x.BHoM_Guid == be.AdjacentSpaces.FirstOrDefault());

                BHE.BuildingElement toRemove = space.BuildingElements.Find(x => x.BHoM_Guid == be.BHoM_Guid);

                space.BuildingElements.Remove(toRemove);
                space.BuildingElements.Add(be);

                //Update the building
                BHE.Space spaceToRemove = building.Spaces.Find(x => x.BHoM_Guid == space.BHoM_Guid);
                building.Spaces.Remove(spaceToRemove);
                building.Add(space);
            }*/

            return building;
        }

        /***************************************************/
    }
}




