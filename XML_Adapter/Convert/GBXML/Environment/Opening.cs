/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

using BH.oM.Base;
using BHE = BH.oM.Environment.Elements;
using BHP = BH.oM.Environment.Fragments;
using BHX = BH.Adapter.XML.GBXMLSchema;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;
using BH.Engine.Environment;
using BH.oM.Adapters.XML.Settings;

using System.ComponentModel;
using BH.oM.Base.Attributes;

using BH.Engine.Adapters.XML;

namespace BH.Adapter.XML
{
    public static partial class Convert
    {
        [Description("Get the GBXML representation of a BHoM Environments Opening")]
        [Input("opening", "The BHoM Environments Opening to convert into a GBXML Opening")]
        [Output("openingGBXML", "The GBXML representation of a BHoM Environment Opening")]
        public static BHX.Opening ToGBXML(this BHE.Opening opening, BHE.Panel hostPanel, GBXMLSettings settings)
        {
            BHX.Opening gbOpening = new BHX.Opening();

            BHG.Polyline pLine = opening.Polyline();

            gbOpening.Name = opening.Name;
            gbOpening.ID = "opening" + opening.BHoM_Guid.ToString().Replace("-", "").Substring(0, 5);
            gbOpening.PlanarGeometry.PolyLoop = pLine.ToGBXML(settings);
            gbOpening.PlanarGeometry.ID = "openingPGeom-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);
            gbOpening.RectangularGeometry.CartesianPoint = BH.Engine.Geometry.Query.Centroid(pLine).ToGBXML(settings);
            gbOpening.RectangularGeometry.Height = Math.Round(opening.Height(), settings.RoundingSettings.GeometryHeight);
            //TODO: temporary solution to get valid file to be replaced with correct height
            if (opening.Height() == 0)
               gbOpening.RectangularGeometry.Height = 0.1;
            gbOpening.RectangularGeometry.Width = Math.Round(opening.Width(), settings.RoundingSettings.GeometryWidth);
            gbOpening.RectangularGeometry.ID = "rGeomOpening-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);

            pLine = pLine.CleanPolyline(minimumSegmentLength: settings.DistanceTolerance);
            double openingArea = pLine.Area();
            double panelArea = hostPanel.Polyline().Area();

            if (openingArea >= panelArea)
            {
                pLine = BH.Engine.Geometry.Modify.Offset(pLine, settings.OffsetDistance);

                if (pLine == null)
                    pLine = opening.Polyline(); //Reset the polyline if something went wrong with the offset
                
                gbOpening.PlanarGeometry.PolyLoop = pLine.ToGBXML(settings);
            }

            //Normals away from space
            //if (!BH.Engine.Environment.Query.NormalAwayFromSpace(pLine, hostSpace, settings.PlanarTolerance))
                //gbOpening.PlanarGeometry.PolyLoop = pLine.Flip().ToGBXML();

            gbOpening.CADObjectID = opening.CADObjectID();
            gbOpening.OpeningType = opening.Type.ToGBXML();

            BHP.OriginContextFragment contextProperties = opening.FindFragment<BHP.OriginContextFragment>(typeof(BHP.OriginContextFragment));
            string elementID = "";
            string familyName = "";
            if (contextProperties != null)
            {
                elementID = contextProperties.ElementID;
                familyName = contextProperties.TypeName;
            }

            if (gbOpening.OpeningType.ToLower() == "fixedwindow" && contextProperties != null && contextProperties.TypeName.ToLower().Contains("skylight"))
                gbOpening.OpeningType = "FixedSkylight";

            if (familyName == "System Panel") //No SAM_BuildingElementType for this one atm
                gbOpening.OpeningType = "FixedWindow";

            if (settings.ReplaceSolidOpeningsIntoDoors && gbOpening.OpeningType.Contains("Window") && (opening.OpeningConstruction != null && opening.OpeningConstruction.Name.Contains("SLD"))) //Change windows with SLD construction into doors for IES
                gbOpening.OpeningType = "NonSlidingDoor";

            if (settings.IncludeConstructions)
                gbOpening.WindowTypeIDRef = "window-" + (contextProperties != null ? contextProperties.TypeName.CleanName() : (opening.OpeningConstruction != null ? opening.OpeningConstruction.Name.CleanName() : ""));
            else
                gbOpening.WindowTypeIDRef = null;

            gbOpening.ConstructionIDRef = null; //ToDo review having this property on an opening?

            return gbOpening;
        }

        [Description("Get the BHoM Environments representation of a GBXML Opening")]
        [Input("gbOpening", "The GBXML Opening to convert into a BHoM Environments Opening")]
        [Output("openingBHoM", "The BHoM representation of a GBXML Opening")]
        public static BHE.Opening FromGBXML(this BHX.Opening gbOpening)
        {
            BHE.Opening opening = new BHE.Opening();

            BHG.Polyline pLine = gbOpening.PlanarGeometry.PolyLoop.FromGBXML();
            opening.Edges = pLine.ToEdges();

            string[] cadSplit = gbOpening.CADObjectID.Split('[');
            if (cadSplit.Length > 0)
                opening.Name = cadSplit[0].Trim();
            if (cadSplit.Length > 1)
            {
                BHP.OriginContextFragment envContext = new BHP.OriginContextFragment();
                envContext.ElementID = cadSplit[1].Split(']')[0].Trim();
                envContext.TypeName = opening.Name;

                if (opening.Fragments == null) opening.Fragments = new FragmentSet();
                opening.Fragments.Add(envContext);
            }

            opening.Type = gbOpening.OpeningType.FromGBXMLOpeningType();

            return opening;
        }
    }
}


