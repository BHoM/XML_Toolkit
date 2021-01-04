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
        public virtual string TimeOfPeakLoad { get; set; } = "";

        [XmlElement("OutsideDryBulbTemperature")]
        public virtual OutsideDryBulbTemperature OutsideDryBulbTemperature { get; set; } = new OutsideDryBulbTemperature();

        [XmlElement("OutsideWetBulbTemperature")]
        public virtual OutsideWetBulbTemperature OutsideWetBulbTemperature { get; set; } = new OutsideWetBulbTemperature();

        [XmlElement("OutsideHumidityRatioAtPeak")]
        public virtual OutsideHumidityRatioAtPeak OutsideHumidityRatioAtPeak { get; set; } = new OutsideHumidityRatioAtPeak();

        [XmlElement("ZoneDryBulbTemperature")]
        public virtual ZoneDryBulbTemperature ZoneDryBulbTemperature { get; set; } = new ZoneDryBulbTemperature();

        [XmlElement("ZoneRelativeHumdity")]
        public virtual ZoneRelativeHumidity ZoneRelativeHumdity { get; set; } = new ZoneRelativeHumidity();

        [XmlElement("ZoneHumidityRatioAtPeak")]
        public virtual ZoneHumidityRatioAtPeak ZoneHumidityRatioAtPeak { get; set; } = new ZoneHumidityRatioAtPeak();

        [XmlElement("PeakDesignSensibleLoad")]
        public virtual PeakDesignSensibleLoad PeakDesignSensibleLoad { get; set; } = new PeakDesignSensibleLoad();

        [XmlElement("EstimatedInstantDelayedSensibleLoad")]
        public virtual EstimatedInstantDelayedSensibleLoad EstimatedInstantDelayedSensibleLoad { get; set; } = new EstimatedInstantDelayedSensibleLoad();

        [XmlElement("Difference")]
        public virtual Difference Difference { get; set; } = new Difference();
    }
}

