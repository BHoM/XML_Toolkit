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
using BH.oM.Base;
using BHE = BH.oM.Environment.Elements;
using BHP = BH.oM.Environment.Properties;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.Engine.Environment;

namespace BH.Engine.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static BHE.Panel FlipReversedPanels(this BHE.Panel panel, BHE.Space space)
        {
            /* Ensure that all of the surface coordinates are listed in a counterclockwise order. This is a requirement of GBXML Polyloop definitions */

            BHG.Polyline pline = new BHG.Polyline() { ControlPoints = panel.PanelCurve.IControlPoints() };
            BHG.Polyline bound = new BHG.Polyline();

            if (!BH.Engine.Geometry.Query.IsClockwise(pline, space.Centre()))
                bound = pline.Flip();
            else
                bound = pline;
            

            BHE.Panel pan = BH.Engine.Environment.Create.Panel(bound);
            return pan;

            /***************************************************/
        }
    }
}




