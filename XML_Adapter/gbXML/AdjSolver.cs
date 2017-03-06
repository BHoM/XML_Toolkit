using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHE = BHoM.Environmental.Elements;
using BHG = BHoM.Geometry;

namespace XML_Adapter.gbXML
{
    public class AdjSolver
    {

        public static List<BHE.Panel> AdjacensiesSolver(List<BHE.Panel> pans, List<BHE.Space> spaces)
        {
            //int adj1 = 0;
            //int adj2 = 0;
            for (int i = 0; i < pans.Count - 1 ; i++)
            {
                for (int j = 0; j < spaces.Count - 1 ; j++)
                {
                    if (pans[i].adjSpaces.Count >= 2)
                    {
                        break;
                    }
                    for (int k = 0; k < spaces[j].Polylines.Count - 1 ; k++)
                    {
                        if (pans[i].External_Contours[0].ControlPoints.Count == spaces[j].Polylines[k].PointCount)
                        {
                            bool adjFound = true;
                            for (int l = 0; l < pans[i].External_Contours[0].ControlPoints.Count; l++)
                            {
                                bool breakout = true;
                                for (int m = 0; m < spaces[j].Polylines[k].PointCount; m++)
                                {
                                    
                                    if (pans[i].External_Contours[0].ControlPoints[l].DistanceTo(spaces[j].Polylines[k][m]) <= 0.001)
                                    {
                                        breakout = false;
                                        break;
                                    }
                                }
                                if (breakout == true)
                                {
                                    adjFound = false;
                                    break;
                                }
                            }
                            if (adjFound == true)
                            {
                                pans[i].adjSpaces.Add(spaces[j].BHoM_Guid.ToString());
                                spaces[j].Polylines.RemoveAt(k);
                                //if (pans[i].adjSpaces.Count == 1)
                                //{
                                //    adj1 = adj1 + 1;
                                //}
                                //if (pans[i].adjSpaces.Count == 2)
                                //{
                                //    adj1 = adj1 - 1;
                                //    adj2 = adj2 + 1;
                                //}
                                break;
                            }
                        }
                    }
                }
            }
            return pans;
        }
    }
}
