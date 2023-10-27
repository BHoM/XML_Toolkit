using NUnit.Framework;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Adapter.XML;
using BH.oM.Adapters.XML;
using BH.oM.Adapter;
using BH.Engine.Adapter;
using BH.oM.Adapters.XML.Settings;
using BH.oM.Base;
using BH.oM.Data.Requests;
using BH.oM.Environment.Elements;
using BH.oM.Physical.Constructions;
using BH.Engine.Environment;

namespace BH.Tests.Adapter.XML
{
    public class PushTests
    {
        XMLAdapter m_adapter;
        XMLConfig m_config;

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
            };
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
            m_config.Schema = oM.Adapters.XML.Enums.Schema.GBXML;
            m_config.Settings = new GBXMLSettings() { IncludeConstructions = true };
            FilterRequest request = new FilterRequest();

            List<IBHoMObject> jsonObjs = BH.Engine.Adapters.File.Compute.ReadFromJsonFile(Path.Combine(m_config.File.Directory, "TestModel.json"), true).Cast<IBHoMObject>().ToList();

            //Push, then pull objects.
            m_adapter.Push(jsonObjs, actionConfig: m_config);
            List<IBHoMObject> objs = m_adapter.Pull(request, actionConfig: m_config).Cast<IBHoMObject>().ToList();

            List<Panel> pulledPanels = BH.Engine.Environment.Query.Panels(objs);
            List<Panel> jsonPanels = BH.Engine.Environment.Query.Panels(jsonObjs);
            List<Construction> constructions = objs.Where(x => x.GetType() == typeof(Construction)).Cast<Construction>().ToList();
            List<Construction> jsonConstructions = objs.Where(x => x.GetType() == typeof(Construction) && x.Name == "generic_construction").Cast<Construction>().ToList();

            pulledPanels = BH.Engine.Data.Query.OrderBy(pulledPanels, "Name");
            jsonPanels = BH.Engine.Data.Query.OrderBy(jsonPanels, "Name");

            //assert correct values
            pulledPanels.Count.Should().Be(jsonPanels.Count, "There was a different number of panels pushed then pulled compared to expected.");
            for (int i = 0; i < pulledPanels.Count; i++)
            {
                pulledPanels[i].Name.Should().Be(jsonPanels[i].Name, "The name of the panel pulled was not the same as the json panel.");
                pulledPanels[i].IsIdentical(jsonPanels[i]).Should().BeTrue("The panel with name {pulledPanels[i].Name} was not identical to the json panel with the same name.");
            }
            constructions.Count.Should().Be(jsonConstructions.Count, "There is only one type of construction (generic_construction) used in the building.");

            objs.Count.Should().Be(jsonObjs.Count-3, "The number of pulled objects should be the same as the number of json objects, minus unused constructions.");
        }

        [Test]
        [Description("Test for pushing a set of BHoM environment objects to xml with constructions set to false.")]
        public void PushGBXMLNoConstructions()
        {
            m_config.Schema = oM.Adapters.XML.Enums.Schema.GBXML;
            m_config.Settings = new GBXMLSettings() { IncludeConstructions = false };
            FilterRequest request = new FilterRequest();

            List<IBHoMObject> jsonObjs = BH.Engine.Adapters.File.Compute.ReadFromJsonFile(Path.Combine(m_config.File.Directory, "TestModel.json"), true).Cast<IBHoMObject>().ToList();

            //Push, then pull objects.
            m_adapter.Push(jsonObjs, actionConfig: m_config);
            List<IBHoMObject> objs = m_adapter.Pull(request, actionConfig: m_config).Cast<IBHoMObject>().ToList();

            List<Panel> pulledPanels = BH.Engine.Environment.Query.Panels(objs);
            List<Panel> jsonPanels = BH.Engine.Environment.Query.Panels(jsonObjs);
            List<Construction> constructions = objs.Where(x => x.GetType() == typeof(Construction)).Cast<Construction>().ToList();

            pulledPanels = BH.Engine.Data.Query.OrderBy(pulledPanels, "Name");
            jsonPanels = BH.Engine.Data.Query.OrderBy(jsonPanels, "Name");

            //assert correct values
            pulledPanels.Count.Should().Be(jsonPanels.Count, "There was a different number of panels pushed then pulled compared to expected.");
            for (int i = 0; i < pulledPanels.Count; i++)
            {
                pulledPanels[i].Name.Should().Be(jsonPanels[i].Name, "The name of the panel pulled was not the same as the json panel.");
                pulledPanels[i].IsIdentical(jsonPanels[i]).Should().BeTrue("The panel with name {pulledPanels[i].Name} was not identical to the json panel with the same name.");
            }
            constructions.Count.Should().Be(0, "No constructions are being pulled.");

            objs.Count.Should().Be(jsonObjs.Count - 4, "The number of pulled objects should be the same as the number of json objects, minus constructions.");
        }

        [Test]
        [Description("Test for pushing GBXML with exportDetail set to BuildingShell")]
        public void PushGBXMLBuildingShell()
        {
            m_config.Schema = oM.Adapters.XML.Enums.Schema.GBXML;
            m_config.Settings = new GBXMLSettings()
            {
                IncludeConstructions = false,
                ExportDetail = oM.Adapters.XML.Enums.ExportDetail.BuildingShell
            };
        }
    }
}