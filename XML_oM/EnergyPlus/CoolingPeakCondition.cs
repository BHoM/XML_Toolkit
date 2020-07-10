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

using System.Xml.Serialization;
using BH.oM.Base;

namespace BH.oM.XML.EnergyPlus
{
    [Serializable]
    [XmlRoot(ElementName = "EnergyPlusTabularReports", IsNullable = false, Namespace = "")]
    public class CoolingPeakCondition : EnergyPlusObject
    {
        [XmlAttribute("name")]
        public override string Name { get; set; } = "";

        [XmlAttribute("TimeOfPeakLoad")]
        public string TimeOfPeakLoad { get; set; } = "";

        [XmlAttribute("OutsideDryBulbTemperature")]
        public OutsideDryBulbTemperature OutsideDryBulbTemperature { get; set; } = new OutsideDryBulbTemperature();

        [XmlAttribute("OutsideWetBulbTemperature")]
        public OutsideWetBulbTemperature OutsideWetBulbTemperature { get; set; } = new OutsideWetBulbTemperature();

        [XmlAttribute("OutsideHumidityRatioAtPeak")]
        public OutsideHumidityRatioAtPeak OutsideHumidityRatioAtPeak { get; set; } = new OutsideHumidityRatioAtPeak();

        [XmlAttribute("ZoneDryBulbTemperature")]
        public ZoneDryBulbTemperature ZoneDryBulbTemperature { get; set; } = new ZoneDryBulbTemperature();

        [XmlAttribute("ZoneRelativeHumdity")]
        public ZoneRelativeHumdity ZoneRelativeHumdity { get; set; } = new ZoneRelativeHumdity();

        [XmlAttribute("ZoneHumidityRatioAtPeak")]
        public ZoneHumidityRatioAtPeak ZoneHumidityRatioAtPeak { get; set; } = new ZoneHumidityRatioAtPeak();

        [XmlAttribute("PeakDesignSensibleLoad")]
        public PeakDesignSensibleLoad PeakDesignSensibleLoad { get; set; } = new PeakDesignSensibleLoad();

        [XmlAttribute("EstimatedInstantDelayedSensibleLoad")]
        public EstimatedInstantDelayedSensibleLoad EstimatedInstantDelayedSensibleLoad { get; set; } = new EstimatedInstantDelayedSensibleLoad();

        [XmlAttribute("Difference")]
        public Difference Difference { get; set; } = new Difference();
    }
}
