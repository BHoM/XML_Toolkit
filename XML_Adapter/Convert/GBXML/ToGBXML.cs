/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

using GBXML = BH.Adapter.XML.GBXMLSchema;
using BH.oM.Adapters.XML.Settings;
using BH.oM.Base;
using BH.oM.Environment.Elements;
using BH.oM.Environment.Fragments;
using BH.oM.Spatial.SettingOut;
using BH.oM.Physical.Constructions;

using BH.Engine.Environment;
using BH.oM.Adapters.XML;
using BH.Engine.Base;

namespace BH.Adapter.XML
{
    public static partial class Convert
    {
        public static GBXML.GBXML ToGBXML(this List<IBHoMObject> objs, GBXMLSettings settings)
        {
            List<Panel> panels = objs.Panels();
            List<Space> spaces = objs.Spaces();
            List<Level> levels = objs.Levels();
            List<Building> buildings = objs.Buildings();

            if (buildings.Count > 1)
                BH.Engine.Base.Compute.RecordWarning("More than 1 building has been supplied. Space information will be assigned to the first building but all buildings will be serialised");
            else if (buildings.Count == 0)
            {
                BH.Engine.Base.Compute.RecordWarning("No building was supplied, generic empty building is being used to group the data");
                buildings.Add(new Building());
            }

            List<Panel> shadingElements = panels.FilterPanelsByType(new List<PanelType>() { PanelType.Shade }).Item1;
            panels = panels.FilterPanelsByType(new List<PanelType>() { PanelType.Shade }).Item2; //Remove shading if it exists

            List<List<Panel>> panelsAsSpaces = panels.ToSpaces();
            List<Construction> constructions = panels.Where(x => x.Construction != null).Select(x => x.Construction as Construction).ToList();
            List<Construction> windowConstructions = panels.OpeningsFromElements().UniqueConstructions();

            List<GBXML.Building> xmlBuildings = buildings.Select(x => x.ToGBXML()).ToList();
            List<GBXML.BuildingStorey> xmlLevels = levels.Where(x => x.StoreyGeometry(panelsAsSpaces) != null).Select(x => x.ToGBXML(x.StoreyGeometry(panelsAsSpaces), settings)).ToList();
            List<GBXML.Space> xmlSpaces = panelsAsSpaces.Select(x => x.ToGBXML(x.Level(levels), settings)).OrderBy(x => x.Name).ToList();
            List<GBXML.Surface> xmlSurfaces = new List<GBXML.Surface>();
            List<GBXML.Construction> xmlConstructions = new List<GBXML.Construction>();
            List<GBXML.Layer> xmlLayers = new List<GBXML.Layer>();
            List<GBXML.Material> xmlMaterials = new List<GBXML.Material>();
            List<GBXML.WindowType> xmlWindows = new List<GBXML.WindowType>();

            List<Panel> usedPanels = new List<Panel>();
            foreach(List<Panel> space in panelsAsSpaces)
            {
                foreach(Panel p in space)
                {
                    if (usedPanels.Where(x => x.BHoM_Guid == p.BHoM_Guid).FirstOrDefault() != null) continue;

                    xmlSurfaces.Add(p.ToGBXML(settings, space));
                    usedPanels.Add(p);
                }
            }

            //Check we haven't missed any panels (such as shading) and include them
            foreach (Panel p in panels)
            {
                if (usedPanels.Where(x => x.BHoM_Guid == p.BHoM_Guid).FirstOrDefault() != null) continue;

                xmlSurfaces.Add(p.ToGBXML(settings));
                usedPanels.Add(p);
            }

            //Include shading
            foreach (Panel p in shadingElements)
            {
                if (usedPanels.Where(x => x.BHoM_Guid == p.BHoM_Guid).FirstOrDefault() != null) continue;

                xmlSurfaces.Add(p.ToGBXMLShade(settings));
                usedPanels.Add(p);
            }

            foreach (Construction c in constructions)
            {
                if (c == null)
                    continue;

                GBXML.Construction xmlConc = c.ToGBXML();
                if (xmlConstructions.Where(x => x.ID == xmlConc.ID).FirstOrDefault() != null)
                    continue; //Don't add the same construction twice

                List<GBXML.Material> layerMaterials = new List<GBXML.Material>();
                foreach (Layer l in c.Layers)
                    layerMaterials.Add(l.ToGBXML(settings));

                xmlMaterials.AddRange(layerMaterials);

                GBXML.Layer layer = layerMaterials.ToGBXML();
                xmlLayers.Add(layer);

                xmlConc.LayerID.LayerIDRef = layer.ID;
                xmlConstructions.Add(xmlConc);
            }

            foreach(Opening o in panels.OpeningsFromElements())
            {
                OriginContextFragment openingEnvContextProperties = o.FindFragment<OriginContextFragment>(typeof(OriginContextFragment));
                
                string nameCheck = "";
                if (openingEnvContextProperties != null)
                    nameCheck = openingEnvContextProperties.TypeName;
                else if (o.OpeningConstruction != null)
                    nameCheck = o.OpeningConstruction.Name;

                var t = xmlWindows.Where(a => a.Name == nameCheck).FirstOrDefault();
                if (t == null && o.OpeningConstruction != null)
                    xmlWindows.Add(o.OpeningConstruction.ToGBXMLWindow(settings, o));
            }

            GBXML.GBXML gbx = new GBXML.GBXML();

            gbx.Campus.Building = xmlBuildings.ToArray();
            gbx.Campus.Location = buildings[0].ToGBXMLLocation(settings);
            gbx.Campus.Building[0].BuildingStorey = xmlLevels.ToArray();
            gbx.Campus.Building[0].Space = xmlSpaces;
            gbx.Campus.Surface = xmlSurfaces;

            if (settings.IncludeConstructions)
            {
                gbx.Construction = xmlConstructions.ToArray();
                gbx.Layer = xmlLayers.ToArray();
                gbx.Material = xmlMaterials.ToArray();
                gbx.WindowType = xmlWindows.ToArray();
            }
            else
                gbx.WindowType = null;

            //Set the building area
            List<Panel> floorElements = panels.Where(x => x.Type == PanelType.Floor || x.Type == PanelType.FloorExposed || x.Type == PanelType.FloorInternal || x.Type == PanelType.FloorRaised || x.Type == PanelType.SlabOnGrade || x.Type == PanelType.UndergroundSlab).ToList();

            double buildingFloorArea = 0;
            foreach (Panel p in floorElements)
                buildingFloorArea += p.Area();

            gbx.Campus.Building[0].Area = buildingFloorArea;

            //Document History                          
            GBXML.DocumentHistory DocumentHistory = new GBXML.DocumentHistory();
            DocumentHistory.CreatedBy.Date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            gbx.DocumentHistory = DocumentHistory;

            //Unit set up
            gbx.TemperatureUnit = settings.UnitSetUp.TemperatureUnit.ToGBX();
            gbx.LengthUnit = settings.UnitSetUp.LengthUnit.ToGBX();
            gbx.AreaUnit = settings.UnitSetUp.AreaUnit.ToGBX();
            gbx.VolumeUnit = settings.UnitSetUp.VolumeUnit.ToGBX();
            gbx.UseSIUnitsForResults = (settings.ResultsUnitType == oM.Adapters.XML.Enums.UnitType.SI ? "true" : "false");

            return gbx;
        }

        private static string ToGBX(this AreaUnit areaUnit)
        {
            switch(areaUnit)
            {
                case AreaUnit.SquareCentimeters:
                    return "SquareCentimeters";
                case AreaUnit.SquareFeet:
                    return "SquareFeet";
                case AreaUnit.SquareInches:
                    return "SquareInches";
                case AreaUnit.SquareKilometers:
                    return "SquareKilometers";
                case AreaUnit.SquareMeters:
                    return "SquareMeters";
                case AreaUnit.SquareMiles:
                    return "SquareMiles";
                case AreaUnit.SquareMillimeters:
                    return "SquareMillimeters";
                case AreaUnit.SquareYards:
                    return "SquareYards";
                default:
                    return "SquareMeters";
            }
        }

        private static string ToGBX(this LengthUnit lengthUnit)
        {
            switch(lengthUnit)
            {
                case LengthUnit.Centimeters:
                    return "Centimeters";
                case LengthUnit.Feet:
                    return "Feet";
                case LengthUnit.Inches:
                    return "Inches";
                case LengthUnit.Kilometers:
                    return "Kilometers";
                case LengthUnit.Meters:
                    return "Meters";
                case LengthUnit.Miles:
                    return "Miles";
                case LengthUnit.Millimeters:
                    return "Millimeters";
                case LengthUnit.Yards:
                    return "Yards";
                default:
                    return "Meters";
            }
        }

        private static string ToGBX(this TemperatureUnit temperatureUnit)
        {
            switch(temperatureUnit)
            {
                case TemperatureUnit.Celcius:
                    return "C";
                case TemperatureUnit.Fahrenheit:
                    return "F";
                case TemperatureUnit.Kelvin:
                    return "K";
                case TemperatureUnit.Rankine:
                    return "R";
                default:
                    return "C";
            }
        }

        private static string ToGBX(this VolumeUnit volumeUnit)
        {
            switch(volumeUnit)
            {
                case VolumeUnit.CubicCentimeters:
                    return "CubicCentimeters";
                case VolumeUnit.CubicFeet:
                    return "CubicFeet";
                case VolumeUnit.CubicInches:
                    return "CubicInches";
                case VolumeUnit.CubicKilometers:
                    return "CubicKilometers";
                case VolumeUnit.CubicMeters:
                    return "CubicMeters";
                case VolumeUnit.CubicMiles:
                    return "CubicMiles";
                case VolumeUnit.CubicMillimeters:
                    return "CubicMillimeters";
                case VolumeUnit.CubicYards:
                    return "CubicYards";
                default:
                    return "CubicMeters";
            }
        }
    }
}





