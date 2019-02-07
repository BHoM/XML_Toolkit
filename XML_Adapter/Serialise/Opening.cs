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

using BHP = BH.oM.Environment.Properties;

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

            foreach (BH.oM.Environment.Elements.Opening opening in openings)
            {
                if (opening.OpeningCurve == null) continue;

                BH.oM.XML.Opening gbOpening = BH.Engine.XML.Convert.ToGBXML(opening);

                //Normals away from space
                if (!BH.Engine.Environment.Query.NormalAwayFromSpace(opening.OpeningCurve.ICollapseToPolyline(BH.oM.Geometry.Tolerance.Angle), space))
                    gbOpening.PlanarGeometry.PolyLoop = BH.Engine.XML.Convert.ToGBXML(opening.OpeningCurve.ICollapseToPolyline(BH.oM.Geometry.Tolerance.Angle).Flip());

                BuildingElement buildingElement = new BuildingElement();
                

                if (opening.ElementProperties() != null && opening.EnvironmentContextProperties() != null)
                {

                    BHP.ElementProperties elementProperties = opening.ElementProperties() as BHP.ElementProperties;
                    BHP.EnvironmentContextProperties contextProperties = opening.EnvironmentContextProperties() as BHP.EnvironmentContextProperties;

                    string elementID = contextProperties.ElementID;
                    string familyName = contextProperties.TypeName;

                    gbOpening.CADObjectID = opening.CADObjectID(exportType);
                    gbOpening.OpeningType = elementProperties.ToGBXMLType();

                    if (familyName == "System Panel") //No SAM_BuildingElementType for this one atm
                        gbOpening.OpeningType = "FixedWindow";

                    if (exportType == ExportType.gbXMLIES && gbOpening.OpeningType.Contains("Window") && elementProperties.Construction.Name.Contains("SLD")) //Change windows with SLD construction into doors for IES
                        gbOpening.OpeningType = "NonSlidingDoor";

                    if (exportType == ExportType.gbXMLIES)
                        gbOpening.WindowTypeIDRef = BH.Engine.XML.Query.GetCleanName(elementProperties.Construction.Name);
                    else
                        gbOpening.WindowTypeIDRef = null;
                }

                gbOpening.ConstructionIDRef = null; //ToDo review having this property on an opening?
                gbOpenings.Add(gbOpening);
            }

            return gbOpenings;
        }
    }
}