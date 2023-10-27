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
                File = new FileSettings() { Directory = ModelsPath, FileName = "PushedModel.xml"}
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
    }
}
