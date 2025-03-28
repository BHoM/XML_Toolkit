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

using BH.oM.Adapter;
using BH.oM.Adapters.XML.Enums;
using BH.oM.Adapters.XML.Settings;

using System.ComponentModel;

namespace BH.oM.Adapters.XML
{
    [Description("Define configuration settings for pushing and pulling XML files using the XML Adapter.")]
    public class XMLConfig : ActionConfig, IXMLConfig
    {
        [Description("File settings for the file to push to or pull from.")]
        public virtual FileSettings File { get; set; } = null;

        [Description("Define the schema which the XML Adapter should be operating with.")]
        public virtual Schema Schema { get; set; } = Schema.Undefined;

        [Description("Set optional settings to use when pushing or pulling XML based on the chosen schema.")]
        public virtual IXMLSettings Settings { get; set; } = null;

        [Description("Determine whether 'nil' XML attributes should be removed when pushing to an XML file.")]
        public virtual bool RemoveNils { get; set; } = false;
    }
}


