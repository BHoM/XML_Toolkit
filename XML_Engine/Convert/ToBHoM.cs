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
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods - Geometry                 ****/
        /***************************************************/

        public static BHG.Point ToBHoM(CartesianPoint cartesianPoint)
        {
            List<double> coords = new List<double>();
            foreach (String s in cartesianPoint.Coordinate)
                coords.Add(System.Convert.ToDouble(s));

            for (int x = coords.Count; x < 3; x++)
                coords.Add(0); //Add additional elements in case the cartesian point had less than 3 points

            return BH.Engine.Geometry.Create.Point(coords[0], coords[1], coords[2]);
        }

        /***************************************************/

        public static BHG.Polyline ToBHoM(Polyloop ploop)
        {
            List<BHG.Point> pts = new List<BH.oM.Geometry.Point>();
            if (1 <= ploop.CartesianPoint.Count())
            {
                foreach (CartesianPoint Cpt in ploop.CartesianPoint)
                {
                    pts.Add(ToBHoM(Cpt));
                }
                pts.Add((BHG.Point)pts[0].Clone());
            }
            BHG.Polyline pline = BH.Engine.Geometry.Create.Polyline(pts);
            return pline;
        }



        /***************************************************/
    }
}
