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

using BH.oM.Base;
using BHE = BH.oM.Environment.Elements;
using BHP = BH.oM.Environment.Fragments;
using BHX = BH.oM.XML;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;
using BH.Engine.Environment;
using BH.oM.XML.Settings;

using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        [Description("Get the GBXML representation of a BHoM Environments Opening")]
        [Input("opening", "The BHoM Environments Opening to convert into a GBXML Opening")]
        [Output("openingGBXML", "The GBXML representation of a BHoM Environment Opening")]
        public static BHX.Opening ToGBXML(this BHE.Opening opening)
        {
            BHX.Opening gbOpening = new BHX.Opening();

            BHG.Polyline pLine = opening.Polyline();

            gbOpening.Name = opening.Name;
            gbOpening.ID = "opening" + opening.BHoM_Guid.ToString().Replace("-", "").Substring(0, 5);
            gbOpening.PlanarGeometry.PolyLoop = pLine.ToGBXML();
            gbOpening.PlanarGeometry.ID = "openingPGeom-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);
            gbOpening.RectangularGeometry.CartesianPoint = Geometry.Query.Centre(pLine).ToGBXML();
            gbOpening.RectangularGeometry.Height = Math.Round(opening.Height(), 3);
            //TODO: temporary solution to get valid file to be replaced with correct height
            if (opening.Height() == 0)
               gbOpening.RectangularGeometry.Height = 0.1;
            gbOpening.RectangularGeometry.Width = Math.Round(opening.Width(), 3);
            //if (opening.Width() == 0)
            //    gbOpening.RectangularGeometry.Width = 0.1;
            gbOpening.RectangularGeometry.ID = "rGeomOpening-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);

            return gbOpening;
        }

        [Description("Get the BHoM Environments representation of a GBXML Opening")]
        [Input("opening", "The GBXML Opening to convert into a BHoM Environments Opening")]
        [Output("openingBHoM", "The BHoM representation of a GBXML Opening")]
        public static BHE.Opening FromGBXML(this BHX.Opening gbOpening)
        {
            BHE.Opening opening = new BHE.Opening();

            BHG.Polyline pLine = gbOpening.PlanarGeometry.PolyLoop.FromGBXML();
            opening.Edges = pLine.ToEdges();

            string[] cadSplit = gbOpening.CADObjectID.Split('[');
            if (cadSplit.Length > 0)
                opening.Name = cadSplit[0].Trim();
            if (cadSplit.Length > 1)
            {
                BHP.OriginContextFragment envContext = new BHP.OriginContextFragment();
                envContext.ElementID = cadSplit[1].Split(']')[0].Trim();
                envContext.TypeName = opening.Name;

                if (opening.Fragments == null) opening.Fragments = new FragmentSet();
                opening.Fragments.Add(envContext);
            }

            opening.Type = gbOpening.OpeningType.FromGBXMLOpeningType();

            return opening;
        }
    }
}