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

using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace BH.oM.XML.EnergyPlus
{
    public class EnergyPlusObject : IBHoMObject
    {
        [XmlIgnore]
        public virtual Guid BHoM_Guid { get; set; } = Guid.NewGuid();
        [XmlIgnore]
        public virtual Dictionary<string, object> CustomData { get; set; } = new Dictionary<string, object>();
        [XmlIgnore]
        public virtual string Name { get; set; } = "";
        [XmlIgnore]
        public virtual FragmentSet Fragments { get; set; } = new FragmentSet();
        [XmlIgnore]
        public virtual HashSet<string> Tags { get; set; } = new HashSet<string>();

    }
}





