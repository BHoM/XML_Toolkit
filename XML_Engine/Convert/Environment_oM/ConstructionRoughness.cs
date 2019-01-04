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
using System.Text;
using System.Threading.Tasks;

using BHE = BH.oM.Environment.Elements;
using BHP = BH.oM.Environment.Properties;
using BHX = BH.oM.XML;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;
using BH.Engine.Environment;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        public static BHX.Roughness ToGBXML(this BHE.ConstructionRoughness roughness)
        {
            BHX.Roughness r = new BHX.Roughness();

            switch (roughness)
            {
                case BHE.ConstructionRoughness.MediumRough:
                    r.Value = "MediumRough";
                    break;
                case oM.Environment.Elements.ConstructionRoughness.MediumSmooth:
                    r.Value = "MediumSmooth";
                    break;
                case oM.Environment.Elements.ConstructionRoughness.Rough:
                    r.Value = "Rough";
                    break;
                case oM.Environment.Elements.ConstructionRoughness.Smooth:
                    r.Value = "Smooth";
                    break;
                case oM.Environment.Elements.ConstructionRoughness.VeryRough:
                    r.Value = "VeryRough";
                    break;
                case oM.Environment.Elements.ConstructionRoughness.VerySmooth:
                    r.Value = "VerySmooth";
                    break;
            }

            return r;
        }

        public static BHE.ConstructionRoughness ToBHoM(this BHX.Roughness roughness)
        {
            if (roughness.Value.Equals("MediumRough"))
                return BHE.ConstructionRoughness.MediumRough;
            if (roughness.Value.Equals("MediumSmooth"))
                return BHE.ConstructionRoughness.MediumSmooth;
            if (roughness.Value.Equals("Rough"))
                return BHE.ConstructionRoughness.Rough;
            if (roughness.Value.Equals("Smooth"))
                return BHE.ConstructionRoughness.Smooth;
            if (roughness.Value.Equals("VeryRough"))
                return BHE.ConstructionRoughness.VeryRough;
            if (roughness.Value.Equals("VerySmooth"))
                return BHE.ConstructionRoughness.VerySmooth;

            return BHE.ConstructionRoughness.Undefined;
        }
    }
}

