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
using BHM = BH.oM.Physical.Materials;

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
            BH.oM.XML.GBXML gbx = XMLReader.Load(_fileSettings.FullFileName());

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
            objects.AddRange(ReadConstructions(gbx));
            //objects.AddRange(ReadMaterials(gbx));
            objects.AddRange(ReadLevels(gbx));
            objects.AddRange(ReadSpaces(gbx));

            return objects;
        }

        private List<BHE.Space> ReadSpaces(BHX.GBXML gbx, List<string> ids = null)
        {
            //ToDo - Fix this!
            List<BHE.Space> s = new List<BHE.Space>();

            if (gbx.Campus != null)
            {
                foreach (BHX.Building b in gbx.Campus.Building)
                {
                    foreach(BHX.Space space in b.Space)
                    {
                        BHE.Space bhomS = new oM.Environment.Elements.Space();
                        bhomS.Name = space.Name;
                        OriginContextFragment f = new OriginContextFragment();
                        f.ElementID = space.ID;
                        bhomS.Fragments.Add(f);
                        s.Add(bhomS);
                    }
                }
            }

            return s;
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
            List<BHE.Panel> panels = new List<BHE.Panel>();

            if(gbx.Campus != null && gbx.Campus.Surface != null)
            {
                foreach(BHX.Surface s in gbx.Campus.Surface)
                {
                    BHE.Panel p = s.ToBHoM();
                    if (gbx.Construction != null)
                    {
                        BHX.Construction c = gbx.Construction.Where(x => x.ID == s.ConstructionIDRef).FirstOrDefault();
                        if (c != null)
                        {
                            BHX.Layer gbLayer = gbx.Layer.Where(x => x.ID == c.LayerID.LayerIDRef).FirstOrDefault();
                            if (gbLayer != null)
                            {

                                List<BHX.Material> gbMaterials = gbx.Material.Where(x => gbLayer.MaterialID.Where(y => y.MaterialIDRef == x.ID).FirstOrDefault() != null).ToList();
                                if (gbMaterials.Count > 0)
                                {
                                    List<BHC.Layer> layers = gbMaterials.Select(x => x.ToBHoM()).ToList();
                                    p.Construction = c.ToBHoM(layers);
                                }
                            }
                        }
                    }

                    foreach (BHX.Opening gbxOpening in s.Opening)
                        p.Openings.Add(gbxOpening.ToBHoM());
                    panels.Add(p);
                }
            }

            return panels;
        }

        private List<BHC.Construction> ReadConstructions(BHX.GBXML gbx, List<string> ids = null)
        {
            List<BHC.Construction> constructions = new List<BHC.Construction>();

            if (gbx.Construction != null)
            {
                if(gbx.Layer != null)
                {
                    foreach(BHX.Construction c in gbx.Construction)
                    {
                        BHX.Layer gbLayer = gbx.Layer.Where(x => x.ID == c.LayerID.LayerIDRef).FirstOrDefault();
                        if(gbLayer != null)
                        {

                            List<BHX.Material> gbMaterials = gbx.Material.Where(x => gbLayer.MaterialID.Where(y => y.MaterialIDRef == x.ID).FirstOrDefault() != null).ToList();
                            if(gbMaterials.Count > 0)
                            {
                                List<BHC.Layer> layers = gbMaterials.Select(x => x.ToBHoM()).ToList();
                                constructions.Add(c.ToBHoM(layers));
                            }
                        }
                    }
                }
            }

            return constructions;
        }

        /*private List<BH.oM.Physical.Properties.Material> ReadMaterials(BHX.GBXML gbx, List<string> ids = null)
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