using System;
using System.Collections.Generic;

using BH.oM.Environment.Elements;


namespace XML_Engine
{
    /// <summary>
    /// BHoM XML Engine Query Methods
    /// </summary>
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        /// <summary>
        /// Gets Exposed To Sun from surface type
        /// </summary>
        /// <param name="surfaceType">XML SurfaceType</param>
        /// <returns name="ExposedToSun">Exposed To Sun</returns>
        /// <search>
        /// Query, ExposedToSun, surfaceType
        /// </search>
        public static bool ExposedToSun(string surfaceType)
        {
            if (string.IsNullOrEmpty(surfaceType))
                return false;

            string aSurfaceType = surfaceType.ToLower();
            aSurfaceType = aSurfaceType.Replace(" ", string.Empty);

            return aSurfaceType == "raisedfloor" || aSurfaceType == "exteriorwall" || aSurfaceType == "roof";
        }

        /***************************************************/
    }
}


