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

using BH.Adapter.XML;
using BH.oM.Adapter;
using BH.Engine.Adapter;
using BH.oM.Adapters.XML;
using BH.oM.Adapters.XML.Settings;
using BH.oM.Base;
using BH.oM.Data.Requests;
using BH.oM.XML.Bluebeam;
using BH.oM.XML.EnergyPlus;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Environment.Elements;
using BH.Engine.Environment;
using BH.oM.Physical.Constructions;

namespace BH.Tests.Adapter.XML
{
    public class PullTests
    {
        XMLAdapter m_Adapter;
        XMLConfig m_config;

        [OneTimeSetUp]
        [Description("On loading tests, instantiate an adapter and pull config to be used in all tests, and get the file path to the test files.")]
        public void OneTimeSetUp()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            List<string> paths = currentDirectory.Split('\\').ToList();
            paths = paths.Take(paths.IndexOf(".ci") + 2).ToList();
            string ModelsPath = Path.Join(string.Join("\\", paths), "Models");
            m_Adapter = new XMLAdapter();
            m_config = new XMLConfig()
            {
                File = new FileSettings() { Directory = ModelsPath },
            };
        }

        [SetUp]
        [Description("When running a new test, clear any errors that have occurred in the previous test.")]
        public void Setup()
        {
            BH.Engine.Base.Compute.ClearCurrentEvents();
        }

        [TearDown]
        [Description("After each test, log types and messages of any BHoM events raised during the test.")]
        public void TearDown()
        {
            var events = BH.Engine.Base.Query.CurrentEvents();
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
        [Description("Test pulling BlueBeam XML file.")]
        public void PullBlueBeam()
        {
            m_config.Schema = oM.Adapters.XML.Enums.Schema.Bluebeam;
            m_config.File.FileName = "BluebeamMarkup.xml";
            FilterRequest request = new FilterRequest();

            MarkupSummary markupSummary = m_Adapter.Pull(request, actionConfig: m_config).Cast<MarkupSummary>().ToList()[0];

            List<Markup> markups = markupSummary.Markup;
            List<Markup> openings = markups.Where(x => x.Layer == "Openings").ToList();

            List<oM.Base.Debugging.Event> events = BH.Engine.Base.Query.CurrentEvents();

            markups.Count.Should().Be(8, "Wrong number of markups pulled compared to expected.");
            openings.Count.Should().Be(3, "Wrong number of openings pulled compared to expected.");
            events.Count.Should().Be(0, "There were errors when serialising the xml, see console for more details on the error.");
        }

        [Test]
        [Description("Test pulling a Default XML file, using the BlueBeamMarkup.xml file.")]
        public void PullDefault()
        {
            m_config.Schema = oM.Adapters.XML.Enums.Schema.Undefined;
            m_config.File.FileName = "BlueBeamMarkup.xml";
            FilterRequest request = new FilterRequest() { Type = typeof(MarkupSummary) };

            MarkupSummary markupSummary = m_Adapter.Pull(request, actionConfig: m_config).Cast<MarkupSummary>().ToList()[0];

            List<Markup> markups = markupSummary.Markup;
            List<Markup> openings = markups.Where(x => x.Layer == "Openings").ToList();

            List<oM.Base.Debugging.Event> events = BH.Engine.Base.Query.CurrentEvents();

            markups.Count.Should().Be(8, "Wrong number of markups pulled compared to expected.");
            openings.Count.Should().Be(3, "Wrong number of openings pulled compared to expected.");
            events.Count.Should().Be(0, "There were errors when serialising the xml, see console for more details on the error.");
        }

        [Test]
        [Description("Test pulling EnergyPlus XML file.")]
        public void PullEnergyPlus()
        {
            m_config.Schema = oM.Adapters.XML.Enums.Schema.EnergyPlusLoads;
            m_config.File.FileName = "EnergyPlusLoadsTest.xml";
            FilterRequest request = new FilterRequest();

            EnergyPlusTabularReport report = m_Adapter.Pull(request, actionConfig: m_config).Cast<EnergyPlusTabularReport>().ToList()[0];

            double dryBulb = double.Parse(report.ZoneComponentLoadSummary[0].CoolingPeakConditions.OutsideDryBulbTemperature.Value);
            double wetBulb = double.Parse(report.ZoneComponentLoadSummary[0].CoolingPeakConditions.OutsideWetBulbTemperature.Value);
            double relHum = double.Parse(report.ZoneComponentLoadSummary[0].CoolingPeakConditions.ZoneRelativeHumdity.Value);

            dryBulb.Should().Be(31.15);
            wetBulb.Should().Be(31.09);
            relHum.Should().Be(42.55);
        }

        [Test]
        [Description("Test pulling GBXML file.")]
        public void PullGBXML()
        {
            m_config.Schema = oM.Adapters.XML.Enums.Schema.GBXML;
            m_config.File.FileName = "GBXMLTest.xml";
            m_config.Settings = new GBXMLSettings();
            FilterRequest request = new FilterRequest();

            //the gbXML file and the json file contain the same BHoM objects.
            List<IBHoMObject> pulledObjs = m_Adapter.Pull(request, actionConfig: m_config).Cast<IBHoMObject>().ToList();
            List<IBHoMObject> jsonObjs = BH.Engine.Adapters.File.Compute.ReadFromJsonFile(Path.Combine(m_config.File.Directory, "TestModel.json"), true).Cast<IBHoMObject>().ToList();

            List<Panel> pulledPanels = BH.Engine.Environment.Query.Panels(pulledObjs);
            List<Panel> jsonPanels = BH.Engine.Environment.Query.Panels(jsonObjs);
            List<Building> buildings = BH.Engine.Environment.Query.Buildings(pulledObjs);
            List<Space> spaces = BH.Engine.Environment.Query.Spaces(pulledObjs);
            List<Construction> constructions = pulledObjs.Where(x => x.GetType() == typeof(Construction)).Cast<Construction>().ToList();

            pulledPanels = BH.Engine.Data.Query.OrderBy(pulledPanels, "Name");
            jsonPanels = BH.Engine.Data.Query.OrderBy(jsonPanels, "Name");

            //assert that the pulled panels are the same as the panels deserialised from json
            pulledPanels.Count.Should().Be(jsonPanels.Count, "The number of panels pulled should be the same as the number of panels in the json.");
            for (int i = 0; i< pulledPanels.Count; i++)
            {
                pulledPanels[i].Name.Should().Be(jsonPanels[i].Name, "The names of the pulled panels should be the same as the names of the json panels.");
                pulledPanels[i].IsIdentical(jsonPanels[i]).Should().BeTrue($"The panel pulled with the name {pulledPanels[i].Name} was not the same as the json panel with the same name.");
            }
            buildings.Count.Should().Be(1, "Wrong number of buildings pulled compared to expected.");
            spaces.Count.Should().Be(9, "Wrong number of buildings pulled compared to expected.");
            constructions.Count.Should().Be(4, "Wrong number of constructions pulled compared to expected.");
        }
    }
}

