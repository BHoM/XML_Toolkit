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
        public static string ToGBXML(this BHE.AbsorptanceType aType)
        {
            switch (aType)
            {
                case BHE.AbsorptanceType.ExtIR:
                    return "ExtIR";
                case BHE.AbsorptanceType.ExtSolar:
                    return "ExtSolar";
                case BHE.AbsorptanceType.ExtTotal:
                    return "ExtTotal";
                case BHE.AbsorptanceType.ExtVisible:
                    return "ExtVisible";
                case BHE.AbsorptanceType.IntIR:
                    return "IntIR";
                case BHE.AbsorptanceType.IntSolar:
                    return "IntSolar";
                case BHE.AbsorptanceType.IntTotal:
                    return "IntTotal";
                case BHE.AbsorptanceType.IntVisible:
                    return "IntVisible";
                default:
                    return "ExtIR";
            }
        }

        public static BHE.AbsorptanceType ToBHoMAbsorptanceType(this string aType)
        {
            if (aType.Equals("ExtIR"))
                return BHE.AbsorptanceType.ExtIR;
            if (aType.Equals("ExtSolar"))
                return BHE.AbsorptanceType.ExtSolar;
            if (aType.Equals("ExtTotal"))
                return BHE.AbsorptanceType.ExtTotal;
            if (aType.Equals("ExtVisible"))
                return BHE.AbsorptanceType.ExtVisible;
            if (aType.Equals("IntIR"))
                return BHE.AbsorptanceType.IntIR;
            if (aType.Equals("IntSolar"))
                return BHE.AbsorptanceType.IntSolar;
            if (aType.Equals("IntTotal"))
                return BHE.AbsorptanceType.IntTotal;
            if (aType.Equals("IntVisible"))
                return BHE.AbsorptanceType.IntVisible;

            return BHE.AbsorptanceType.Undefined;
        }
    }
}