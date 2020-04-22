/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using System.Xml.Serialization;
using System.IO;

using BH.oM.Base;
using BHE = BH.oM.Environment.Elements;
using BH.oM.Environment.Fragments;
using BHG = BH.oM.Geometry;

using BH.oM.External.XML;
using BH.oM.External.XML.Enums;
using BHX = BH.oM.External.XML.GBXML;
using BHC = BH.oM.Physical.Constructions;

using BH.oM.Adapter;
using BH.Engine.Adapter;

namespace BH.Adapter.XML
{
    public partial class XMLAdapter : BHoMAdapter
    {

        private IEnumerable<IBHoMObject> ReadGBXML(Type type = null, XMLConfig config = null)
        {
            BH.oM.External.XML.GBXML.GBXML gbx = null;
            TextReader reader = new StreamReader(_fileSettings.GetFullFileName());
            XmlSerializer szer = new XmlSerializer(typeof(BH.oM.External.XML.GBXML.GBXML));
            gbx = (BH.oM.External.XML.GBXML.GBXML)szer.Deserialize(reader);
            reader.Close();

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
            else if (type == typeof(BHG.SettingOut.Level))
                return ReadLevels(gbx);
            else
                return ReadFullXMLFile(gbx);
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<IBHoMObject> ReadFullXMLFile(BH.oM.External.XML.GBXML.GBXML gbx, List<string> ids = null)
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
                    foreach (BHX.Space space in b.Space)
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
                return new List<BHE.Building>() { gbx.Campus.Location.FromGBXML() };
            else
                return new List<BHE.Building>();
        }

        private List<BHE.Panel> ReadPanels(BHX.GBXML gbx, List<string> ids = null)
        {
            List<BHE.Panel> panels = new List<BHE.Panel>();

            if (gbx.Campus != null && gbx.Campus.Surface != null)
            {
                foreach (BHX.Surface s in gbx.Campus.Surface)
                {
                    BHE.Panel p = s.FromGBXML();
                    if (gbx.Construction != null)
                    {
                        BHX.Construction c = gbx.Construction.Where(x => x.ID == s.ConstructionIDRef).FirstOrDefault();
                        if (c != null)
                        {
                            BHX.Layer gbLayer = gbx.Layer.Where(x => x.ID == c.LayerID.LayerIDRef).FirstOrDefault();
                            if (gbLayer != null && gbLayer.MaterialID != null)
                            {
                                List<BHX.Material> gbMaterials = gbx.Material.Where(x => gbLayer.MaterialID.Where(y => y.MaterialIDRef == x.ID).FirstOrDefault() != null).ToList();
                                if (gbMaterials.Count > 0)
                                {
                                    List<BHC.Layer> layers = gbMaterials.Select(x => x.FromGBXML()).ToList();
                                    p.Construction = c.FromGBXML(layers);
                                }
                            }
                        }
                    }

                    foreach (BHX.Opening gbxOpening in s.Opening)
                        p.Openings.Add(gbxOpening.FromGBXML());
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
                if (gbx.Layer != null)
                {
                    foreach (BHX.Construction c in gbx.Construction)
                    {
                        BHX.Layer gbLayer = gbx.Layer.Where(x => x.ID == c.LayerID.LayerIDRef).FirstOrDefault();
                        if (gbLayer != null && gbLayer.MaterialID != null)
                        {
                            List<BHX.Material> gbMaterials = gbx.Material.Where(x => gbLayer.MaterialID.Where(y => y.MaterialIDRef == x.ID).FirstOrDefault() != null).ToList();
                            if (gbMaterials.Count > 0)
                            {
                                List<BHC.Layer> layers = gbMaterials.Select(x => x.FromGBXML()).ToList();
                                constructions.Add(c.FromGBXML(layers));
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

        private List<BHG.SettingOut.Level> ReadLevels(BHX.GBXML gbx, List<string> ids = null)
        {
            if (gbx.Campus.Building.Length > 0)
                return gbx.Campus.Building[0].BuildingStorey.Select(x => x.FromGBXML()).ToList();
            else
                return new List<BHG.SettingOut.Level>();
        }
    }
}