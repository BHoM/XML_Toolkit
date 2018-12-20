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
    public class ToGBXMLTypeSurfaceType
    {
        [TestMethod]
        public void SurfaceType_Rooflight()
        {
            string x = "Rooflight";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("OperableSkylight"));
        }

        [TestMethod]
        public void SurfaceType_Roof()
        {
            string x = "Roof";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("Roof"));
        }

        [TestMethod]
        public void SurfaceType_ExternalWall()
        {
            string x = "External Wall";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("ExteriorWall"));
        }

        [TestMethod]
        public void SurfaceType_InternalWall()
        {
            string x = "Internal Wall";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("InteriorWall"));
        }

        [TestMethod]
        public void SurfaceType_InternalFloor()
        {
            string x = "Internal Floor";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("InteriorFloor"));
        }

        [TestMethod]
        public void SurfaceType_Shade()
        {
            string x = "Shade";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("Shade"));
        }

        [TestMethod]
        public void SurfaceType_UndergroundWall()
        {
            string x = "Underground Wall";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("UndergroundWall"));
        }

        [TestMethod]
        public void SurfaceType_UndergroundSlab()
        {
            string x = "Underground Slab";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("UndergroundSlab"));
        }

        [TestMethod]
        public void SurfaceType_InternalCeiling()
        {
            string x = "Internal Ceiling";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("Ceiling"));
        }

        [TestMethod]
        public void SurfaceType_UndergroundCeiling()
        {
            string x = "Underground Ceiling";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("UndergroundCeiling"));
        }

        [TestMethod]
        public void SurfaceType_RaisedFloor()
        {
            string x = "Raised Floor";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("RaisedFloor"));
        }

        [TestMethod]
        public void SurfaceType_SlabOnGrade()
        {
            string x = "Slab on Grade";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("SlabOnGrade"));
        }

        [TestMethod]
        public void SurfaceType_CurtainWall()
        {
            string x = "Curtain Wall";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("ExteriorWall"));
        }

        [TestMethod]
        public void SurfaceType_ExposedFloor()
        {
            string x = "Exposed Floor";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("RaisedFloor"));
        }

        [TestMethod]
        public void SurfaceType_NoType()
        {
            string x = "No Type";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("Air"));
        }

        [TestMethod]
        public void SurfaceType_ExternalWall_TAS()
        {
            string x = "EXTERNALWALL";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("ExteriorWall"));
        }

        [TestMethod]
        public void SurfaceType_InternalWall_TAS()
        {
            string x = "INTERNALWALL";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("InteriorWall"));
        }

        [TestMethod]
        public void SurfaceType_RoofElement_TAS()
        {
            string x = "ROOFELEMENT";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("Roof"));
        }

        [TestMethod]
        public void SurfaceType_InternalFloor_TAS()
        {
            string x = "INTERNALFLOOR";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("InteriorFloor"));
        }

        [TestMethod]
        public void SurfaceType_ExposedFloor_TAS()
        {
            string x = "EXPOSEDFLOOR";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("ExposedFloor"));
        }

        [TestMethod]
        public void SurfaceType_ShadeElement_TAS()
        {
            string x = "SHADEELEMENT";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("Shade"));
        }

        [TestMethod]
        public void SurfaceType_UndergroundWall_TAS()
        {
            string x = "UNDERGROUNDWALL";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("UndergroundWall"));
        }

        [TestMethod]
        public void SurfaceType_UndergroundSlab_TAS()
        {
            string x = "UNDERGROUNDSLAB";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("UndergroundSlab"));
        }

        [TestMethod]
        public void SurfaceType_Ceiling_TAS()
        {
            string x = "CEILING";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("Ceiling"));
        }

        [TestMethod]
        public void SurfaceType_UndergroundCeiling_TAS()
        {
            string x = "UNDERGROUNDCEILING";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("UndergroundCeiling"));
        }

        [TestMethod]
        public void SurfaceType_RaisedFloor_TAS()
        {
            string x = "Raised Floor";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("RaisedFloor"));
        }

        [TestMethod]
        public void SurfaceType_SlabOnGrade_TAS()
        {
            string x = "SLABONGRADE";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("SlabOnGrade"));
        }

        [TestMethod]
        public void SurfaceType_Glazing()
        {
            string x = "Glazing";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("FixedWindow"));
        }

        [TestMethod]
        public void SurfaceType_Door()
        {
            string x = "Door";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("NonSlidingDoor"));
        }

        [TestMethod]
        public void SurfaceType_Default()
        {
            string x = "null";
            Assert.IsTrue(x.ToGBXMLSurfaceType().Equals("Air"));
        }
    }
}