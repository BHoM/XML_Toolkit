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

using BHE = BH.oM.Environment.Elements;

using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Adapter.XML
{
    public static partial class Convert
    {
        [Description("Get the GBXML type for a BHoM Environments Panel")]
        [Input("element", "The BHoM Environments Panel to get the GBXML Type from")]
        [Input("adjacentSpaces", "A collection of Environment Panels that are adjacent to the element the type is being obtained from")]
        [Output("type", "The GBXML type for the BHoM Environment Panel")]
        public static string ToGBXMLType(this BHE.Panel element, List<List<BHE.Panel>> adjacentSpaces = null)
        {
            if (adjacentSpaces == null) adjacentSpaces = new List<List<BHE.Panel>>();

            string type = "Air";
            if (element == null)
                return type;

            if (adjacentSpaces.Count == 0)
                type = "Shade";
            else
                type = element.Type.ToGBXML();

            return type;
        }

        [Description("Get the GBXML representation of a BHoM Environments Panel Type")]
        [Input("type", "The BHoM Environments Panel Type to convert into a GBXML Type")]
        [Output("type", "The GBXML representation of a BHoM Environment Panel Type")]
        public static string ToGBXML(this BHE.PanelType type)
        {
            switch(type)
            {
                case BHE.PanelType.Ceiling:
                    return "Ceiling";
                case BHE.PanelType.Floor:
                case BHE.PanelType.FloorExposed:
                    return "ExposedFloor";
                case BHE.PanelType.FloorInternal:
                    return "InteriorFloor";
                case BHE.PanelType.FloorRaised:
                    return "RaisedFloor";
                case BHE.PanelType.Roof:
                    return "Roof";
                case BHE.PanelType.Shade:
                    return "Shade";
                case BHE.PanelType.SlabOnGrade:
                    return "SlabOnGrade";
                case BHE.PanelType.SolarPanel:
                    return "SolarPanel";
                case BHE.PanelType.UndergroundCeiling:
                    return "UndergroundCeiling";
                case BHE.PanelType.UndergroundSlab:
                    return "UndergroundSlab";
                case BHE.PanelType.UndergroundWall:
                    return "UndergroundWall";
                case BHE.PanelType.Wall:
                case BHE.PanelType.WallExternal:
                    return "ExteriorWall";
                case BHE.PanelType.WallInternal:
                    return "InteriorWall";
                default:
                    return "Air"; //Adiabatic
            }
        }

        [Description("Get the GBXML representation of a BHoM Environments Opening Type")]
        [Input("type", "The BHoM Environments Opening Type to convert into a GBXML Type")]
        [Output("type", "The GBXML representation of a BHoM Environment Opening Type")]
        public static string ToGBXML(this BHE.OpeningType type)
        {
            switch (type)
            {
                case BHE.OpeningType.Door:
                    return "NonSlidingDoor";
                case BHE.OpeningType.Frame:
                    return "Frame";
                case BHE.OpeningType.Glazing:
                case BHE.OpeningType.Window:
                case BHE.OpeningType.WindowWithFrame:
                case BHE.OpeningType.CurtainWall:
                    return "FixedWindow";
                case BHE.OpeningType.Rooflight:
                case BHE.OpeningType.RooflightWithFrame:
                    return "OperableSkylight";
                case BHE.OpeningType.VehicleDoor:
                    return "VehicleDoor";
                default:
                    return "Air"; //Adiabatic
            }
        }

        [Description("Get the BHoM Environments Panel Type representation of a GBXML Type")]
        [Input("type", "The GBXML Type to convert into a BHoM Environments Panel Type")]
        [Output("type", "The BHoM representation of a GBXML Type")]
        public static BHE.PanelType FromGBXMLPanelType(this string type)
        {
            switch (type)
            {
                case "Ceiling":
                    return BHE.PanelType.Ceiling;
                case "ExposedFloor":
                    return BHE.PanelType.FloorExposed;
                case "InteriorFloor":
                    return BHE.PanelType.FloorInternal;
                case "RaisedFloor":
                    return BHE.PanelType.FloorRaised;
                case "Roof":
                    return BHE.PanelType.Roof;
                case "Shade":
                    return BHE.PanelType.Shade;
                case "SlabOnGrade":
                    return BHE.PanelType.SlabOnGrade;
                case "SolarPanel":
                    return BHE.PanelType.SolarPanel;
                case "UndergroundCeiling":
                    return BHE.PanelType.UndergroundCeiling;
                case "UndergroundSlab":
                    return BHE.PanelType.UndergroundSlab;
                case "UndergroundWall":
                    return BHE.PanelType.UndergroundWall;
                case "ExteriorWall":
                    return BHE.PanelType.WallExternal;
                case "InteriorWall":
                    return BHE.PanelType.WallInternal;
                default:
                    return BHE.PanelType.Undefined;
            }
        }

        [Description("Get the BHoM Environments Opening Type representation of a GBXML Type")]
        [Input("type", "The GBXML Type to convert into a BHoM Environments Opening Type")]
        [Output("type", "The BHoM representation of a GBXML Type")]
        public static BHE.OpeningType FromGBXMLOpeningType(this string type)
        {
            switch (type)
            {
                case "NonSlidingDoor":
                    return BHE.OpeningType.Door;
                case "Frame":
                    return BHE.OpeningType.Frame;
                case "FixedWindow":
                    return BHE.OpeningType.Window;
                case "OperableSkylight":
                    return BHE.OpeningType.Rooflight;
                case "VehicleDoor":
                    return BHE.OpeningType.VehicleDoor;
                default:
                    return BHE.OpeningType.Undefined;
            }
        }

    }
}
