using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Base;

namespace BH.oM.XML.GBXML
{
    public class RoundingSettings : BHoMObject
    {
        [Description("Define the number of decimal points to round the geometry coordinates to. Defaults is 4")]
        public virtual int GeometricPoints { get; set; } = 4;

        [Description("Define the number of decimal points to round the Building Location data to. Default is 5")]
        public virtual int BuildingLocation { get; set; } = 5;

        [Description("Define the number of decimal points to round the Layer Thickness to. Default is 4")]
        public virtual int LayerThickness { get; set; } = 4;

        [Description("Define the number of decimal points to round the Material Conductivity to. Default is 3")]
        public virtual int MaterialConductivity { get; set; } = 3;

        [Description("Define the number of decimal points to round the Material Density to. Default is 3")]
        public virtual int MaterialDensity { get; set; } = 3;

        [Description("Define the number of decimal points to round the Material Reflectance to. This is both Solar and Light Reflectance. Default is 3")]
        public virtual int MaterialReflectance { get; set; } = 3;

        [Description("Define the number of decimal points to round the Material Transmittance to. This is both Solar and Light Transmittance. Default is 3")]
        public virtual int MaterialTransmittance { get; set; } = 3;

        [Description("Define the number of decimal points to round the Material Emittance to. Default is 3")]
        public virtual int MaterialEmittance { get; set; } = 3;

        [Description("Define the number of decimal points to round the Width of geometry to. Default is 3")]
        public virtual int GeometryWidth { get; set; } = 3;

        [Description("Define the number of decimal points to round the Height of geometry to. Default is 3")]
        public virtual int GeometryHeight { get; set; } = 3;

        [Description("Define the number of decimal places to round the Azimuth of geometry to. Default is 3")]
        public virtual int GeometryAzimuth { get; set; } = 3;

        [Description("Define the number of decimal points to round the Tilt of geometry to. Default is 3")]
        public virtual int GeometryTilt { get; set; } = 3;
    }
}
