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

using BH.oM.Environment.Elements;
using System;
using System.Collections.Generic;

using System.Linq;
using BH.Engine.Environment;

using BH.oM.XML;
using BH.Engine.XML;

using BH.oM.Geometry;
using BH.Engine.Geometry;

using BH.oM.XML.Enums;

namespace BH.Adapter.XML
{
    public partial class GBXMLSerializer
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static List<BH.oM.XML.Opening> Serialize(IEnumerable<BH.oM.Environment.Elements.Opening> openings, List<BuildingElement> space, List<BuildingElement> allElements, List<List<BuildingElement>> spaces, List<BH.oM.Environment.Elements.Space> spaceSpaces, BH.oM.XML.GBXML gbx, ExportType exportType)
        {
            List<BH.oM.XML.Opening> gbOpenings = new List<oM.XML.Opening>();

            int openingCount = 0;
            foreach (BH.oM.Environment.Elements.Opening opening in openings)
            {
                if (opening.OpeningCurve == null) continue;

                BH.oM.XML.Opening gbOpening = BH.Engine.XML.Convert.ToGBXML(opening);
                gbOpening.PlanarGeometry.ID = "openingPGeom" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);

                //Normals away from space
                if (!BH.Engine.Environment.Query.NormalAwayFromSpace(opening.OpeningCurve.ICollapseToPolyline(BH.oM.Geometry.Tolerance.Angle), space))
                    gbOpening.PlanarGeometry.PolyLoop = BH.Engine.XML.Convert.ToGBXML(opening.OpeningCurve.ICollapseToPolyline(BH.oM.Geometry.Tolerance.Angle).Flip());

                BuildingElement buildingElement = new BuildingElement();
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

                        gbOpening.CADObjectID = BH.Engine.XML.Query.CadObjectId(opening, allElements, exportType);
                        gbOpening.OpeningType = BH.Engine.XML.Convert.ToGBXMLType(buildingElement, BH.Engine.Environment.Query.AdjacentSpaces(buildingElement, spaces, spaceSpaces), exportType);

                        if (familyName == "System Panel") //No SAM_BuildingElementType for this one atm
                            gbOpening.OpeningType = "FixedWindow";

                        if (exportType == ExportType.gbXMLIES && gbOpening.OpeningType.Contains("Window") && buildingElement.BuildingElementProperties.Name.Contains("SLD")) //Change windows with SLD construction into doors for IES
                            gbOpening.OpeningType = "NonSlidingDoor";
                    }
                }

                gbOpening.ID = "opening-" + openingCount.ToString() + "-" + opening.BHoM_Guid.ToString().Replace("-", "").Substring(0, 5);
                gbOpening.Name = "opening-" + openingCount.ToString();
                openingCount++;
                if (exportType == ExportType.gbXMLIES)
                    gbOpening.ConstructionIDRef = BH.Engine.XML.Query.IdRef(buildingElement); //Only for IES!
                else
                    gbOpening.ConstructionIDRef = null;

                gbOpenings.Add(gbOpening);
            }

            return gbOpenings;
        }
    }
}