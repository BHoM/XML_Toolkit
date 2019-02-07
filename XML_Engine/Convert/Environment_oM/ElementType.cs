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
using BHP = BH.oM.Environment.Properties;
using BHI = BH.oM.Environment.Interface;
using BHX = BH.oM.XML;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;
using BH.Engine.Environment;

using BH.oM.XML.Enums;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        public static string ToGBXMLType(this BHE.BuildingElement element, List<BHE.Space> adjacentSpaces = null, ExportType exportType = ExportType.gbXMLTAS)
        {
            if (adjacentSpaces == null) adjacentSpaces = new List<oM.Environment.Elements.Space>();

            string type = "Air";
            if (element == null || element.ElementProperties() == null)
                return type;

            BHP.ElementProperties elementProperties = element.ElementProperties() as BHP.ElementProperties;

            if (adjacentSpaces.Count == 0 && elementProperties.BuildingElementType != BHE.BuildingElementType.Window && elementProperties.BuildingElementType != BHE.BuildingElementType.Door)
                type = "Shade";
            else
                type = elementProperties.BuildingElementType.ToGBXML();

            return type;
        }

        public static string ToGBXMLType(this BHI.IBHoMExtendedProperties props)
        {
            if (props == null) return "";
            return ToGBXMLType(props as dynamic);
        }

        public static string ToGBXMLType(this BHP.ElementProperties elementProperties)
        {
            if (elementProperties == null) return "";

            return elementProperties.BuildingElementType.ToGBXML();
        }

        public static string ToGBXML(this BHE.BuildingElementType type)
        {
            switch(type)
            {
                case BHE.BuildingElementType.Ceiling:
                    return "Ceiling";
                case oM.Environment.Elements.BuildingElementType.CurtainWall:
                    return "ExteriorWall";
                case oM.Environment.Elements.BuildingElementType.Door:
                    return "NonSlidingDoor";
                case oM.Environment.Elements.BuildingElementType.Floor:
                    return "Floor";
                case oM.Environment.Elements.BuildingElementType.FloorExposed:
                    return "ExposedFloor";
                case oM.Environment.Elements.BuildingElementType.FloorInternal:
                    return "InternalFloor";
                case oM.Environment.Elements.BuildingElementType.FloorRaised:
                    return "RaisedFloor";
                case BHE.BuildingElementType.Frame:
                    return "Frame";
                case BHE.BuildingElementType.Glazing:
                case BHE.BuildingElementType.Window:
                case BHE.BuildingElementType.WindowWithFrame:
                    return "FixedWindow";
                case BHE.BuildingElementType.Roof:
                    return "Roof";
                case BHE.BuildingElementType.Rooflight:
                case BHE.BuildingElementType.RooflightWithFrame:
                    return "OperableSkylight";
                case BHE.BuildingElementType.Shade:
                    return "Shade";
                case BHE.BuildingElementType.SlabOnGrade:
                    return "SlabOnGrade";
                case BHE.BuildingElementType.SolarPanel:
                    return "SolarPanel";
                case BHE.BuildingElementType.UndergroundCeiling:
                    return "UndergroundCeiling";
                case BHE.BuildingElementType.UndergroundSlab:
                    return "UndergroundSlab";
                case BHE.BuildingElementType.UndergroundWall:
                    return "UndergroundWall";
                case BHE.BuildingElementType.VehicleDoor:
                    return "VehicleDoor";
                case BHE.BuildingElementType.Wall:
                    return "Wall";
                case BHE.BuildingElementType.WallExternal:
                    return "ExteriorWall";
                case BHE.BuildingElementType.WallInternal:
                    return "InteriorWall";
                default:
                    return "Air"; //Adiabatic
            }
        }

        
    }
}