/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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

using BH.oM.Geometry;
using BH.Engine.Geometry;

namespace BH.Engine.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static double Angle(this Point endPt1, Point connectingPt, Point endPt2)
        {
            double x1 = endPt1.X - connectingPt.X; //Vector 1 - x
            double y1 = endPt1.Y - connectingPt.Y; //Vector 1 - y
            double z1 = endPt1.Z - connectingPt.Z; //Vector 1 - z
            double sqr1 = (x1 * x1) + (y1 * y1) + (z1 * z1); //Square of vector 1
            double x2 = endPt2.X - connectingPt.X; //Vector 2 - x
            double y2 = endPt2.Y - connectingPt.Y; //Vector 2 - y
            double z2 = endPt2.Z - connectingPt.Z; //Vector 2 - z
            double sqr2 = (x2 * x2) + (y2 * y2) + (z2 * z2); //Square of vector 2

            double xa = x1 * x2;
            double ya = y1 * y2;
            double za = z1 * z2;

            double costr = (xa + ya + za) / Math.Sqrt(Math.Abs(sqr1 * sqr2));
            double angle = Math.Abs(Math.Acos(costr)); //This produces a result in radians
            angle = angle * 180 / Math.PI; //This converts the result into degrees
            return 180 - angle; //Convert so that a flat line is 0 angle through the points
        }
    }
}
