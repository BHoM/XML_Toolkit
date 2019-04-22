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

using BH.oM.Environment.Materials;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        public static string ToGBXML(this AbsorptanceType aType)
        {
            switch (aType)
            {
                case AbsorptanceType.InfraredExternal:
                    return "ExtIR";
                case AbsorptanceType.SolarExternal:
                    return "ExtSolar";
                case AbsorptanceType.TotalExternal:
                    return "ExtTotal";
                case AbsorptanceType.VisibleExternal:
                    return "ExtVisible";
                case AbsorptanceType.InfraredInternal:
                    return "IntIR";
                case AbsorptanceType.SolarInternal:
                    return "IntSolar";
                case AbsorptanceType.TotalInternal:
                    return "IntTotal";
                case AbsorptanceType.VisibleInternal:
                    return "IntVisible";
                default:
                    return "ExtIR";
            }
        }

        public static AbsorptanceType ToBHoMAbsorptanceType(this string aType)
        {
            if (aType.Equals("ExtIR"))
                return AbsorptanceType.InfraredExternal;
            if (aType.Equals("ExtSolar"))
                return AbsorptanceType.SolarExternal;
            if (aType.Equals("ExtTotal"))
                return AbsorptanceType.TotalExternal;
            if (aType.Equals("ExtVisible"))
                return AbsorptanceType.VisibleExternal;
            if (aType.Equals("IntIR"))
                return AbsorptanceType.InfraredInternal;
            if (aType.Equals("IntSolar"))
                return AbsorptanceType.SolarInternal;
            if (aType.Equals("IntTotal"))
                return AbsorptanceType.TotalInternal;
            if (aType.Equals("IntVisible"))
                return AbsorptanceType.VisibleInternal;

            return AbsorptanceType.Undefined;
        }
    }
}