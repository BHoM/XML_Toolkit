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
using BHI = BH.oM.Environment.Interface;
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

        public static string ConstructionID(this BHE.BuildingElement element)
        {
            if (element == null || element.ElementProperties() == null) return null;
            return element.ElementProperties().ConstructionID();
        }

        public static string ConstructionID(this BHI.IBHoMExtendedProperties props)
        {
            if (props == null) return null;
            BHP.ElementProperties properties = props as BHP.ElementProperties;
            if (properties == null) return null;
            return ConstructionID(properties);
        }

        public static string ConstructionID(this BHP.ElementProperties props)
        {
            if (props.Construction == null) return null;
            return props.Construction.ConstructionID();
        }

        public static string ConstructionID(this BHE.Construction construction)
        {
            //Method for constructing an ID based on the name of the property - this allows the same string ID to be generated for the same property for consistency in finding string IDs

            //Originally we used the GUID and got the combinations below - but each name is unique so each returned string ID will be unique anyway - but the following comment line is being left in as a nice little factoid for the next person... (//TD)
            //Using the first 8 digits of the GUID gives 218,340,105,584,896 possible combinations of IDs, so the liklihood of 2 different GUIDs producing the same result from this function is fairly small...

            if (construction.Name == "") return null;

            String rtnID = construction.Name[0].ToString();

            for (int x = 1; x < construction.Name.Length; x++)
                rtnID += ((int)construction.Name[x]).ToString();

            return rtnID;
        }
    }
}




