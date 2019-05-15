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
    public static partial class Modify
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Polyline CleanPolyline(this Polyline pline)
        {
            List<Point> pnts = pline.DiscontinuityPoints();

            if (pnts.Count < 3) return pline; //If there's only two points here then this method isn't necessary

            int startIndex = 0;
            while(startIndex < pnts.Count)
            {
                Point first = pnts[startIndex];
                Point second = pnts[(startIndex + 1) % pnts.Count];
                Point third = pnts[(startIndex + 2) % pnts.Count];

                double angle = first.Angle(second, third);

                if (angle <= BH.oM.Geometry.Tolerance.Angle)
                {
                    //Delete the second point from the list, it's not necessary
                    pnts.RemoveAt((startIndex + 1) % pnts.Count);
                }
                else
                    startIndex++; //Move onto the next point
            }

            Polyline pLine = new Polyline()
            {
                ControlPoints = pnts,
            };

            return pLine;
        }
    }
}
