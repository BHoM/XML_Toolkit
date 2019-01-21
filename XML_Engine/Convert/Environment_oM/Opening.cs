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
            BHX.Opening jsonOpening = new BHX.Opening();

            BHG.Polyline pLine = new oM.Geometry.Polyline() { ControlPoints = opening.OpeningCurve.IControlPoints() };

            jsonOpening.Name = "opening-" + opening.BHoM_Guid.ToString().Replace("-", "").Substring(0, 5);
            jsonOpening.ID = "opening-" + opening.BHoM_Guid.ToString().Replace("-", "").Substring(0, 5);
            jsonOpening.PlanarGeometry.PolyLoop = pLine.ToGBXML();
            jsonOpening.PlanarGeometry.ID = "openingPGeom-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            jsonOpening.RectangularGeometry.CartesianPoint = Geometry.Query.Centre(pLine).ToGBXML();
            jsonOpening.RectangularGeometry.Height = Math.Round(opening.Height(), 3);
            jsonOpening.RectangularGeometry.Width = Math.Round(opening.Width(), 3);
            jsonOpening.RectangularGeometry.ID = "rGeomOpening-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

            return jsonOpening;
        }

        public static BHX.Opening ToJson(this BHE.Opening opening, List<BHE.BuildingElement> space, List<BHE.BuildingElement> allElements, List<List<BHE.BuildingElement>> spaces, List<BHE.Space> spaceSpaces)
        {
            BHX.Opening jsonOpening = opening.ToGBXML();

            BHG.Polyline pLine = new BHG.Polyline() { ControlPoints = opening.OpeningCurve.IControlPoints() };
            if (pLine.NormalAwayFromSpace(space))
                jsonOpening.PlanarGeometry.PolyLoop = pLine.Flip().ToGBXML();

            BHE.BuildingElement buildingElement = new BHE.BuildingElement();
            string familyName = "";
            string typeName = "";

            if (opening.CustomData.ContainsKey("Revit_elementId"))
            {
                string elementID = (opening.CustomData["Revit_elementId"]).ToString();
                buildingElement = allElements.Find(x => x != null && x.CustomData.ContainsKey("Revit_elementId") && x.CustomData["Revit_elementId"].ToString() == elementID);

                if (buildingElement != null)
                {
                    if (buildingElement.BuildingElementProperties.CustomData.ContainsKey("Family Name"))
                    {
                        familyName = buildingElement.BuildingElementProperties.CustomData["Family Name"].ToString();
                        typeName = buildingElement.BuildingElementProperties.Name;
                    }

                    jsonOpening.CADObjectID = buildingElement.CADObjectID();
                    jsonOpening.OpeningType = buildingElement.ToGBXMLType(buildingElement.AdjacentSpaces(spaces, spaceSpaces));

                    if (familyName == "System Panel") //No SAM_BuildingElementType for this one atm
                        jsonOpening.OpeningType = "FixedWindow";

                    if (jsonOpening.OpeningType.Contains("Window") && buildingElement.BuildingElementProperties.Name.Contains("SLD")) //Change windows with SLD construction into doors
                        jsonOpening.OpeningType = "NonSlidingDoor";
                }
            }

            return jsonOpening;
        }

        public static BHE.Opening ToBHoM(this BHX.Opening opening)
        {
            BHE.Opening bhomOpening = new BHE.Opening();

            BHG.Polyline pLine = opening.PlanarGeometry.PolyLoop.ToBHoM();
            bhomOpening.OpeningCurve = pLine;

            return bhomOpening;
        }
    }
}