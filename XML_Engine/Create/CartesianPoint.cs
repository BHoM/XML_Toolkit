using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BH.oM.XML;

namespace BH.Engine.XML
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static CartesianPoint CartesianPoint(string x = "", string y = "", string z = "")
        { 
            CartesianPoint pt = new CartesianPoint();
            string[] coords = new string[3];
            coords[0] = x;
            coords[1] = y;
            coords[2] = z;
            pt.Coordinate = coords;
            return pt;
        }

        public static CartesianPoint CartesianPoint(int x = 0, int y = 0, int z = 0)
        {
            return CartesianPoint(x.ToString(), y.ToString(), z.ToString());
        }

        public static CartesianPoint CartesianPoint(double x = 0, double y = 0, double z= 0)
        {
            return CartesianPoint(x.ToString(), y.ToString(), z.ToString());   
        }
    }
}
