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

using System.Xml.Serialization;

namespace BH.oM.XML.Bluebeam
{
    [Serializable]
    [XmlRoot(ElementName = "Markup", IsNullable = false)]
    public class Markup : BluebeamObject
    {
        [XmlElement("Page_Label")]
        public virtual int PageLabel { get; set; }

        [XmlElement("Subject")]
        public virtual string Subject { get; set; }

        [XmlElement("Space")]
        public virtual string Space { get; set; }

        [XmlElement("Author")]
        public virtual string Author { get; set; }

        [XmlElement("Date")]
        public virtual string Date { get; set; }

        [XmlElement("Colour")]
        public virtual string Colour { get; set; }

        [XmlElement("Comments")]
        public virtual string Comments { get; set; }

        [XmlElement("Length")]
        public virtual double Length { get; set; }

        [XmlElement("Area")]
        public virtual double Area { get; set; }

        [XmlElement("Label")]
        public virtual string Label { get; set; }

        [XmlElement("Depth")]
        public virtual string XMLDepth { get; set; }
        
        [XmlElement("Layer")]
        public virtual string Layer { get; set; }
    }
}

