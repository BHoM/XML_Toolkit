/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

using BH.oM.Environment.MaterialFragments;
using BHM = BH.oM.Physical.Materials;
using BHC = BH.oM.Physical.Constructions;
using BHP = BH.oM.Environment.Fragments;
using BHX = BH.Adapter.XML.GBXMLSchema;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;
using BH.Engine.Environment;

using System.ComponentModel;
using BH.oM.Base.Attributes;

using BH.Engine.Adapters.XML;
using BH.oM.Adapters.XML.Settings;

namespace BH.Adapter.XML
{
    public static partial class Convert
    {
        [Description("Get the GBXML representation of a BHoM Construction Layer")]
        [Input("layer", "The BHoM Construction Layer to convert into a GBXML Material")]
        [Output("material", "The GBXML representation of a BHoM Construction Layer")]
        public static BHX.Material ToGBXML(this BHC.Layer layer, GBXMLSettings settings)
        {
            BHX.Material gbMaterial = new BHX.Material();

            //double rValue = Math.Round(layer.RValue(), 3);
            //if (double.IsInfinity(rValue) || double.IsNaN(rValue)) rValue = -1; //Error

            gbMaterial.ID = "material-" + layer.Material.Name.CleanName();
            gbMaterial.Name = layer.Material.Name;
            //gbMaterial.RValue.Value = rValue.ToString();
            gbMaterial.Thickness.Value = Math.Round(layer.Thickness, settings.RoundingSettings.LayerThickness).ToString();

            IEnvironmentMaterial envMaterial = layer.Material.Properties.Where(x => x is IEnvironmentMaterial).FirstOrDefault() as IEnvironmentMaterial;

            if (envMaterial != null)
            {
                gbMaterial.Density.Value = Math.Round(envMaterial.Density, settings.RoundingSettings.MaterialDensity).ToString();
                gbMaterial.Conductivity.Value = Math.Round(envMaterial.Conductivity, settings.RoundingSettings.MaterialConductivity).ToString();
                gbMaterial.SpecificHeat.Value = envMaterial.SpecificHeat.ToString();
            }

            return gbMaterial;
        }

        [Description("Get the GBXML Layer for a collection of GBXML materials")]
        [Input("materials", "The GBXML Materials to get the layer for")]
        [Output("layer", "The GBXML layer for the given GBXML Materials")]
        public static BHX.Layer ToGBXML(this List<BHX.Material> materials)
        {
            BHX.Layer l = new BHX.Layer();

            l.ID = "layer";// + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

            List<BHX.MaterialID> materialIDs = new List<BHX.MaterialID>();
            foreach (BHX.Material m in materials)
            {
                l.ID += "-" + m.Name.CleanName();
                BHX.MaterialID id = new BHX.MaterialID();
                id.MaterialIDRef = m.ID;
                materialIDs.Add(id);
            }

            l.MaterialID = materialIDs.ToArray();

            return l;
        }

        [Description("Get the GBXML glazed representation of a BHoM Construction Layer")]
        [Input("layer", "The BHoM Construction Layer to convert into a GBXML Glaze Material")]
        [Output("glaze", "The GBXML glazed representation of a BHoM Construction Layer")]
        public static BHX.Glaze ToGBXGlazed(this BHC.Layer layer, GBXMLSettings settings)
        {
            if (layer == null || layer.Material == null) return null;
            SolidMaterial transparentProperties = layer.FindMaterial<SolidMaterial>(typeof(SolidMaterial));
            if (transparentProperties == null) return null;

            BHX.Glaze glaze = new BHX.Glaze();

            glaze.ID = "glaze-" + layer.Material.Name.Replace(" ", "-").Replace(",", "") + layer.Material.BHoM_Guid.ToString().Substring(0, 5);
            glaze.Name = layer.Material.Name;
            glaze.Thickness.Value = Math.Round(layer.Thickness, settings.RoundingSettings.LayerThickness).ToString();
            glaze.Conductivity.Value = transparentProperties.Conductivity.ToString();

            glaze.SolarTransmittance = new BHX.Transmittance { Value = Math.Round(transparentProperties.SolarTransmittance, settings.RoundingSettings.MaterialTransmittance).ToString(), Type = "Solar" };

            List<BHX.Reflectance> solarReflectance = new List<BHX.Reflectance>();
            solarReflectance.Add(new BHX.Reflectance { Value = Math.Round(transparentProperties.SolarReflectanceExternal, settings.RoundingSettings.MaterialReflectance).ToString(), Type = "ExtSolar" });
            solarReflectance.Add(new BHX.Reflectance { Value = Math.Round(transparentProperties.SolarReflectanceInternal, settings.RoundingSettings.MaterialReflectance).ToString(), Type = "IntSolar" });
            glaze.SolarReflectance = solarReflectance.ToArray();

            glaze.LightTransmittance = new BHX.Transmittance { Value = Math.Round(transparentProperties.LightTransmittance, settings.RoundingSettings.MaterialTransmittance).ToString(), Type = "Visible" };

            List<BHX.Reflectance> lightReflectance = new List<BHX.Reflectance>();
            lightReflectance.Add(new BHX.Reflectance { Value = Math.Round(transparentProperties.LightReflectanceExternal, settings.RoundingSettings.MaterialReflectance).ToString(), Type = "ExtVisible" });
            lightReflectance.Add(new BHX.Reflectance { Value = Math.Round(transparentProperties.LightReflectanceInternal, settings.RoundingSettings.MaterialReflectance).ToString(), Type = "IntVisible" });
            glaze.LightReflectance = lightReflectance.ToArray();

            List<BHX.Emittance> emittance = new List<BHX.Emittance>();
            emittance.Add(new BHX.Emittance { Value = Math.Round(transparentProperties.EmissivityExternal, settings.RoundingSettings.MaterialEmittance).ToString(), Type = "ExtIR" });
            emittance.Add(new BHX.Emittance { Value = Math.Round(transparentProperties.EmissivityInternal, settings.RoundingSettings.MaterialEmittance).ToString(), Type = "IntIR" });
            glaze.Emittance = emittance.ToArray();

            return glaze;
        }

        [Description("Get the GBXML gap (air gap, etc.) representation of a BHoM Construction Layer")]
        [Input("layer", "The BHoM Construction Layer to convert into a GBXML Gap Material")]
        [Output("gap", "The GBXML gap representation of a BHoM Construction Layer")]
        public static BHX.Gap ToGBXGap(this BHC.Layer layer, GBXMLSettings settings)
        {
            if (layer == null || layer.Material == null) return null;
            GasMaterial gasProperties = layer.FindMaterial<GasMaterial>(typeof(GasMaterial));
            if (gasProperties == null) return null;

            BHX.Gap gap = new BHX.Gap();

            gap.ID = "gap-" + layer.Material.Name.Replace(" ", "-").Replace(",", "") + layer.Material.BHoM_Guid.ToString().Substring(0, 5);
            gap.Name = layer.Material.Name;
            gap.Thickness.Value = Math.Round(layer.Thickness, settings.RoundingSettings.LayerThickness).ToString();
            gap.Conductivity.Value = Math.Round(gasProperties.Conductivity, settings.RoundingSettings.MaterialConductivity).ToString();

            switch(gasProperties.Gas)
            {
                case Gas.Air:
                    gap.Gas = "Air";
                    break;
                case Gas.Argon:
                    gap.Gas = "Argon";
                    break;
                case Gas.Krypton:
                    gap.Gas = "Krypton";
                    break;
                default:
                    gap.Gas = "Custom";
                    break;
            }

            return gap;
        }

        [Description("Get the BHoM representation of a GBXML Material")]
        [Input("gbMaterial", "The GBXML Material to convert into a BHoM Construction Layer")]
        [Output("layer", "The BHoM representation of a GBXML Material")]
        public static BHC.Layer FromGBXML(this BHX.Material gbMaterial)
        {
            BHC.Layer layer = new BHC.Layer();
            SolidMaterial materialProperties = new SolidMaterial();

            try
            {
                layer.Thickness = System.Convert.ToDouble(gbMaterial.Thickness.Value);
                materialProperties.Conductivity = System.Convert.ToDouble(gbMaterial.Conductivity.Value);
                materialProperties.SpecificHeat = System.Convert.ToDouble(gbMaterial.SpecificHeat.Value);
                materialProperties.Density = System.Convert.ToDouble(gbMaterial.Density.Value);
            }
            catch { }

            BHM.Material material = new BHM.Material();
            material.Name = gbMaterial.Name;

            material.Properties.Add(materialProperties);
            layer.Material = material;

            return layer;
        }
    }
}





