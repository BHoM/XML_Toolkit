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
using BH.oM.Geometry;

using System.Linq;

namespace XML_Test.Engine.Convert
{
    [TestClass]
    public class ToBHoM_Polyloop
    {
        [TestMethod]
        public void TestToBHoM_Polyloop()
        {
            //Determines whether a looped polyline created by toBHoM with 5 points actually has it's controlpoints in the set points.   
            Random rand = new Random();
            int number = 5;
            List<CartesianPoint> pts = new List<CartesianPoint>();

            for (int x = 0; x < number; x++)
                pts.Add(Create.CartesianPoint(rand.NextDouble(), rand.NextDouble(), rand.NextDouble()));
            Polyloop pLoop = new Polyloop();
            pLoop.CartesianPoint = pts.ToArray();
            Polyline pLine = BH.Engine.XML.Convert.ToBHoM(pLoop);
            for (int x = 0; x < pLine.ControlPoints.Count - 1; x++) //Count -1 because first point is added twice as end point to pLine which gives list length greater than input length
            {
                Point p = pLine.ControlPoints[x];
                Assert.IsTrue(p.X.ToString().Equals(pts[x].Coordinate[0]));
                Assert.IsTrue(p.Y.ToString().Equals(pts[x].Coordinate[1]));
                Assert.IsTrue(p.Z.ToString().Equals(pts[x].Coordinate[2]));
            }
        }

        [TestMethod]
        public void TestToBHoM_Polyloop_1pts()
        {   
            // 1 point Polyloop works in the method even though it might not geometrically
            Random rand = new Random();
            int number = 1;                                         
            List<CartesianPoint> pts = new List<CartesianPoint>();

            for (int x = 0; x < number; x++)
                pts.Add(Create.CartesianPoint(rand.NextDouble(), rand.NextDouble(), rand.NextDouble()));
            Polyloop pLoop = new Polyloop();
            pLoop.CartesianPoint = pts.ToArray();
            Polyline pLine = BH.Engine.XML.Convert.ToBHoM(pLoop);
            for (int x = 0; x < pLine.ControlPoints.Count - 1; x++)
            {
                Point p = pLine.ControlPoints[x];
                Assert.IsTrue(p.X.ToString().Equals(pts[x].Coordinate[0]));
                Assert.IsTrue(p.Y.ToString().Equals(pts[x].Coordinate[1]));
                Assert.IsTrue(p.Z.ToString().Equals(pts[x].Coordinate[2]));
            }
        }
        
        [TestMethod]
        public void TestToBHoM_Polyloop_2pts()
        {    
            // 2 point Polyloop works in the method even though it might not geometrically
            Random rand = new Random();
            int number = 2;                                     
            List<CartesianPoint> pts = new List<CartesianPoint>();
            
            for (int x = 0; x < number; x++)
                pts.Add(Create.CartesianPoint(rand.NextDouble(), rand.NextDouble(), rand.NextDouble()));
            Polyloop pLoop = new Polyloop();
            pLoop.CartesianPoint = pts.ToArray();
            Polyline pLine = BH.Engine.XML.Convert.ToBHoM(pLoop);
            for (int x = 0; x < pLine.ControlPoints.Count - 1; x++) 
            {
                Point p = pLine.ControlPoints[x];
                Assert.IsTrue(p.X.ToString().Equals(pts[x].Coordinate[0]));
                Assert.IsTrue(p.Y.ToString().Equals(pts[x].Coordinate[1]));
                Assert.IsTrue(p.Z.ToString().Equals(pts[x].Coordinate[2]));
            }
        }

        [TestMethod]
        public void TestToBHoM_Polyloop_FirstLast()
        {
            // Determines if the first and last point of the polyloop are the same point (as they should be).
            Random rand = new Random();
            int number = 10;                                         
            List<CartesianPoint> pts = new List<CartesianPoint>();

            for (int x = 0; x < number; x++)
                pts.Add(Create.CartesianPoint(rand.NextDouble(), rand.NextDouble(), rand.NextDouble()));
            Polyloop pLoop = new Polyloop();
            pLoop.CartesianPoint = pts.ToArray();
            Polyline pLine = BH.Engine.XML.Convert.ToBHoM(pLoop);
            for (int x = 0; x < pLine.ControlPoints.Count - 1; x++)
            {
                Point p = pLine.ControlPoints[x];
                Assert.IsTrue(pLine.ControlPoints.Last().X.ToString().Equals(pLine.ControlPoints.First().X.ToString()));
                Assert.IsTrue(pLine.ControlPoints.Last().Y.ToString().Equals(pLine.ControlPoints.First().Y.ToString()));
                Assert.IsTrue(pLine.ControlPoints.Last().Z.ToString().Equals(pLine.ControlPoints.First().Z.ToString()));
            }
        }

        [TestMethod]
        public void TestToBHoM_Polyloop_Stress_1000pts()
        {
            // Testing one loop with 1000 points 
            Random rand = new Random();
            int number = 1000;
            List<CartesianPoint> pts = new List<CartesianPoint>();

            for (int x = 0; x < number; x++)
                pts.Add(Create.CartesianPoint(rand.NextDouble()*100, rand.NextDouble() * 100, rand.NextDouble() * 100));
            Polyloop pLoop = new Polyloop();
            pLoop.CartesianPoint = pts.ToArray();
            Polyline pLine = BH.Engine.XML.Convert.ToBHoM(pLoop);
            for (int x = 0; x < pLine.ControlPoints.Count - 1; x++)
            {
                Point p = pLine.ControlPoints[x];
                Assert.IsTrue(p.X.ToString().Equals(pts[x].Coordinate[0]));
                Assert.IsTrue(p.Y.ToString().Equals(pts[x].Coordinate[1]));
                Assert.IsTrue(p.Z.ToString().Equals(pts[x].Coordinate[2]));
            }
        }

        [TestMethod]
        public void TestToBHoM_Polyloop_Stress_1000loops()
        { 
            // Testing 1000 loops with 5 points each
            Random rand = new Random();
            int number = 5;
            for (int i = 0; i < 1000; i++)
            {
                List<CartesianPoint> pts = new List<CartesianPoint>();
                for (int j = 0; j < number; j++) 
                    pts.Add(Create.CartesianPoint(rand.NextDouble() * 100, rand.NextDouble() * 100, rand.NextDouble() * 100));
                Polyloop pLoop = new Polyloop();
                pLoop.CartesianPoint = pts.ToArray();
                Polyline pLine = BH.Engine.XML.Convert.ToBHoM(pLoop);
                for (int k = 0; k < pLine.ControlPoints.Count - 1; k++)
                {
                    Point p = pLine.ControlPoints[k];
                    Assert.IsTrue(p.X.ToString().Equals(pts[k].Coordinate[0]));
                    Assert.IsTrue(p.Y.ToString().Equals(pts[k].Coordinate[1]));
                    Assert.IsTrue(p.Z.ToString().Equals(pts[k].Coordinate[2]));
                }
            }
        }
    }
}
