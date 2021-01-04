/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Environment.Elements;
using BH.oM.Adapters.XML.Settings;
using GBXML = BH.Adapter.XML.GBXMLSchema;
using BH.oM.Geometry.SettingOut;
using BH.oM.Geometry;

using BH.Engine.Geometry;
using BH.Engine.Environment;
using BH.oM.Environment.Fragments;

namespace BH.Adapter.XML
{
    public static partial class Convert
    {
        public static GBXML.Space ToGBXML(this List<Panel> panelsAsSpace, Level spaceLevel, GBXMLSettings settings)
        {
            GBXML.Space xmlSpace = new GBXML.Space();

            xmlSpace.Name = panelsAsSpace.ConnectedSpaceName();
            xmlSpace.ID = "Space" + xmlSpace.Name.Replace(" ", "").Replace("-", "");
            xmlSpace.CADObjectID = BH.Engine.Adapters.XML.Query.CADObjectID(panelsAsSpace);

            List<Polyline> shellGeometry = panelsAsSpace.ClosedShellGeometry();
            List<GBXML.Polyloop> loopShell = new List<GBXML.Polyloop>();
            foreach (Polyline p in shellGeometry)
            {
                if (p.NormalAwayFromSpace(panelsAsSpace, settings.PlanarTolerance))
                    loopShell.Add(p.ToGBXML(settings));
                else
                    loopShell.Add(p.Flip().ToGBXML(settings));
            }

            xmlSpace.ShellGeometry.ClosedShell.PolyLoop = loopShell.ToArray();
            xmlSpace.ShellGeometry.ID = "SpaceShellGeometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            xmlSpace.SpaceBoundary = SpaceBoundaries(panelsAsSpace, settings, settings.PlanarTolerance);
            xmlSpace.PlanarGeoemtry.ID = "SpacePlanarGeometry-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            if (BH.Engine.Environment.Query.FloorGeometry(panelsAsSpace) != null)
            {
                xmlSpace.PlanarGeoemtry.PolyLoop = ToGBXML(BH.Engine.Environment.Query.FloorGeometry(panelsAsSpace), settings);
                xmlSpace.Area = BH.Engine.Environment.Query.FloorGeometry(panelsAsSpace).Area();
                xmlSpace.Volume = panelsAsSpace.Volume();
            }

            if (spaceLevel != null)
            {
                string levelName = "";
                if (spaceLevel.Name == "")
                    levelName = spaceLevel.Elevation.ToString();
                else
                    levelName = spaceLevel.Name;

                xmlSpace.BuildingStoreyIDRef = "Level-" + levelName.Replace(" ", "").ToLower();
            }

            return xmlSpace;
        }

        public static Space FromGBXML(this GBXML.Space space, GBXMLSettings settings = null)
        {
            Space bhomS = new oM.Environment.Elements.Space();
            bhomS.Name = space.Name;
            bhomS.Perimeter = space.PlanarGeoemtry.PolyLoop.FromGBXML();

            OriginContextFragment f = new OriginContextFragment();
            f.ElementID = space.ID;
            bhomS.Fragments.Add(f);
            return bhomS;
        }
    }
}



