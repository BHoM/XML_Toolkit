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
using BH.oM.Base;
using BH.oM.XML.Enums;

using System.ComponentModel;

namespace BH.oM.XML.Settings
{
    public class XMLSettings : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        [Description("Set to true if you want to replace curtain walls to have openings the same size as the wall. This is useful for IES exports. Default false")]
        public bool ReplaceCurtainWalls { get; set; } = false;

        [Description("Set to true if you want to replace an opening which is marked as solid into a door. Useful for IES exports. Default false")]
        public bool ReplaceSolidOpeningsIntoDoors { get; set; } = false;

        [Description("Set to true if you want to include construction and material data in the export. Default false")]
        public bool IncludeConstructions { get; set; } = false;

        [Description("Set to true if you want air types with one adjacent space (i.e. external air walls) to have their type fixed based on their tilt. Default false")]
        public bool FixIncorrectAirTypes { get; set; } = false;

        [Description("Set to false if you want to append to a file when pushing XML. If set to true then a file will be created. If a file exists, it will be overwritten. Default true")]
        public bool NewFile { get; set; } = true;

        [Description("Set the unit type for the export to be either SI or Imperial. Default SI")]
        public UnitType UnitType { get; set; } = UnitType.SI;

        [Description("Set the detail of your export to be either full (whole building), shell (exterior walls only), or spaces (each individual space as its own XML file). Default full")]
        public ExportDetail ExportDetail { get; set; } = ExportDetail.Full;

        [Description("distanceTolerance is used as Description for CleanPolyline method used for opening, default is set to BH.oM.Geometry.Tolerance.Distance")]
        public double DistanceTolerance { get; set; } = BH.oM.Geometry.Tolerance.Distance;

        [Description("Set tolerance for planar surfaces, default is set to BH.oM.Geometry.Tolerance.Distance")]
        public double PlanarTolerance { get; set; } = BH.oM.Geometry.Tolerance.Distance;

        [Description("Set a distance to offset openings that have a area >= the area of the host panel. Value should be negative. Defaults to -0.001")]
        public double OffsetDistance { get; set; } = -0.001;

        /***************************************************/
    }
}

