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

using BHE = BH.oM.Environment.Elements;
using BHM = BH.oM.Environment.Materials;
using BHP = BH.oM.Environment.Properties;
using BHX = BH.oM.XML;
using BHG = BH.oM.Geometry;

using BH.Engine.Geometry;
using BH.Engine.Environment;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        public static BHX.Material ToGBXML(this BHM.Material material)
        {
            BHX.Material gbMaterial = new BHX.Material();

            double rValue = Math.Round(material.Thickness / material.MaterialProperties.Conductivity, 3);
            if (double.IsInfinity(rValue) || double.IsNaN(rValue)) rValue = -1; //Error

            gbMaterial.ID = "material-" + material.BHoM_Guid.ToString().Replace("-", "").Substring(0, 5);
            gbMaterial.Name = material.Name;
            gbMaterial.RValue.Value = rValue.ToString();
            gbMaterial.Thickness = Math.Round(material.Thickness, 3);
            gbMaterial.Conductivity.Value = Math.Round(material.MaterialProperties.Conductivity, 3).ToString();
            gbMaterial.Density.Value = Math.Round(material.MaterialProperties.Density, 3).ToString();
            gbMaterial.SpecificHeat.Value = material.MaterialProperties.SpecificHeat.ToString();

            return gbMaterial;
        }

        public static BHX.Layer ToGBXML(this List<BHX.Material> materials)
        {
            BHX.Layer l = new BHX.Layer();

            l.ID = "layer-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);

            List<BHX.MaterialId> materialIDs = new List<BHX.MaterialId>();
            foreach (BHX.Material m in materials)
            {
                BHX.MaterialId id = new BHX.MaterialId();
                id.MaterialIDRef = m.ID;
                materialIDs.Add(id);
            }

            l.MaterialID = materialIDs.ToArray();

            return l;
        }

        public static BHM.Material ToBHoM(this BHX.Material gbMaterial)
        {
            BHM.Material material = new BHM.Material();

            material.Name = gbMaterial.Name;
            material.Thickness = System.Convert.ToDouble(gbMaterial.Thickness);
            material.MaterialProperties = new BHP.MaterialPropertiesOpaque();
            material.MaterialProperties.Conductivity = System.Convert.ToDouble(gbMaterial.Conductivity.Value);
            material.MaterialProperties.Density = System.Convert.ToDouble(gbMaterial.Density.Value);
            material.MaterialProperties.SpecificHeat = System.Convert.ToDouble(gbMaterial.SpecificHeat.Value);

            return material;
        }
    }
}