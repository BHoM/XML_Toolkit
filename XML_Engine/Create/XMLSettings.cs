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

using BH.oM.XML.Settings;
using BH.oM.XML.Enums;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;
using System.Reflection;

namespace BH.Engine.XML
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Deprecated("2.4", "Deprecated to expose Planar Tolerance setting", null, "XMLSettings(bool replaceCurtainWalls = false, bool replaceSolidOpeningsIntoDoors = false, bool includeConstructions = false, bool fixIncorrectAirTypes = false, bool newFile = true, UnitType unitType = UnitType.SI, ExportDetail exportDetail = ExportDetail.Full, double distanceTolerance = BH.oM.Geometry.Tolerance.Distance, double planarTolerance = BH.oM.Geometry.Tolerance.Distance)")]
        [Input("replaceCurtainWalls", "Set to true if you want to replace curtain walls to have openings the same size as the wall. This is useful for IES exports. Default false")]
        [Input("replaceSolidOpeningsIntoDoors", "Set to true if you want to replace an opening which is marked as solid into a door. Useful for IES exports. Default false")]
        [Input("includeConstructions", "Set to true if you want to include construction and material data in the export. Default false")]
        [Input("fixIncorrectAirTypes", "Set to true if you want air types with one adjacent space (i.e. external air walls) to have their type fixed based on their tilt. Default false")]
        [Input("newFile", "Set to false if you want to append to a file when pushing XML. If set to true then a file will be created. If a file exists, it will be overwritten. Default true")]
        [Input("unitType", "Set the unit type for the export to be either SI or Imperial. Default SI")]
        [Input("exportDetail", "Set the detail of your export to be either full (whole building), shell (exterior walls only), or spaces (each individual space as its own XML file). Default full")]
        [Input("distanceTolerance", "distanceTolerance is used as input for CleanPolyline method used for opening, default is set to BH.oM.Geometry.Tolerance.Distance")]
        public static XMLSettings XMLSettings(bool replaceCurtainWalls = false, bool replaceSolidOpeningsIntoDoors = false, bool includeConstructions = false, bool fixIncorrectAirTypes = false, bool newFile = true, UnitType unitType = UnitType.SI, ExportDetail exportDetail = ExportDetail.Full, double distanceTolerance = 0.01)
        {
            return XMLSettings(replaceCurtainWalls, replaceSolidOpeningsIntoDoors, includeConstructions, fixIncorrectAirTypes, newFile, unitType, exportDetail, distanceTolerance, BH.oM.Geometry.Tolerance.Distance);
        }

        [Deprecated("3.0", "Deprecated to expose Offset Distance setting", null, "XMLSettings(bool replaceCurtainWalls = false, bool replaceSolidOpeningsIntoDoors = false, bool includeConstructions = false, bool fixIncorrectAirTypes = false, bool newFile = true, UnitType unitType = UnitType.SI, ExportDetail exportDetail = ExportDetail.Full, double distanceTolerance = BH.oM.Geometry.Tolerance.Distance, double planarTolerance = BH.oM.Geometry.Tolerance.Distance, double offsetDistance = -0.001)")]
        [Input("replaceCurtainWalls", "Set to true if you want to replace curtain walls to have openings the same size as the wall. This is useful for IES exports. Default false")]
        [Input("replaceSolidOpeningsIntoDoors", "Set to true if you want to replace an opening which is marked as solid into a door. Useful for IES exports. Default false")]
        [Input("includeConstructions", "Set to true if you want to include construction and material data in the export. Default false")]
        [Input("fixIncorrectAirTypes", "Set to true if you want air types with one adjacent space (i.e. external air walls) to have their type fixed based on their tilt. Default false")]
        [Input("newFile", "Set to false if you want to append to a file when pushing XML. If set to true then a file will be created. If a file exists, it will be overwritten. Default true")]
        [Input("unitType", "Set the unit type for the export to be either SI or Imperial. Default SI")]
        [Input("exportDetail", "Set the detail of your export to be either full (whole building), shell (exterior walls only), or spaces (each individual space as its own XML file). Default full")]
        [Input("distanceTolerance", "distanceTolerance is used as input for CleanPolyline method used for opening, default is set to BH.oM.Geometry.Tolerance.Distance")]
        [Input("planarTolerance", "Set tolerance for planar surfaces, default is set to BH.oM.Geometry.Tolerance.Distance")]
        public static XMLSettings XMLSettings(bool replaceCurtainWalls = false, bool replaceSolidOpeningsIntoDoors = false, bool includeConstructions = false, bool fixIncorrectAirTypes = false, bool newFile = true, UnitType unitType = UnitType.SI, ExportDetail exportDetail = ExportDetail.Full, double distanceTolerance = BH.oM.Geometry.Tolerance.Distance, double planarTolerance = BH.oM.Geometry.Tolerance.Distance)
        {
            return XMLSettings(replaceCurtainWalls, replaceSolidOpeningsIntoDoors, includeConstructions, fixIncorrectAirTypes, newFile, unitType, exportDetail, distanceTolerance, planarTolerance, -0.001);
        }

        [Deprecated("3.1", "Deprecated in favour of default Create components available within the BHoM")]
        [Input("replaceCurtainWalls", "Set to true if you want to replace curtain walls to have openings the same size as the wall. This is useful for IES exports. Default false")]
        [Input("replaceSolidOpeningsIntoDoors", "Set to true if you want to replace an opening which is marked as solid into a door. Useful for IES exports. Default false")]
        [Input("includeConstructions", "Set to true if you want to include construction and material data in the export. Default false")]
        [Input("fixIncorrectAirTypes", "Set to true if you want air types with one adjacent space (i.e. external air walls) to have their type fixed based on their tilt. Default false")]
        [Input("newFile", "Set to false if you want to append to a file when pushing XML. If set to true then a file will be created. If a file exists, it will be overwritten. Default true")]
        [Input("unitType", "Set the unit type for the export to be either SI or Imperial. Default SI")]
        [Input("exportDetail", "Set the detail of your export to be either full (whole building), shell (exterior walls only), or spaces (each individual space as its own XML file). Default full")]
        [Input("distanceTolerance", "distanceTolerance is used as input for CleanPolyline method used for opening, default is set to BH.oM.Geometry.Tolerance.Distance")]
        [Input("planarTolerance", "Set tolerance for planar surfaces, default is set to BH.oM.Geometry.Tolerance.Distance")]
        [Input("offsetDistance", "Set a distance to offset openings that have a area >= the area of the host panel. Value should be negative. Defaults to -0.001")]
        [Output("xmlSettings", "The XML settings to use with the XML adapter push")]
        public static XMLSettings XMLSettings(bool replaceCurtainWalls = false, bool replaceSolidOpeningsIntoDoors = false, bool includeConstructions = false, bool fixIncorrectAirTypes = false, bool newFile = true, UnitType unitType = UnitType.SI, ExportDetail exportDetail = ExportDetail.Full, double distanceTolerance = BH.oM.Geometry.Tolerance.Distance, double planarTolerance = BH.oM.Geometry.Tolerance.Distance, double offsetDistance = -0.001)
        {
            if (offsetDistance >= 0)
            {
                BH.Engine.Reflection.Compute.RecordWarning("Offset distance should be negative to offset the openings inwards, away from the edges of the host panel and ensuring opening area is less than the host panel area. A positive offset will result in openings being larger than the host panel");
            }
            return new XMLSettings
            {
                ReplaceCurtainWalls = replaceCurtainWalls,
                ReplaceSolidOpeningsIntoDoors = replaceSolidOpeningsIntoDoors,
                IncludeConstructions = includeConstructions,
                FixIncorrectAirTypes = fixIncorrectAirTypes,
                NewFile = newFile,
                UnitType = unitType,
                ExportDetail = exportDetail,
                DistanceTolerance = distanceTolerance,
                PlanarTolerance = planarTolerance,
                OffsetDistance = offsetDistance,
            };
        }
    }
}

