/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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

using NUnit.Framework;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Adapter.XML;
using BH.Engine.Environment;
using BH.Engine.Adapter;
using BH.Engine.Adapters.XML;
using BH.oM.Adapters.XML;
using BH.oM.Adapter;
using BH.oM.Adapters.XML.Settings;
using BH.oM.Base;
using BH.oM.Data.Requests;
using BH.oM.Environment.Elements;
using BH.oM.Physical.Constructions;

namespace BH.Tests.Adapter.XML
{
    public class PushTests
    {
        XMLAdapter m_adapter;
        XMLConfig m_config;
        List<IBHoMObject> m_jsonObjects;
        List<Panel> m_jsonPanels;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            List<string> paths = currentDirectory.Split('\\').ToList();
            paths = paths.Take(paths.IndexOf(".ci") + 2).ToList();
            string ModelsPath = Path.Join(string.Join("\\", paths), "Models");
            m_adapter = new XMLAdapter();
            m_config = new XMLConfig()
            {
                File = new FileSettings() { Directory = ModelsPath, FileName = "PushedModel.xml"},
                Schema = oM.Adapters.XML.Enums.Schema.GBXML
            };
            m_jsonObjects = BH.Engine.Adapters.File.Compute.ReadFromJsonFile(Path.Combine(ModelsPath, "TestModel.json"), true).Cast<IBHoMObject>().ToList();
            m_jsonPanels = BH.Engine.Environment.Query.Panels(m_jsonObjects);
        }

        [SetUp]
        public void Setup()
        {
            BH.Engine.Base.Compute.ClearCurrentEvents();
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(m_config.File.GetFullFileName());
            var events = Engine.Base.Query.CurrentEvents();
            if (events.Any())
            {
                Console.WriteLine("BHoM Events raised during execution:");
                foreach (var ev in events)
                {
                    Console.WriteLine($"{ev.Type}: {ev.Message}");
                }
            }
        }

        [Test]
        [Description("Test for pushing a set of BHoM environment objects to xml with constructions set to true.")]
        public void PushGBXML()
        {
            m_config.Settings = new GBXMLSettings() 
            {
                IncludeConstructions = true,
                ExportDetail = oM.Adapters.XML.Enums.ExportDetail.Full,
                UnitSetUp = new GBXMLUnitSetUp()
                {
                    TemperatureUnit = TemperatureUnit.Celcius,
                    LengthUnit = LengthUnit.Meters,
                    AreaUnit = AreaUnit.SquareMeters,
                    VolumeUnit = VolumeUnit.CubicMeters
                }
            };
            FilterRequest request = new FilterRequest();

            //Push, then pull objects.
            m_adapter.Push(m_jsonObjects, actionConfig: m_config);
            List<IBHoMObject> objs = m_adapter.Pull(request, actionConfig: m_config).Cast<IBHoMObject>().ToList();

            List<Panel> pulledPanels = BH.Engine.Environment.Query.Panels(objs);
            List<Construction> constructions = objs.Where(x => x.GetType() == typeof(Construction)).Cast<Construction>().ToList();
            List<Construction> jsonConstructions = m_jsonObjects.Where(x => x.GetType() == typeof(Construction) && x.Name == "generic_construction").Cast<Construction>().ToList();

            pulledPanels = BH.Engine.Data.Query.OrderBy(pulledPanels, "Name");
            List<Panel> jsonPanels = BH.Engine.Data.Query.OrderBy(m_jsonPanels, "Name");

            //assert correct values
            pulledPanels.Count.Should().Be(jsonPanels.Count, "There was a different number of panels pushed then pulled compared to expected.");
            for (int i = 0; i < pulledPanels.Count; i++)
            {
                pulledPanels[i].Name.Should().Be(jsonPanels[i].Name, "The name of the panel pulled was not the same as the json panel.");
                pulledPanels[i].IsIdentical(jsonPanels[i]).Should().BeTrue($"The panel with name {pulledPanels[i].Name} was not identical to the json panel with the same name.");
            }
            constructions.Count.Should().Be(jsonConstructions.Count, "There is only one type of construction (generic_construction) used in the building.");

            objs.Count.Should().Be(m_jsonObjects.Count-3, "The number of pulled objects should be the same as the number of json objects, minus unused constructions.");
        }

        [Test]
        [Description("Test for pushing a set of BHoM environment objects to xml with constructions set to false.")]
        public void PushGBXMLNoConstructions()
        {
            m_config.Settings = new GBXMLSettings() 
            { 
                IncludeConstructions = false,
                ExportDetail = oM.Adapters.XML.Enums.ExportDetail.Full,
                UnitSetUp = new GBXMLUnitSetUp()
                {
                    TemperatureUnit = TemperatureUnit.Celcius,
                    LengthUnit = LengthUnit.Meters,
                    AreaUnit = AreaUnit.SquareMeters,
                    VolumeUnit = VolumeUnit.CubicMeters
                }
            };
            FilterRequest request = new FilterRequest();

            //Push, then pull objects.
            m_adapter.Push(m_jsonObjects, actionConfig: m_config);
            List<IBHoMObject> objs = m_adapter.Pull(request, actionConfig: m_config).Cast<IBHoMObject>().ToList();

            List<Panel> pulledPanels = BH.Engine.Environment.Query.Panels(objs);
            List<Construction> constructions = objs.Where(x => x.GetType() == typeof(Construction)).Cast<Construction>().ToList();

            pulledPanels = BH.Engine.Data.Query.OrderBy(pulledPanels, "Name");
            List<Panel> jsonPanels = BH.Engine.Data.Query.OrderBy(m_jsonPanels, "Name");

            //assert correct values
            pulledPanels.Count.Should().Be(jsonPanels.Count, "There was a different number of panels pushed then pulled compared to expected.");
            for (int i = 0; i < pulledPanels.Count; i++)
            {
                pulledPanels[i].Name.Should().Be(jsonPanels[i].Name, "The name of the panel pulled was not the same as the json panel.");
                pulledPanels[i].IsIdentical(jsonPanels[i]).Should().BeTrue($"The panel with name {pulledPanels[i].Name} was not identical to the json panel with the same name.");
            }
            constructions.Count.Should().Be(0, "No constructions should be pulled, but some were pulled anyway.");

            objs.Count.Should().Be(m_jsonObjects.Count - 4, "The number of pulled objects should be the same as the number of json objects, minus constructions.");
        }

        [Test]
        [Description("Test for pushing GBXML with exportDetail set to BuildingShell")]
        public void PushGBXMLBuildingShell()
        {
            m_config.Settings = new GBXMLSettings()
            {
                IncludeConstructions = false,
                ExportDetail = oM.Adapters.XML.Enums.ExportDetail.BuildingShell,
                UnitSetUp = new GBXMLUnitSetUp()
                {
                    TemperatureUnit = TemperatureUnit.Celcius,
                    LengthUnit = LengthUnit.Meters,
                    AreaUnit = AreaUnit.SquareMeters,
                    VolumeUnit = VolumeUnit.CubicMeters
                }
            };
            FilterRequest request = new FilterRequest();

            //push, then pull objects
            m_adapter.Push(m_jsonObjects, actionConfig: m_config);
            List<IBHoMObject> objs = m_adapter.Pull(request, actionConfig: m_config).Cast<IBHoMObject>().ToList();

            List<Panel> pulledPanels = BH.Engine.Environment.Query.Panels(objs);
            List<Construction> constructions = objs.Where(x => x.GetType() == typeof(Construction)).Cast<Construction>().ToList();

            pulledPanels = BH.Engine.Data.Query.OrderBy(pulledPanels, "Name");
            List<Panel> jsonPanels = BH.Engine.Data.Query.OrderBy(m_jsonPanels.ToSpaces().ExternalElements(), "Name");

            //assert correct values
            pulledPanels.Count.Should().Be(jsonPanels.Count, "There was a different number of external panels pulled compared to expected.");
            for (int i = 0; i < jsonPanels.Count; i++)
            {
                pulledPanels[i].Name.Should().Be(jsonPanels[i].Name, "The name of the panel pulled was not the same as the json panel.");
                pulledPanels[i].IsIdentical(jsonPanels[i]).Should().BeTrue($"The panel with name {pulledPanels[i].Name} was not identical to the json panel with the same name.");
            }
            constructions.Count.Should().Be(0, "No constructions should be pulled, but some were pulled anyway.");

            objs.Count.Should().Be(42, "There was a different number of objects pulled compared to expected.");
        }

        [Test]
        [Description("Test for pushing GBXML with different units set.")]
        public void PushGBXMLUnits()
        {
            m_config.Settings = new GBXMLSettings()
            {
                IncludeConstructions = false,
                ExportDetail = oM.Adapters.XML.Enums.ExportDetail.Full,
                UnitSetUp = new GBXMLUnitSetUp()
                {
                    TemperatureUnit = TemperatureUnit.Kelvin,
                    LengthUnit = LengthUnit.Feet,
                    AreaUnit = AreaUnit.SquareFeet,
                    VolumeUnit = VolumeUnit.CubicFeet
                }
            };
            
            m_adapter.Push(m_jsonObjects, actionConfig: m_config);

            List<string> lines = File.ReadAllLines(m_config.File.GetFullFileName()).ToList();

            lines[1].Should().Contain("lengthUnit=\"Feet\"", "The length unit was not set to Feet.");
            lines[1].Should().Contain("areaUnit=\"SquareFeet\"", "The area unit was not set to Square Feet.");
            lines[1].Should().Contain("volumeUnit=\"CubicFeet\"", "The volume unit was not set to Cubic Feet.");
            lines[1].Should().Contain("temperatureUnit=\"K\"", "The temperature unit was not set to Kelvin.");
        }
    }
}