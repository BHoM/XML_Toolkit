/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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
