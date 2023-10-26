using BH.Adapter.XML;
using BH.oM.Adapter;
using BH.oM.Adapters.XML;
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

        }

        [Test]
        [Description("Test pulling CSProject XML file.")]
        public void PullCSProject()
        {

        }

        [Test]
        [Description("Test pulling Default XML file.")]
        public void PullDefault()
        {

        }

        [Test]
        [Description("Test pulling EnergyPlus XML file.")]
        public void PullEnergyPlus()
        {

        }

        [Test]
        [Description("Test pulling GBXML file.")]
        public void PullGBXML()
        {

        }

        [Test]
        [Description("Test pulling KML file.")] //should return empty list
        public void PullKML()
        {

        }
    }
}