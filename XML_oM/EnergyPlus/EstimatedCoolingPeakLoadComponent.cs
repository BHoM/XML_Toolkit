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

using System.Xml.Serialization;
using BH.oM.Base;

namespace BH.oM.XML.EnergyPlus
{
    [Serializable]
    [XmlRoot(ElementName = "EnergyPlusTabularReports", IsNullable = false, Namespace = "")]
    public class EstimatedCoolingPeakLoadComponent : EnergyPlusObject
    {
        [XmlElement("name")]
        public override string Name { get; set; } = "";

        [XmlElement("SensibleInstant")]
        public virtual SensibleInstant SensibleInstant { get; set; } = new SensibleInstant();

        [XmlElement("SensibleDelayed")]
        public virtual SensibleDelayed SensibleDelayed { get; set; } = new SensibleDelayed();

        [XmlElement("SensibleReturnAir")]
        public virtual SensibleReturnAir SensibleReturnAir { get; set; } = new SensibleReturnAir();

        [XmlElement("Latent")]
        public virtual Latent Latent { get; set; } = new Latent();

        [XmlElement("Total")]
        public virtual Total Total { get; set; } = new Total();

        [XmlElement("GrandTotal")]
        public virtual double GrandTotal { get; set; } = 0.0;
    }
}





