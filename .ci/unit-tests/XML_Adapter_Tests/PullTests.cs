using BH.Adapter.XML;
using BH.Adapter.XML.GBXMLSchema;
using BH.oM.Adapter;
using BH.oM.Adapters.XML;
using BH.oM.Adapters.XML.Settings;
using BH.oM.Base;
using BH.oM.Data.Requests;
using BH.oM.XML.Bluebeam;
using BH.oM.XML.CSProject;
using BH.oM.XML.EnergyPlus;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [Description("Test pulling Default XML file.")]
        public void PullDefault()
        {
            var String = "Pass";
            String.Should().Be("Fail");
        }

        [Test]
        [Description("Test pulling EnergyPlus XML file.")]
        public void PullEnergyPlus() //do this
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
        public void PullGBXML() //do this
        {
            m_config.Schema = oM.Adapters.XML.Enums.Schema.GBXML;
            m_config.File.FileName = "GBXMLTest.xml";
            m_config.Settings = new GBXMLSettings();
            FilterRequest request = new FilterRequest();

            List<IBHoMObject> objs = m_Adapter.Pull(request, actionConfig: m_config).Cast<IBHoMObject>().ToList();

            objs.Count.Should().Be(73, "Wrong number of objects pulled compared to expected.");
        }
    }
}