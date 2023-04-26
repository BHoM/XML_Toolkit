/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using System.ComponentModel;
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

        [Description("By default this is an XML String representation. Most software will automatically handle the conversion to a numerical representation but users can also use the Query.Depth(Markup) method if necessary.")]
        [XmlElement("Depth")]
        public virtual string Depth { get; set; }
        
        [XmlElement("Layer")]
        public virtual string Layer { get; set; }

        [Description("This is a custom property which can be added to Bluebeam for SAP workflow interoperability.")]
        [XmlElement("TFA-FloorType")]
        public virtual string TFAFloorType { get; set; }

        [Description("This is a custom property which can be added to Bluebeam for SAP workflow interoperability.")]
        [XmlElement("TFA-Storey")]
        public virtual int TFAStorey { get; set; }

        [Description("This is a custom property which can be added to Bluebeam for SAP workflow interoperability.")]
        [XmlElement("TFA-Height")]
        public virtual double TFAHeight { get; set; }

        [Description("This is a custom property which can be added to Bluebeam for SAP workflow interoperability.")]
        [XmlElement("Opening-Type")]
        public virtual string OpeningType { get; set; }

        [Description("This is a custom property which can be added to Bluebeam for SAP workflow interoperability.")]
        [XmlElement("Opening-Location")]
        public virtual string OpeningLocation { get; set; }

        [Description("This is a custom property which can be added to Bluebeam for SAP workflow interoperability.")]
        [XmlElement("Opening-Height")]
        public virtual double OpeningHeight { get; set; }

        [Description("This is a custom property which can be added to Bluebeam for SAP workflow interoperability.")]
        [XmlElement("Opening-Orientation")]
        public virtual string OpeningOrientation { get; set; }

        [Description("This is a custom property which can be added to Bluebeam for SAP workflow interoperability.")]
        [XmlElement("Opening-Pitch")]
        public virtual string OpeningPitch { get; set; }

        [Description("This is a custom property which can be added to Bluebeam for SAP workflow interoperability.")]
        [XmlElement("Wall-Type")]
        public virtual string WallType { get; set; }

        [Description("This is a custom property which can be added to Bluebeam for SAP workflow interoperability.")]
        [XmlElement("Wall-Height")]
        public virtual double WallHeight { get; set; }
    }
}


