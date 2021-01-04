/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
using BHE = BH.oM.Environment.Elements;

using BHC = BH.oM.Physical.Constructions;

using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Adapters.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Gets the unique construction ID for an Environment Panel")]
        [Input("panel", "An Environment Panel with a construction")]
        [Output("contstructionID", "The unique ID for the construction on the panel")]
        public static string ConstructionID(this BHE.Panel panel)
        {
            if (panel == null || panel.Construction == null) return null;
            return panel.Construction.ConstructionID();
        }

        [Description("Gets the unique construction ID for a construction")]
        [Input("construction", "An instance of a construction implementing the Physical IConstruction interface")]
        [Output("contstructionID", "The unique ID for the construction")]
        public static string ConstructionID(this BHC.IConstruction construction)
        {
            if (construction == null) return null;
            return ConstructionID(construction as dynamic);
        }

        [Description("Gets the unique construction ID for a Physical construction")]
        [Input("construction", "An instance of a Physical construction")]
        [Output("contstructionID", "The unique ID for the construction")]
        public static string ConstructionID(this BHC.Construction construction)
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






