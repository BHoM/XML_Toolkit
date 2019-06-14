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
using BHP = BH.oM.Environment.Fragments;
using BHX = BH.oM.XML;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;
using BH.Engine.Environment;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        public static BHX.Opening ToGBXML(this BHE.Opening opening)
        {
            BHX.Opening gbOpening = new BHX.Opening();

            BHG.Polyline pLine = opening.ToPolyline();

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

        public static BHX.Opening ToGBXML(this BHE.Opening opening, List<BHE.Panel> space, BHX.Enums.ExportType exportType = BHX.Enums.ExportType.gbXMLTAS)
        {
            BHX.Opening gbOpening = opening.ToGBXML();

            BHG.Polyline pLine = opening.ToPolyline();
            if (pLine.NormalAwayFromSpace(space))
                gbOpening.PlanarGeometry.PolyLoop = pLine.Flip().ToGBXML();

            BHP.OriginContextFragment contextProperties = opening.FindFragment<BHP.OriginContextFragment>(typeof(BHP.OriginContextFragment));

            if (contextProperties != null)
            {
                string elementID = contextProperties.ElementID;
                string familyName = contextProperties.TypeName;

                gbOpening.CADObjectID = opening.CADObjectID(exportType);
                gbOpening.OpeningType = opening.Type.ToGBXML();

                if (familyName == "System Panel") //No SAM_BuildingElementType for this one atm
                    gbOpening.OpeningType = "FixedWindow";

                if (gbOpening.OpeningType.Contains("Window") && opening.OpeningConstruction.Name.Contains("SLD")) //Change windows with SLD construction into doors
                    gbOpening.OpeningType = "NonSlidingDoor";
            }

            return gbOpening;
        }

        public static BHE.Opening ToBHoM(this BHX.Opening gbOpening)
        {
            BHE.Opening opening = new BHE.Opening();

            BHG.Polyline pLine = gbOpening.PlanarGeometry.PolyLoop.ToBHoM();
            opening.Edges = pLine.ToEdges();

            string[] cadSplit = gbOpening.CADObjectID.Split('[');
            if (cadSplit.Length > 0)
                opening.Name = cadSplit[0].Trim();

            return opening;
        }
    }
}