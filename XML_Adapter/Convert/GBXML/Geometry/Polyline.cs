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
using BHX = BH.oM.XML;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;

using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Adapter.XML
{
    public static partial class Convert
    {
        [Description("Get the GBXML representation of a BHoM Polyline")]
        [Input("pLine", "The BHoM Geometry Point to convert into a GBXML Polyloop")]
        [Input("tolerance", "The tolerance to determine whether the polyline is closed or not, default to BH.oM.Geometry.Tolerance.Distance")]
        [Output("polyloop", "The GBXML representation of a BHoM Polyline")]
        public static BHX.Polyloop ToGBXML(this BHG.Polyline pLine, double tolerance = BHG.Tolerance.Distance)
        {
            BHX.Polyloop polyloop = new BHX.Polyloop();

            pLine = pLine.CleanPolyline(minimumSegmentLength: tolerance);

            List<BHG.Point> pts = pLine.DiscontinuityPoints();
            if (pts.Count == 0) return polyloop;

            int count = ((pts.First().SquareDistance(pts.Last()) < (tolerance * tolerance)) ? pts.Count - 1 : pts.Count);
            List<BHX.CartesianPoint> cartpoint = new List<BHX.CartesianPoint>();
            for (int i = 0; i < count; i++)
            {
                BHX.CartesianPoint cpt = pts[i].ToGBXML();
                List<string> coord = new List<string>();
                cartpoint.Add(cpt);
            }
            polyloop.CartesianPoint = cartpoint.ToArray();
            return polyloop;
        }

        [Description("Get the BHoM representation of a GBXML Polyloop")]
        [Input("pLoop", "The GBXML Polyloop to convert into a BHoM Polyline")]
        [Input("tolerance", "The tolerance to determine whether the polyline is closed or not, default to BH.oM.Geometry.Tolerance.Distance")]
        [Output("polyline", "The BHoM representation of a GBXML Polyloop")]
        public static BHG.Polyline FromGBXML(this BHX.Polyloop pLoop, double tolerance = BHG.Tolerance.Distance)
        {
            BHG.Polyline pLine = new BHG.Polyline();

            List<BHX.CartesianPoint> pts = pLoop.CartesianPoint.ToList();

            List<BHG.Point> bhomPts = new List<BHG.Point>();
            for (int x = 0; x < pts.Count; x++)
                bhomPts.Add(pts[x].FromGBXML());

            if (bhomPts.First().SquareDistance(bhomPts.Last()) > (tolerance * tolerance))
                bhomPts.Add(bhomPts.First());

            pLine.ControlPoints = bhomPts;
            return pLine;
        }
    }
}

