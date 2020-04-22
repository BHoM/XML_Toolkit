/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using BHX = BH.oM.External.XML.GBXML;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;

using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Adapter.XML
{
    public static partial class Convert
    {
        [Description("Get the GBXML representation of a BHoM Point")]
        [Input("pt", "The BHoM Geometry Point to convert into a GBXML Cartesian Point")]
        [Output("cartesianPoint", "The GBXML representation of a BHoM Point")]
        public static BHX.CartesianPoint ToGBXML(this BHG.Point pt)
        {
            BHX.CartesianPoint cartPoint = new BHX.CartesianPoint();
            List<string> coord = new List<string>();

            coord.Add(Math.Round(pt.X, 4).ToString());
            coord.Add(Math.Round(pt.Y, 4).ToString());
            coord.Add(Math.Round(pt.Z, 4).ToString());

            cartPoint.Coordinate = coord.ToArray();

            return cartPoint;
        }

        [Description("Get the BHoM representation of a GBXML Point")]
        [Input("pt", "The GBXML Cartesian Point to convert into a BHoM Geometry Point")]
        [Output("bhomPoint", "The BHoM representation of a GBXML Point")]
        public static BHG.Point FromGBXML(this BHX.CartesianPoint pt)
        {
            BHG.Point bhomPt = new BHG.Point();
            try
            {
                bhomPt.X = (pt.Coordinate.Length >= 1 ? System.Convert.ToDouble(pt.Coordinate[0]) : 0);
                bhomPt.Y = (pt.Coordinate.Length >= 2 ? System.Convert.ToDouble(pt.Coordinate[1]) : 0);
                bhomPt.Z = (pt.Coordinate.Length >= 3 ? System.Convert.ToDouble(pt.Coordinate[2]) : 0);
            }
            catch { }

            return bhomPt;
        }
    }
}
