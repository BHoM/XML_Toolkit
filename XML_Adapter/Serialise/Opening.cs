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

        public static List<BH.oM.XML.Opening> Serialize(IEnumerable<BH.oM.Environment.Elements.Opening> openings, List<Panel> space, List<Panel> allElements, List<List<Panel>> spaces, BH.oM.XML.GBXML gbx, ExportType exportType)
        {
            List<BH.oM.XML.Opening> gbOpenings = new List<oM.XML.Opening>();

            foreach (BH.oM.Environment.Elements.Opening opening in openings)
            {
                if (opening.ToPolyline() == null) continue;

                BH.oM.XML.Opening gbOpening = BH.Engine.XML.Convert.ToGBXML(opening);

                //Normals away from space
                if (!BH.Engine.Environment.Query.NormalAwayFromSpace(opening.ToPolyline(), space))
                    gbOpening.PlanarGeometry.PolyLoop = BH.Engine.XML.Convert.ToGBXML(opening.ToPolyline().Flip());

                Panel buildingElement = new Panel();
;
                BHP.OriginContextFragment contextProperties = opening.FindFragment<BHP.OriginContextFragment>(typeof(BHP.OriginContextFragment));

                string elementID = "";
                string familyName = "";
                if (contextProperties != null)
                {
                    elementID = contextProperties.ElementID;
                    familyName = contextProperties.TypeName;
                }

                gbOpening.CADObjectID = opening.CADObjectID(exportType);
                gbOpening.OpeningType = opening.Type.ToGBXML();

                if (gbOpening.OpeningType.ToLower() == "fixedwindow" && contextProperties != null && contextProperties.TypeName.ToLower().Contains("skylight"))
                    gbOpening.OpeningType = "FixedSkylight";

                if (familyName == "System Panel") //No SAM_BuildingElementType for this one atm
                    gbOpening.OpeningType = "FixedWindow";

                if (exportType == ExportType.gbXMLIES && gbOpening.OpeningType.Contains("Window") && opening.OpeningConstruction.Name.Contains("SLD")) //Change windows with SLD construction into doors for IES
                    gbOpening.OpeningType = "NonSlidingDoor";

                if (exportType == ExportType.gbXMLIES)
                    gbOpening.WindowTypeIDRef = "window-" + (contextProperties == null? opening.OpeningConstruction.Name.CleanName() : contextProperties.TypeName.CleanName());
                else
                    gbOpening.WindowTypeIDRef = null;

                gbOpening.ConstructionIDRef = null; //ToDo review having this property on an opening?
                gbOpenings.Add(gbOpening);
            }

            return gbOpenings;
        }
    }
}