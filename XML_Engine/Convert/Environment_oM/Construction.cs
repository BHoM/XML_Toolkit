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

using BHI = BH.oM.Environment.Interface;
using BHE = BH.oM.Environment.Elements;
using BHP = BH.oM.Environment.Properties;
using BHX = BH.oM.XML;
using BHG = BH.oM.Geometry;
using BHM = BH.oM.Environment.Materials;

using BH.Engine.Geometry;
using BH.Engine.Environment;

namespace BH.Engine.XML
{
    public static partial class Convert
    {
        public static BHX.Construction ToGBXMLConstruction(this BHE.BuildingElement element)
        {
            if (element == null || element.ElementProperties() == null) return null;
            return element.ElementProperties().ToGBXMLConstruction();
        }

        public static BHX.WindowType ToGBXMLWindow(this BHE.Opening opening)
        {
            if (opening == null || opening.ElementProperties() == null) return null;
            return opening.ElementProperties().ToGBXMLWindow(opening);
        }

        public static BHX.Construction ToGBXMLConstruction(this BHI.IBHoMExtendedProperties properties)
        {
            if (properties == null) return null;
            BHP.ElementProperties props = properties as BHP.ElementProperties;
            if (props == null || props.Construction == null) return null;
            return props.Construction.ToGBXML();
        }

        public static BHX.WindowType ToGBXMLWindow(this BHI.IBHoMExtendedProperties properties, BHE.Opening opening)
        {
            if (properties == null) return null;
            BHP.ElementProperties props = properties as BHP.ElementProperties;
            if (props == null || props.Construction == null) return null;
            return props.Construction.ToGBXMLWindow(opening);
        }

        public static BHX.Construction ToGBXML(this BHE.Construction construction, BHE.BuildingElement element = null)
        {
            BHX.Construction gbConstruction = new BHX.Construction();

            BHP.EnvironmentContextProperties contextProperties = null;
            BHP.BuildingElementAnalyticalProperties analysisProperties = null;
            if (element != null)
            {
                contextProperties = element.EnvironmentContextProperties() as BHP.EnvironmentContextProperties;
                analysisProperties = element.AnalyticalProperties() as BHP.BuildingElementAnalyticalProperties;
            }

            gbConstruction.ID = (contextProperties == null ? construction.ConstructionID() : contextProperties.TypeName.GetCleanName().Replace(" ", "-"));
            gbConstruction.Absorptance = construction.ToGBXMLAbsorptance();
            gbConstruction.Name = construction.Name;
            gbConstruction.Roughness = construction.Roughness.ToGBXML();
            gbConstruction.UValue.Value = (analysisProperties == null ? (element != null ? element.UValue() : 0) : analysisProperties.UValue).ToString();

            return gbConstruction;
        }

        public static BHX.WindowType ToGBXMLWindow(this BHE.Construction construction, BHE.Opening opening)
        {
            BHX.WindowType window = new BHX.WindowType();

            BHP.BuildingElementAnalyticalProperties extraProperties = opening.PropertiesByType(typeof(BHP.BuildingElementAnalyticalProperties)) as BHP.BuildingElementAnalyticalProperties;
            BHP.EnvironmentContextProperties contextProperties = opening.EnvironmentContextProperties() as BHP.EnvironmentContextProperties;

            window.ID = (contextProperties == null ? "window-" + construction.Name.GetCleanName().Replace(" ", "-") : contextProperties.TypeName.GetCleanName().Replace(" ", "-"));
            window.Name = (contextProperties == null ? construction.Name : contextProperties.TypeName);
            window.UValue.Value = (extraProperties == null ? "0" : extraProperties.UValue.ToString());
            window.Transmittance.Value = (extraProperties == null ? "0" : extraProperties.LTValue.ToString());
            window.SolarHeatGainCoefficient.Value = (extraProperties == null ? "0" : extraProperties.GValue.ToString());
            if (construction.Materials.Count > 0)
                window.InternalGlaze = (construction.Materials[0] as BHM.Material).ToGBXGlazed();
            if (construction.Materials.Count > 1)
                window.Gap = (construction.Materials[1] as BHM.Material).ToGBXGap();
            if (construction.Materials.Count > 2)
                window.ExternalGlaze = (construction.Materials[2] as BHM.Material).ToGBXGlazed();

            return window;
        }

        public static BHX.Absorptance ToGBXMLAbsorptance(this BHE.Construction construction)
        {
            BHX.Absorptance absorptance = new BHX.Absorptance();
            absorptance.Unit = construction.AbsorptanceUnit.ToGBXML();
            absorptance.Type = construction.AbsorptanceType.ToGBXML();
            absorptance.Value = construction.AbsorptanceValue.ToString();
            return absorptance;
        }

        public static BHE.Construction ToBHoM(this BHX.Construction gbConstruction)
        {
            BHE.Construction construction = new BHE.Construction();

            construction.AbsorptanceType = gbConstruction.Absorptance.Type.ToBHoMAbsorptanceType();
            construction.AbsorptanceUnit = gbConstruction.Absorptance.Unit.ToBHoMAbsorptanceUnit();
            construction.AbsorptanceValue = System.Convert.ToDouble(gbConstruction.Absorptance.Value);

            construction.Roughness = gbConstruction.Roughness.ToBHoM();
            construction.Name = gbConstruction.Name;

            return construction;
        }
    }
}
