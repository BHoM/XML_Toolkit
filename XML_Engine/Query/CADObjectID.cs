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
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using System.Linq;
using BHE = BH.oM.Environment.Elements;
using BHP = BH.oM.Environment.Fragments;

using BH.oM.Adapters.XML.Enums;

using BH.Engine.Environment;

using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Adapters.XML
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Gets the CAD Object ID for an Environment Panel suitable for GBXML")]
        [Input("element", "An Environment Panel to obtain the CAD Object ID of")]
        [Input("replaceCurtainWalls", "Determine whether to change the CAD Object ID based on whether we are replacing curtain walls or not, default false")]
        [Output("cadObjectID", "The CAD Object ID for the software where this Panel came from")]
        public static string CADObjectID(this BHE.Panel element, bool replaceCurtainWalls = false)
        {
            string CADObjectID = "";
            if (element == null) return CADObjectID;

            BHP.OriginContextFragment contextProperties = element.FindFragment<BHP.OriginContextFragment>(typeof(BHP.OriginContextFragment));

            if (contextProperties != null && contextProperties.ElementID != "")
            {
                // change only Basic Wall and keep Curtain as it is
                if (replaceCurtainWalls && element.Name.Contains("GLZ") && (element.Name.Contains("Basic Wall") || element.Name.Contains("Floor") || element.Name.Contains("Roof")))
                    element.Name = "Curtain " + element.Name;

                CADObjectID = element.Name + " [" + contextProperties.ElementID + "]";
            }
            else
            {
                //Use a default object ID
                switch(element.Type)
                {
                    case BHE.PanelType.Ceiling:
                        CADObjectID += "Compound Ceiling: SIM_INT_SLD";
                        break;
                    case BHE.PanelType.CurtainWall:
                        if (element.ConnectedSpaces.Count == 2)
                            CADObjectID += "Curtain Wall: SIM_INT_GLZ";
                        else
                            CADObjectID += "Curtain Wall: SIM_EXT_GLZ";
                        break;
                    case BHE.PanelType.Floor:
                    case BHE.PanelType.FloorInternal:
                        CADObjectID += "Floor: SIM_INT_SLD";
                        break;
                    case BHE.PanelType.FloorExposed:
                    case BHE.PanelType.FloorRaised:
                        CADObjectID += "Floor: SIM_EXT_SLD";
                        break;
                    case BHE.PanelType.Roof:
                        CADObjectID += "Basic Roof: SIM_EXT_SLD";
                        break;
                    case BHE.PanelType.Shade:
                        CADObjectID += "Basic Roof: SIM_EXT_SHD_Roof";
                        break;
                    case BHE.PanelType.SlabOnGrade:
                        CADObjectID += "Floor: SIM_EXT_GRD";
                        break;
                    case BHE.PanelType.UndergroundCeiling:
                        CADObjectID += "Floor: SIM_INT_SLD_Parking";
                        break;
                    case BHE.PanelType.UndergroundSlab:
                        CADObjectID += "Floor: SIM_EXT_GRD";
                        break;
                    case BHE.PanelType.UndergroundWall:
                        CADObjectID += "Basic Wall: SIM_EXT_GRD";
                        break;
                    case BHE.PanelType.Wall:
                    case BHE.PanelType.WallExternal:
                        CADObjectID += "Basic Wall: SIM_EXT_SLD";
                        break;
                    case BHE.PanelType.WallInternal:
                        CADObjectID += "Basic Wall: SIM_INT_SLD";
                        break;
                    default:
                        CADObjectID += "Undefined";
                        break;
                }
                CADObjectID += " BHoM [000000]";
            }

            return CADObjectID;
        }

        [Description("Gets the CAD Object ID for a Space suitable for GBXML")]
        [Input("panelsAsSpace", "A collection of Environment Panels that represent a closed space to obtain the CAD Object ID of")]
        [Output("cadObjectID", "The CAD Object ID for the software where this collection of Panels came from")]
        public static string CADObjectID(this List<BHE.Panel> panelsAsSpace)
        {
            return "[" + panelsAsSpace.ConnectedSpaceName() + "]"; //ToDo: Fix this properly when the oM changes are made
        }

        /***************************************************/

        [Description("Gets the CAD Object ID for an Environment Opening suitable for GBXML")]
        [Input("opening", "An Environment Opening to obtain the CAD Object ID of")]
        [Output("cadObjectID", "The CAD Object ID for the software where this Opening came from")]
        public static string CADObjectID(this BHE.Opening opening)
        {
            BHP.OriginContextFragment contextProp = opening.FindFragment<BHP.OriginContextFragment>(typeof(BHP.OriginContextFragment));
            if (contextProp == null) return "WinInst: SIM_EXT_GLZ [000000]";

            string cadID = "";
            if (contextProp.TypeName == "") cadID += "WinInst: SIM_EXT_GLZ";
            else cadID += contextProp.TypeName;

            cadID += " [";

            if (contextProp.ElementID == "") cadID += "000000";
            else cadID += contextProp.ElementID;

            cadID += "]";

            return cadID;
        }

        /***************************************************/

        [Description("Gets the CAD Object ID for a Space suitable for GBXML")]
        [Input("space", "An Environment Space to obtain the CAD Object ID of")]
        [Output("cadObjectID", "The CAD Object ID for the software where this space came from")]
        public static string CADObjectID(this BHE.Space space)
        {
            BHP.OriginContextFragment contextProp = space.FindFragment<BHP.OriginContextFragment>(typeof(BHP.OriginContextFragment));
            if (contextProp == null) return "";

            return contextProp.TypeName + " [" + contextProp.ElementID + "]";
        }
    }
}

