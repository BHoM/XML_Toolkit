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

namespace BH.oM.XML.Bluebeam
{
    [Serializable]
    [XmlRoot(ElementName = "Markup", IsNullable = false)]
    public class Markup : BluebeamObject
    {
        [XmlAttribute("Page_Label")]
        public virtual int PageLabel { get; set; }

        [XmlAttribute("Subject")]
        public virtual string Subject { get; set; }

        [XmlAttribute("Space")]
        public virtual string Space { get; set; }

        [XmlAttribute("Author")]
        public virtual string Author { get; set; }

        [XmlAttribute("Date")]
        public virtual DateTime Date { get; set; }

        [XmlAttribute("Colour")]
        public virtual string Colour { get; set; }

        [XmlAttribute("Comments")]
        public virtual string Comments { get; set; }

        [XmlAttribute("Length")]
        public virtual double Length { get; set; }

        [XmlAttribute("Area")]
        public virtual double Area { get; set; }

        [XmlAttribute("Label")]
        public virtual string Label { get; set; }

        [XmlAttribute("Depth")]
        public virtual double Depth { get; set; }

        [XmlAttribute("Layer")]
        public virtual string Layer { get; set; }
    }
}
