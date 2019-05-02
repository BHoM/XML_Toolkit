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

using BH.oM.Environment.MaterialFragments;

using BHX = BH.oM.XML;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        public static BHX.Roughness ToGBXML(this Roughness roughness)
        {
            BHX.Roughness r = new BHX.Roughness();

            switch (roughness)
            {
                case Roughness.MediumRough:
                    r.Value = "MediumRough";
                    break;
                case Roughness.MediumSmooth:
                    r.Value = "MediumSmooth";
                    break;
                case Roughness.Rough:
                    r.Value = "Rough";
                    break;
                case Roughness.Smooth:
                    r.Value = "Smooth";
                    break;
                case Roughness.VeryRough:
                    r.Value = "VeryRough";
                    break;
                case Roughness.VerySmooth:
                    r.Value = "VerySmooth";
                    break;
            }

            return r;
        }

        public static Roughness ToBHoM(this BHX.Roughness roughness)
        {
            if (roughness.Value.Equals("MediumRough"))
                return Roughness.MediumRough;
            if (roughness.Value.Equals("MediumSmooth"))
                return Roughness.MediumSmooth;
            if (roughness.Value.Equals("Rough"))
                return Roughness.Rough;
            if (roughness.Value.Equals("Smooth"))
                return Roughness.Smooth;
            if (roughness.Value.Equals("VeryRough"))
                return Roughness.VeryRough;
            if (roughness.Value.Equals("VerySmooth"))
                return Roughness.VerySmooth;

            return Roughness.Undefined;
        }
    }
}

