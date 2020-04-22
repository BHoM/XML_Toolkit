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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHE = BH.oM.Environment.Elements;
using BHP = BH.oM.Environment.Fragments;
using BHX = BH.oM.XML;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;
using BH.Engine.Environment;

using BHC = BH.oM.Physical.Constructions;
using BHEM = BH.oM.Environment.MaterialFragments;
using BHM = BH.oM.Physical.Materials;

using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Adapter.XML
{
    public static partial class Convert
    {
        [Description("Get the GBXML representation of a BHoM Construction owned by an Environments Panel")]
        [Input("element", "The BHoM Environment Panel to get the GBXML construction from")]
        [Output("construction", "The GBXML Construction")]
        public static BHX.Construction ToGBXMLConstruction(this BHE.Panel element)
        {
            if (element.Construction == null) return null;
            return element.Construction.ToGBXML(element);
        }

        [Description("Get the GBXML representation of a BHoM Construction")]
        [Input("construction", "The BHoM Construction to convert to a GBXML Construction")]
        [Input("panel", "The BHoM Environment Panel which hosts the construction")]
        [Output("construction", "The GBXML Construction")]
        public static BHX.Construction ToGBXML(this BHC.IConstruction construction, BHE.Panel panel = null)
        {
            if (construction == null) return null;
            return ToGBXML(construction as dynamic, panel);
        }

        [Description("Get the GBXML representation of a BHoM Construction")]
        [Input("construction", "The BHoM Construction to convert to a GBXML Construction")]
        [Input("opening", "The BHoM Environment Opening which hosts the construction")]
        [Output("windowType", "The GBXML Window Construction")]
        public static BHX.WindowType ToGBXMLWindow(this BHC.IConstruction construction, BHE.Opening opening = null)
        {
            if (construction == null) return null;
            return ToGBXMLWindow(construction as dynamic, opening);
        }

        [Description("Get the GBXML representation of a BHoM Construction")]
        [Input("construction", "The BHoM Construction to convert to a GBXML Construction")]
        [Input("element", "The BHoM Environment Panel which hosts the construction")]
        [Output("construction", "The GBXML Construction")]
        public static BHX.Construction ToGBXML(this BHC.Construction construction, BHE.Panel element = null)
        {
            if (construction == null) return null;
            BHX.Construction gbConstruction = new BHX.Construction();

            BHP.OriginContextFragment contextProperties = null;
            BHP.PanelAnalyticalFragment analysisProperties = null;
            if (element != null)
            {
                contextProperties = element.FindFragment<BHP.OriginContextFragment>(typeof(BHP.OriginContextFragment));
                analysisProperties = element.FindFragment<BHP.PanelAnalyticalFragment>(typeof(BHP.PanelAnalyticalFragment));
            }

            gbConstruction.ID = (contextProperties == null ? construction.ConstructionID() : contextProperties.TypeName.CleanName().Replace(" ", "-"));
            gbConstruction.Absorptance.Value = construction.Absorptance().ToString();
            gbConstruction.Name = (contextProperties == null ? construction.Name : contextProperties.TypeName);
            gbConstruction.Roughness = construction.Roughness().ToGBXML();
            gbConstruction.UValue.Value = (analysisProperties == null || analysisProperties.UValue == 0 ? (construction.UValue() == double.NaN || double.IsInfinity(construction.UValue()) ? 10 : construction.UValue()) : analysisProperties.UValue).ToString();

            if (gbConstruction.UValue.Value == "10")
                BH.Engine.Reflection.Compute.RecordWarning(string.Format("U-Value has been calculated to Infinity or NaN. Has been set to default 10"));

            return gbConstruction;
        }

        [Description("Get the GBXML representation of a BHoM Construction")]
        [Input("construction", "The BHoM Construction to convert to a GBXML Construction")]
        [Input("opening", "The BHoM Environment Opening which hosts the construction")]
        [Output("windowType", "The GBXML Window Construction")]
        public static BHX.WindowType ToGBXMLWindow(this BHC.Construction construction, BHE.Opening opening)
        {
            BHX.WindowType window = new BHX.WindowType();

            BHP.PanelAnalyticalFragment extraProperties = opening.FindFragment<BHP.PanelAnalyticalFragment>(typeof(BHP.PanelAnalyticalFragment));
            BHP.OriginContextFragment contextProperties = opening.FindFragment<BHP.OriginContextFragment>(typeof(BHP.OriginContextFragment));

            window.ID = "window-" + (contextProperties == null ? construction.Name.CleanName() : contextProperties.TypeName.CleanName());
            window.Name = (contextProperties == null ? construction.Name : contextProperties.TypeName);
            window.UValue.Value = (extraProperties == null || extraProperties.UValue == 0 ? construction.UValue().ToString() : extraProperties.UValue.ToString());
            window.Transmittance.Value = (extraProperties == null ? "0" : extraProperties.LTValue.ToString());
            window.SolarHeatGainCoefficient.Value = (extraProperties == null ? "0" : extraProperties.GValue.ToString());
            if (construction.Layers.Count > 0)
                window.InternalGlaze = (construction.Layers[0]).ToGBXGlazed();
            if (construction.Layers.Count > 1)
                window.Gap = (construction.Layers[1]).ToGBXGap();
            if (construction.Layers.Count > 2)
                window.ExternalGlaze = (construction.Layers[2]).ToGBXGlazed();

            return window;
        }

        [Description("Get the BHoM representation of a GBXML construction")]
        [Input("gbConstruction", "The GBXML Construction to convert to a BHoM Construction")]
        [Input("layers", "The BHoM layers which will be appended to the construction")]
        [Output("construction", "The BHoM Construction")]
        public static BHC.Construction FromGBXML(this BHX.Construction gbConstruction, List<BHC.Layer> layers)
        {
            BHC.Construction construction = new BHC.Construction();
            construction.Name = gbConstruction.Name;
            construction.Layers = layers;

            return construction;
        }

        [Description("Get the BHoM representation of a GBXML Layer")]
        [Input("gbLayer", "The GBXML Layer to convert to a BHoM Layer")]
        [Input("material", "The BHoM material which will be appended to the layer")]
        [Input("thickness", "The thickness of the material on the layer")]
        [Output("layer", "The BHoM Layer")]
        public static BHC.Layer FromGBXML(this BHX.Layer gbLayer, BHM.Material material, double thickness)
        {
            BHC.Layer layer = new BHC.Layer();
            layer.Name = gbLayer.Name;
            layer.Thickness = thickness;
            layer.Material = material;

            return layer;
        }
    }
}

