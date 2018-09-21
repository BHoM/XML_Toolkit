using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BH.Adapter.XML;
using BH.Engine.XML;

using BH.oM.Environment.Elements;

using System.Collections.Generic;

using BH.oM.XML;

namespace BH.Test.XML
{
    [TestClass]
    public partial class Create
    {
        [TestMethod]
        public void TestCreate_CartesianPoint_String()
        {
            String x = "1";
            String y = "2";
            String z = "3";
            CartesianPoint pt = BH.Engine.XML.Create.CartesianPoint(x, y, z);
            Assert.IsTrue(pt.Coordinate[0].Equals(x));
            Assert.IsTrue(pt.Coordinate[1].Equals(y));
            Assert.IsTrue(pt.Coordinate[2].Equals(z));
        }

        [TestMethod]
        public void TestCreate_CartesianPoint_StringDouble()
        {
            String x = "1.123";
            String y = "2.231";
            String z = "3.312";
            CartesianPoint pt = BH.Engine.XML.Create.CartesianPoint(x, y, z);
            Assert.IsTrue(pt.Coordinate[0].Equals(x));
            Assert.IsTrue(pt.Coordinate[1].Equals(y));
            Assert.IsTrue(pt.Coordinate[2].Equals(z));
        }

        [TestMethod]
        public void TestCreate_CartesianPoint_Integer()
        {
            int x = 1;
            int y = 1;
            int z = 1;
            CartesianPoint pt = BH.Engine.XML.Create.CartesianPoint(x, y, z);
            Assert.IsTrue(pt.Coordinate[0].Equals(x.ToString()));
            Assert.IsTrue(pt.Coordinate[1].Equals(y.ToString()));
            Assert.IsTrue(pt.Coordinate[2].Equals(z.ToString()));
        }

        [TestMethod]
        public void TestCreate_CartesianPoint_Double()
        {
            double x = 1;
            double y = 1;
            double z = 1;
            CartesianPoint pt = BH.Engine.XML.Create.CartesianPoint(x, y, z);
            Assert.IsTrue(pt.Coordinate[0].Equals(x.ToString()));
            Assert.IsTrue(pt.Coordinate[1].Equals(y.ToString()));
            Assert.IsTrue(pt.Coordinate[2].Equals(z.ToString()));
        }
        
        [TestMethod]
        public void TestCreate_CartesianPoint_Stress()
        {
            Random rand = new Random();
            for (int i = 0; i < 1000; i++)
            {
                double x = (rand.NextDouble() * 100);
                double y = (rand.NextDouble() * 100);
                double z = (rand.NextDouble() * 100);
                CartesianPoint pt = BH.Engine.XML.Create.CartesianPoint(x, y, z);
                Assert.IsTrue(pt.Coordinate[0].Equals(x.ToString()));
                Assert.IsTrue(pt.Coordinate[1].Equals(y.ToString()));
                Assert.IsTrue(pt.Coordinate[2].Equals(z.ToString()));
            }
        }
    }
}
