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

using BH.oM.Geometry;

using BHE = BH.oM.Environment.Elements;
using BH.Engine.Geometry;
using BH.Engine.Environment;

namespace BH.Engine.XML
{
    public static partial class Query
    {
        public static Point PolygonCentre(this Polyline pLine, double tolerance = Tolerance.Distance)
        {
            //Determine if the polyline is anti-clockwise first - if it isn't then flip it...
            if (pLine.IsClockwise())
                pLine = pLine.Flip();

            double area = pLine.Area();

            int total = pLine.ControlPoints.Count - 1;

            double xSum = 0;
            double ySum = 0;
            double zSum = 0;

            for(int x = 0; x < total; x++)
            {
                Point p1 = pLine.ControlPoints[x];
                Point p2 = pLine.ControlPoints[x + 1];
                double secondSum = ((p1.X * p2.Y) - (p2.X * p1.Y));
                xSum += (p1.X + p2.X) * secondSum;
                ySum += (p1.Y + p2.Y) * secondSum;
                zSum += (p1.Z + p2.Z) * ((p1.X * p2.Z) - (p2.X * p1.Z));
            }

            double sixA = 6 * area;
            double div = 1 / sixA;
            double xFinal = div * xSum;
            double yFinal = div * ySum;
            double zFinal = div * zSum;

            return new Point() { X = xFinal, Y = yFinal, Z = zFinal };
        }

        public static bool IsClockwise(this Polyline pLine)
        {
            double result = 0;

            int total = pLine.ControlPoints.Count - 1;
            for(int x = 0; x < total; x++)
            {
                Point p1 = pLine.ControlPoints[x];
                Point p2 = pLine.ControlPoints[x + 1];
                double x1 = (p2.X - p1.X);
                double y1 = (p2.Y + p1.Y);
                result += (x1 * y1);
            }

            if (result > 0)
                return true; //Clockwise
            else
                return false; //Counter-clockwise
        }
    }
}
