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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Environment.MaterialFragments;
using BHM = BH.oM.Physical.Materials;
using BHC = BH.oM.Physical.Constructions;
using BHP = BH.oM.Environment.Fragments;
using BHX = BH.oM.XML;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;
using BH.Engine.Environment;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        public static BHX.Material ToGBXML(this BHC.Layer layer)
        {
            BHX.Material gbMaterial = new BHX.Material();

            double rValue = Math.Round(layer.RValue(), 3);
            if (double.IsInfinity(rValue) || double.IsNaN(rValue)) rValue = -1; //Error

            gbMaterial.ID = "material-" + layer.Material.Name.CleanName();
            gbMaterial.Name = layer.Material.Name;
            gbMaterial.RValue.Value = rValue.ToString();
            gbMaterial.Thickness.Value = Math.Round(layer.Thickness, 3).ToString();
            gbMaterial.Density.Value = Math.Round(layer.Material.Density, 3).ToString();

            IEnvironmentMaterial envMaterial = layer.Material.Properties.Where(x => x is IEnvironmentMaterial).FirstOrDefault() as IEnvironmentMaterial;

            if (envMaterial != null)
            {
                gbMaterial.Conductivity.Value = Math.Round(envMaterial.Conductivity, 3).ToString();
                gbMaterial.SpecificHeat.Value = envMaterial.SpecificHeat.ToString();
            }

            return gbMaterial;
        }

        public static BHX.Layer ToGBXML(this List<BHX.Material> materials)
        {
            BHX.Layer l = new BHX.Layer();

            l.ID = "layer";// + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

            List<BHX.MaterialId> materialIDs = new List<BHX.MaterialId>();
            foreach (BHX.Material m in materials)
            {
                l.ID += "-" + m.Name.CleanName();
                BHX.MaterialId id = new BHX.MaterialId();
                id.MaterialIDRef = m.ID;
                materialIDs.Add(id);
            }

            l.MaterialID = materialIDs.ToArray();

            return l;
        }

        public static BHC.Layer ToBHoM(this BHX.Material gbMaterial)
        {
            BHC.Layer layer = new BHC.Layer();
            layer.Thickness = System.Convert.ToDouble(gbMaterial.Thickness);

            SolidMaterial materialProperties = new SolidMaterial();
            materialProperties.Conductivity = System.Convert.ToDouble(gbMaterial.Conductivity.Value);
            materialProperties.SpecificHeat = System.Convert.ToDouble(gbMaterial.SpecificHeat.Value);

            BHM.Material material = new BHM.Material();
            material.Name = gbMaterial.Name;
            material.Density = System.Convert.ToDouble(gbMaterial.Density.Value);

            material.Properties.Add(materialProperties);
            layer.Material = material;

            return layer;
        }

        public static BHX.Glaze ToGBXGlazed(this BHC.Layer layer)
        {
            if (layer == null || layer.Material == null) return null;
            SolidMaterial transparentProperties = layer.FindMaterial<SolidMaterial>(typeof(SolidMaterial));
            if (transparentProperties == null) return null;

            BHX.Glaze glaze = new BHX.Glaze();

            glaze.ID = "glaze-" + layer.Material.Name.Replace(" ", "-").Replace(",", "") + layer.Material.BHoM_Guid.ToString().Substring(0, 5);
            glaze.Name = layer.Material.Name;
            glaze.Thickness.Value = Math.Round(layer.Thickness, 4).ToString();
            glaze.Conductivity.Value = transparentProperties.Conductivity.ToString();

            glaze.SolarTransmittance = new BHX.Transmittance { Value = Math.Round(transparentProperties.SolarTransmittance, 3).ToString(), Type = "Solar" };

            List<BHX.Reflectance> solarReflectance = new List<BHX.Reflectance>();
            solarReflectance.Add(new BHX.Reflectance { Value = Math.Round(transparentProperties.SolarReflectanceExternal, 3).ToString(), Type = "ExtSolar" });
            solarReflectance.Add(new BHX.Reflectance { Value = Math.Round(transparentProperties.SolarReflectanceInternal, 3).ToString(), Type = "IntSolar" });
            glaze.SolarReflectance = solarReflectance.ToArray();

            glaze.LightTransmittance = new BHX.Transmittance { Value = Math.Round(transparentProperties.LightTransmittance, 3).ToString(), Type = "Visible" };

            List<BHX.Reflectance> lightReflectance = new List<BHX.Reflectance>();
            lightReflectance.Add(new BHX.Reflectance { Value = Math.Round(transparentProperties.LightReflectanceExternal, 3).ToString(), Type = "ExtVisible" });
            lightReflectance.Add(new BHX.Reflectance { Value = Math.Round(transparentProperties.LightReflectanceInternal, 3).ToString(), Type = "IntVisible" });
            glaze.LightReflectance = lightReflectance.ToArray();

            List<BHX.Emittance> emittance = new List<BHX.Emittance>();
            emittance.Add(new BHX.Emittance { Value = Math.Round(transparentProperties.EmissivityExternal, 3).ToString(), Type = "ExtIR" });
            emittance.Add(new BHX.Emittance { Value = Math.Round(transparentProperties.EmissivityInternal, 3).ToString(), Type = "IntIR" });
            glaze.Emittance = emittance.ToArray();

            return glaze;
        }

        public static BHX.Gap ToGBXGap(this BHC.Layer layer)
        {
            if (layer == null || layer.Material == null) return null;
            GasMaterial gasProperties = layer.FindMaterial<GasMaterial>(typeof(GasMaterial));
            if (gasProperties == null) return null;

            BHX.Gap gap = new BHX.Gap();

            gap.ID = "gap-" + layer.Material.Name.Replace(" ", "-").Replace(",", "") + layer.Material.BHoM_Guid.ToString().Substring(0, 5);
            gap.Name = layer.Material.Name;
            gap.Thickness.Value = Math.Round(layer.Thickness, 4).ToString();
            gap.Conductivity.Value = Math.Round(gasProperties.Conductivity, 3).ToString();

            return gap;
        }
    }
}