//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BHE = BH.oM.Environment.Elements;
//using BHG = BH.oM.Geometry;
//using BHB = BH.oM.Base;
//using BH.Engine.Geometry;

//namespace XML_Adapter.GBXML
//{
//    public class AdjSolver
//    {

//        public static List<BHE.BuildingElementPanel> AdjacensiesSolver(List<BHE.BuildingElementPanel> pansin, List<BHE.Space> spacesin)
//        {

//            List<BHE.Space> spaces = new List<BHE.Space>();
//            List<BHE.BuildingElementPanel> pans = new List<BHE.BuildingElementPanel>();

//            foreach (BHE.BuildingElementPanel pan in pansin)
//            {
//                pans.Add((BHE.BuildingElementPanel)pan.GetShallowClone());
//            }

//            foreach (BHE.Space space in spacesin)
//            {
//                BHE.Space spaceout = (BHE.Space)space.GetShallowClone();
//                //spaceout.Polylines = new List<BHG.Polyline>(); oscar
//                for (int i = 0; i < space.Polylines.Count; i++)
//                {
//                    spaceout.Polylines.Add((BHG.Polyline)space.Polylines[i].Duplicate());
//                }
//                spaces.Add(spaceout);
//            }

//            for (int i = 0; i < pans.Count; i++)
//            {
//                pans[i].adjSpaces.Clear();

//                for (int j = 0; j < spaces.Count; j++)
//                {
//                    if (pans[i].adjSpaces.Count >= 2)
//                    {
//                        break;
//                    }
//                    for (int k = 0; k < spaces[j].Polylines.Count; k++)
//                    {
//                        if (pans[i].External_Contours[0].ControlPoints.Count == spaces[j].Polylines[k].ControlPoints.Count)
//                        {
//                            bool adjFound = true;
//                            for (int l = 0; l < pans[i].External_Contours[0].ControlPoints.Count; l++)
//                            {
//                                bool breakout = true;
//                                for (int m = 0; m < spaces[j].Polylines[k].PointCount; m++)
//                                {
//                                    if (pans[i].External_Contours[0].ControlPoints[l].DistanceTo(spaces[j].Polylines[k][m]) <= 0.001)
//                                    {
//                                        breakout = false;
//                                        break;
//                                    }
//                                }
//                                if (breakout == true)
//                                {
//                                    adjFound = false;
//                                    break;
//                                }
//                            }
//                            if (adjFound == true)
//                            {
//                                pans[i].adjSpaces.Add(spaces[j].BHoM_Guid.ToString());
//                                spaces[j].Polylines.RemoveAt(k);
//                                break;
//                            }
//                        }
//                    }
//                }
//            }
//            return pans;
//        }
//    }
//}
