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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Base;
using BHE = BH.oM.Environment.Elements;
using BH.oM.Environment.Fragments;
using BHG = BH.oM.Geometry;
using BH.Engine;

using BH.Engine.XML;
using BHX = BH.oM.XML;
using BHA = BH.oM.Architecture.Elements;
using BHC = BH.oM.Physical.Constructions;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {
        
        protected override IEnumerable<IBHoMObject> Read(Type type, IList indices = null)
        {
            return Read(type);
        }

        private IEnumerable<IBHoMObject> Read(Type type = null)
        {
            BH.oM.XML.GBXML gbx = XMLReader.Load(FilePath, ProjectName);

            if (type == null)
                return ReadFullXMLFile(gbx);
            else if (type == typeof(BHE.Building))
                return ReadBuilding(gbx);
            else if (type == typeof(BHE.Panel))
                return ReadPanels(gbx);
            /*else if (type == typeof(BHC.Construction))
                return ReadConstructions(gbx);
            else if (type == typeof(BH.oM.Physical.Properties.Material))
                return ReadMaterials(gbx);*/
            else if (type == typeof(BHA.Level))
                return ReadLevels(gbx);
            else
                return ReadFullXMLFile(gbx);            
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<IBHoMObject> ReadFullXMLFile(BH.oM.XML.GBXML gbx, List<string> ids = null)
        {
            List<IBHoMObject> objects = new List<IBHoMObject>();

            objects.AddRange(ReadBuilding(gbx));
            objects.AddRange(ReadPanels(gbx));
            //objects.AddRange(ReadConstructions(gbx));
            //objects.AddRange(ReadMaterials(gbx));
            objects.AddRange(ReadLevels(gbx));

            return objects;
        }

        private List<BHE.Building> ReadBuilding(BHX.GBXML gbx, List<string> ids = null)
        {
            if (gbx.Campus != null)
                return new List<BHE.Building>() { gbx.Campus.Location.ToBHoM() };
            else
                return new List<BHE.Building>();
        }

        private List<BHE.Panel> ReadPanels(BHX.GBXML gbx, List<string> ids = null)
        {
            if (gbx.Campus != null && gbx.Campus.Surface != null)
                return gbx.Campus.Surface.Select(x => x.ToBHoM()).ToList();
            else
                return new List<BHE.Panel>();
        }

        /*private List<BHC.Construction> ReadConstructions(BHX.GBXML gbx, List<string> ids = null)
        {
            if (gbx.Construction != null)
                return gbx.Construction.Select(x => x.ToBHoM()).ToList();
            else
                return new List<BHC.Construction>();
        }

        private List<BH.oM.Physical.Properties.Material> ReadMaterials(BHX.GBXML gbx, List<string> ids = null)
        {
            if (gbx.Material != null)
                return gbx.Material.Select(x => x.ToBHoM()).ToList();
            else
                return new List<BH.oM.Environment.Materials.Material>();
        }*/

        private List<BHA.Level> ReadLevels(BHX.GBXML gbx, List<string> ids = null)
        {
            if (gbx.Campus.Building.Length > 0)
                return gbx.Campus.Building[0].BuildingStorey.Select(x => x.ToBHoM()).ToList();
            else
                return new List<BHA.Level>();
        }
    }
}