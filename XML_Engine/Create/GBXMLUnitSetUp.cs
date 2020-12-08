using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.Adapters.XML;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.Adapters.XML
{
    public static partial class Create
    {
        [Description("Creates an instance of a GBXMLUnitSetUp object based on the LengthUnit type. If the LengthUnit type is Meters, the Area and Volume Units will become SquareMeters and CubicMeters respectively. Allows for the creation of GBXMLUnitSetUp from just the length and temperature attributes.")]
        [Input("length", "The length unit to build the area and volume units from.")]
        [Input("temperature", "The temperature unit to use, defaults to Celcius")]
        [Output("gbxmlUnitSetUp", "A GBXMLUnitSetUp object containing set up options for the gbXML export.")]
        public static GBXMLUnitSetUp GBXMLUnitSetUp(LengthUnit length, TemperatureUnit temperature = TemperatureUnit.Celcius)
        {
            string type  = length.ToString();

            return new GBXMLUnitSetUp
            {
                LengthUnit = length,
                AreaUnit = (AreaUnit)Enum.Parse(typeof(AreaUnit), "Square" + type),
                TemperatureUnit = temperature,
                VolumeUnit = (VolumeUnit)Enum.Parse(typeof(VolumeUnit), "Cubic" + type),
            };
        }
    }
}
