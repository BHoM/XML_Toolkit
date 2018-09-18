using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BH.Adapter.XML;
using BH.Engine.XML;

using BH.oM.Environment.Elements;

using System.Collections.Generic;

namespace BH.Test.XML
{
    [TestClass]
    public class MapByGUID
    {
        [TestMethod]
        public void TestMapByGUID_1()
        {
            //Test to check a null list returns a null object
            List<BuildingElement> elements = null;
            Assert.IsNull(elements.MapByGUID());
        }

        [TestMethod]
        public void TestMapByGUID_2()
        {
            //Test if building elements are mapped by their GUID
            List<BuildingElement> elements = new List<BuildingElement>();
            elements.Add(new BuildingElement());
            elements.Add(new BuildingElement());
            elements.Add(new BuildingElement());

            elements[0].BHoM_Guid = elements[1].BHoM_Guid;

            List<List<BuildingElement>> rtn = elements.MapByGUID();

            Assert.IsNotNull(rtn);
            Assert.IsTrue(rtn.Count == 2);
            Assert.IsTrue(rtn[0].Count == 2);
            Assert.IsTrue(rtn[1].Count == 1);
            Assert.IsTrue(rtn[0][0].BHoM_Guid == rtn[0][1].BHoM_Guid);
            Assert.IsTrue(rtn[0][1].BHoM_Guid != rtn[1][0].BHoM_Guid);
        }
    }
}
