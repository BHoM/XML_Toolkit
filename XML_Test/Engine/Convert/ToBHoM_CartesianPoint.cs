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

using BH.oM.XML;

namespace BH.Test.XML
{
    [TestClass]
    public partial class ToBHoM
    {
        [TestMethod]
        public void TestToBHoM_CartesianPoint_3coords()
        {
            //Test for setting 3 coordinates to a cartesian point, 
            //to see if it returns a BHoM cartesian point with the right coordinates.

            //Create cartesian point coordinates
            CartesianPoint pt = new CartesianPoint();
            string[] coords = new string[3];
            coords[0] = "1";
            coords[1] = "2";
            coords[2] = "3";
            pt.Coordinate = coords;

            //Converts to BHoM point
            BH.oM.Geometry.Point bhomPt = BH.Engine.XML.Convert.ToBHoM(pt);

            //See if the coordinates are set up correctly to the point
            Assert.IsTrue(bhomPt.X == 1);
            Assert.IsTrue(bhomPt.Y == 2);
            Assert.IsTrue(bhomPt.Z == 3);
        }

        [TestMethod]
        public void TestToBHoM_CartesianPoint_0coords()
        {
            //Test for 0 coordinates set on cartesian point

            CartesianPoint pt = new CartesianPoint();
            string[] coords = new string[3];

            pt.Coordinate = coords;

            BH.oM.Geometry.Point bhomPt = BH.Engine.XML.Convert.ToBHoM(pt);

            Assert.IsTrue(bhomPt.X == 0);
            Assert.IsTrue(bhomPt.Y == 0);
            Assert.IsTrue(bhomPt.Z == 0);
        }

        [TestMethod]
        public void TestToBHoM_CartesianPoint_1coord()
        {
            //Test for 1 coordinates set on cartesian point

            CartesianPoint pt = new CartesianPoint();
            string[] coords = new string[3];
            coords[0] = "1";

            pt.Coordinate = coords;

            BH.oM.Geometry.Point bhomPt = BH.Engine.XML.Convert.ToBHoM(pt);

            Assert.IsTrue(bhomPt.X == 1);
            Assert.IsTrue(bhomPt.Y == 0);
            Assert.IsTrue(bhomPt.Z == 0);
        }

        [TestMethod]
        public void TestToBHoM_CartesianPoint_2coords()
        {
            //Test for 2 coordinates set on cartesian point

            CartesianPoint pt = new CartesianPoint();
            string[] coords = new string[3];
            coords[0] = "1";
            coords[1] = "2";

            pt.Coordinate = coords;

            BH.oM.Geometry.Point bhomPt = BH.Engine.XML.Convert.ToBHoM(pt);

            Assert.IsTrue(bhomPt.X == 1);
            Assert.IsTrue(bhomPt.Y == 2);
            Assert.IsTrue(bhomPt.Z == 0);
        }

        [TestMethod]
        public void TestToBHoM_CartesianPoint_2coords_2Array_numbers()
        {
            //Test for 3 coordinates set on cartesian point, but the array has only two numbers

            CartesianPoint pt = new CartesianPoint();
            string[] coords = new string[2];
            coords[0] = "1";
            coords[1] = "2";
            
            pt.Coordinate = coords;

            BH.oM.Geometry.Point bhomPt = BH.Engine.XML.Convert.ToBHoM(pt);

            Assert.IsTrue(bhomPt.X == 1);
            Assert.IsTrue(bhomPt.Y == 2);
            Assert.IsTrue(bhomPt.Z == 0);
        }

        [TestMethod]
        public void TestToBHoM_CartesianPoint_double()
        {
            CartesianPoint pt = new CartesianPoint();
            string[] coords = new string[3];
            coords[0] = "1.12345";
            coords[1] = "2.38909";
            coords[2] = "3.84909";
            pt.Coordinate = coords;

            BH.oM.Geometry.Point bhomPt = BH.Engine.XML.Convert.ToBHoM(pt);

            Assert.IsTrue(bhomPt.X == 1.12345);
            Assert.IsTrue(bhomPt.Y == 2.38909);
            Assert.IsTrue(bhomPt.Z == 3.84909);
        }

        [TestMethod]
        public void TestToBHoM_CartesianPoint_stress()
        {
            Random rand = new Random();
            for (int i = 0; i < 1000; i++)
            {
                CartesianPoint pt = new CartesianPoint();
                string[] coords = new string[3];
                coords[0] = (rand.NextDouble() * 100).ToString();
                coords[1] = (rand.NextDouble() * 100).ToString();
                coords[2] = (rand.NextDouble() * 100).ToString();
                pt.Coordinate = coords;

                BH.oM.Geometry.Point bhomPt = BH.Engine.XML.Convert.ToBHoM(pt);
                Assert.IsTrue(bhomPt.X == (System.Convert.ToDouble(coords[0])));
                Assert.IsTrue(bhomPt.Y == (System.Convert.ToDouble(coords[1])));
                Assert.IsTrue(bhomPt.Z == (System.Convert.ToDouble(coords[2])));
            }
        }
    }
}
