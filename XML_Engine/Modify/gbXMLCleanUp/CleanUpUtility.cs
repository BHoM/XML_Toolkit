using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHE = BH.oM.Environmental.Elements;
using BH.Engine.Geometry;
using BH.Engine.Environment;

using BH.oM.Geometry;
using BH.Engine.XML;

namespace XML_Engine.Modify.gbXMLCleanUp
{
    public static class CleanUpUtility
    {
        public static List<BHE.BuildingElement> GetBuildingElements(this BHE.Building building)
        {
            List<BHE.BuildingElement> rtn = new List<BHE.BuildingElement>();

            rtn.AddRange(building.BuildingElements.FindAll(x => x.BuildingElementProperties != null && x.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Window && x.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Door)); //Add only shade elements
        
            foreach(BHE.Space s in building.Spaces)
            {
                foreach(BHE.BuildingElement be in s.BuildingElements)
                {
                    if(be != null)
                        if (rtn.Where(x => x.BHoM_Guid == be.BHoM_Guid).FirstOrDefault() == null && be.BuildingElementProperties != null && be.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Window && be.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Door) //Add only shade elements
                            rtn.Add(be);
                }
            }

            return rtn;
        }

        public static Dictionary<BHE.BuildingElement, List<BHE.BuildingElement>> IdentifyOverlaps(this BHE.BuildingElement be, List<BHE.BuildingElement> bes)
        {
            Dictionary<BHE.BuildingElement, List<BHE.BuildingElement>> rtn = new Dictionary<BHE.BuildingElement, List<BHE.BuildingElement>>();
            rtn.Add(be, new List<BHE.BuildingElement>());

            Polyline be1P = be.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);

            foreach (BHE.BuildingElement be2 in bes)
            {
                if(be2.BHoM_Guid != be.BHoM_Guid)
                {
                    Polyline be2P = be2.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);
                    if (be1P.IsCoplanar(be2P))
                    {
                        List<Polyline> intersections = be1P.BooleanIntersection(be2P);
                        if (intersections.Count > 0)
                            rtn[be].Add(be2);
                    }
                }
            }

            return rtn;
        }

        public static Dictionary<BHE.BuildingElement, List<BHE.BuildingElement>> SplitElement(this BHE.BuildingElement be, List<BHE.BuildingElement> bes)
        {
            Dictionary<BHE.BuildingElement, List<BHE.BuildingElement>> rtn = new Dictionary<BHE.BuildingElement, List<BHE.BuildingElement>>();

            Polyline be1P = be.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);
            Dictionary<BHE.BuildingElement, List<Polyline>> replacementGeom = new Dictionary<BHE.BuildingElement, List<Polyline>>();

            //Get the new polylines for each building element
            foreach (BHE.BuildingElement be2 in bes)
            {
                Polyline be2P = be2.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06); //No need to check CoPlanarity as this was done when identifiying the overlap

                Dictionary<BHE.BuildingElement, List<Polyline>> geomBuild = new Dictionary<BHE.BuildingElement, List<Polyline>>();
                geomBuild.Add(be, new List<Polyline>());
                geomBuild.Add(be2, new List<Polyline>());

                List<Polyline> intersections = be1P.BooleanIntersection(be2P);
                foreach(Polyline inte in intersections)
                {
                    geomBuild[be].AddRange(be1P.SplitAtPoints(inte.ControlPoints));
                    geomBuild[be2].AddRange(be2P.SplitAtPoints(inte.ControlPoints));
                }

                foreach(KeyValuePair<BHE.BuildingElement, List<Polyline>> kvp in geomBuild)
                {
                    List<Polyline> remove = new List<Polyline>();
                    foreach (Polyline p5 in kvp.Value)
                    {
                        bool isNotIn = true;
                        foreach (Point px in p5.ControlPoints)
                        {
                            foreach (Polyline p6 in intersections)
                            {
                                if (p6.ControlPoints.Contains(px))
                                    isNotIn = false;

                                if (!isNotIn) break;
                            }
                            if (!isNotIn) break;
                        }

                        if (!isNotIn)
                            remove.Add(p5);
                    }

                    foreach (Polyline l5 in remove)
                        kvp.Value.Remove(l5);
                }

                foreach (KeyValuePair<BHE.BuildingElement, List<Polyline>> kvp in geomBuild)
                {
                    foreach (Polyline p5 in kvp.Value)
                    {
                        while (p5.ControlPoints.Last().Distance(p5.ControlPoints.First()) > 0.01)
                        {
                            bool addedPoint = false;
                            foreach (Point px in intersections[0].ControlPoints)
                            {
                                if (!p5.ControlPoints.Contains(px) && !kvp.Key.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).ControlPoints.Contains(px) && px.Match2Of3(p5.ControlPoints.Last()))
                                {
                                    p5.ControlPoints.Add(px);
                                    addedPoint = true;
                                }
                            }
                            if (!addedPoint)
                                p5.ControlPoints.Add(p5.ControlPoints[0]);
                        }
                    }
                }

                foreach (KeyValuePair<BHE.BuildingElement, List<Polyline>> kvp in geomBuild)
                {
                    if (!replacementGeom.ContainsKey(kvp.Key))
                        replacementGeom.Add(kvp.Key, new List<Polyline>());
                    replacementGeom[kvp.Key].AddRange(kvp.Value);
                }
            }

            //Make the new BE from the new polylines
            foreach(KeyValuePair<BHE.BuildingElement, List<Polyline>> kvp in replacementGeom)
            {
                BHE.BuildingElement ori = kvp.Key;
                if(kvp.Value.Count > 1)
                {
                    //Only do this if we have more than 1 BE to replace
                    foreach(Polyline p in kvp.Value)
                    {
                        BHE.BuildingElement newBE = ori.GetShallowClone() as BHE.BuildingElement;
                        if (newBE.BuildingElementGeometry == null)
                            newBE.BuildingElementGeometry = ori.BuildingElementGeometry.Copy();
                        if (newBE.BuildingElementProperties == null)
                            newBE.BuildingElementProperties = ori.BuildingElementProperties.GetShallowClone() as BH.oM.Environmental.Properties.BuildingElementProperties;

                        newBE.BuildingElementGeometry.ISetGeometry(p);

                        if (!rtn.ContainsKey(ori))
                            rtn.Add(ori, new List<BHE.BuildingElement>());
                        rtn[ori].Add(newBE);
                    }
                }
                else if(kvp.Value.Count == 0)
                {
                    //This BE was cut in such a way that it ended up with no polygon - slightly problematic but basically the entire polygon was the intersection with the other BE - so add the old BE back
                    if (!rtn.ContainsKey(ori))
                        rtn.Add(ori, new List<BHE.BuildingElement>());
                    rtn[ori].Add(ori);
                }
            }

            return rtn;
        }

        public static BHE.Building ChangeBuildingElements(this BHE.Building building, BHE.BuildingElement beToRemove, List<BHE.BuildingElement> besToAdd)
        {
            //Remove the BE from the spaces
            for (int x = 0; x < building.Spaces.Count; x++)
            {
                bool removed = false;
                for (int y = 0; y < building.Spaces[x].BuildingElements.Count; y++)
                {
                    if (building.Spaces[x].BuildingElements[y].BHoM_Guid == beToRemove.BHoM_Guid)
                    {
                        building.Spaces[x].BuildingElements.Remove(beToRemove);
                        removed = true;
                    }
                }
                if (removed)
                {
                    foreach (BHE.BuildingElement be in besToAdd)
                        if (!be.AdjacentSpaces.Contains(building.Spaces[x].BHoM_Guid))
                            be.AdjacentSpaces.Add(building.Spaces[x].BHoM_Guid);
                    building.Spaces[x].BuildingElements.AddRange(besToAdd);
                }
            }

            //Add the building elements
            if (building.BuildingElements.Where(x => x.BHoM_Guid == beToRemove.BHoM_Guid).FirstOrDefault() != null)
            {
                building.BuildingElements.Remove(beToRemove);
                building.BuildingElements.AddRange(besToAdd);
            }

            return building;
        }

        public static BHE.Building RemovePerfectOverlaps(this BHE.Building building, BHE.BuildingElement beToRemove)
        {
            List<BHE.BuildingElement> allBEs = building.GetBuildingElements();
            List<BHE.BuildingElement> matchingBEs = allBEs.Where(x => x.MatchBEs(beToRemove)).ToList();

            Polyline be1P = beToRemove.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);
            List<BHE.BuildingElement> closeBEs = allBEs.Where(x => x.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).Centre().Distance(be1P.Centre()) < 0.01).ToList();

            for (int x = 1; x < matchingBEs.Count; x++)
            {
                //Start at 1 and remove all of the BuildingElements that match the one to be removed
                building.BuildingElements.Remove(matchingBEs[x]);
                List<BHE.Space> spaces = building.Spaces.Where(a => a.BuildingElements.Contains(matchingBEs[x])).ToList();
                foreach (BHE.Space s in spaces)
                    s.BuildingElements.Remove(matchingBEs[x]);
            }

            return building;
            /*double tol = 0.01; //0.01;

            List<BHE.BuildingElement> allBes = building.GetBuildingElements();
            Polyline be1P = beToRemove.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);

            List<BHE.BuildingElement> closeBEs = allBes.Where(x => x.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).Centre().Distance(be1P.Centre()) < tol).ToList();

            List<BHE.BuildingElement> toRemove = new List<BHE.BuildingElement>();

            foreach(BHE.BuildingElement be2 in closeBEs)
            {
                //Check that every control point of BE2 is in the control point of beToRemove
                Polyline be2P = be2.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);

                bool isMatch = true;
                if (be1P.ControlPoints.Count != be2P.ControlPoints.Count)
                    isMatch = false;
                else
                {
                    foreach (Point px in be1P.ControlPoints)
                    {
                        if (!be2P.ControlPoints.Contains(px))
                            isMatch = false;
                    }

                    if (!isMatch)
                    {
                        foreach (Point px in be1P.ControlPoints)
                            isMatch &= px.OnePointMatchesTol(be2P.ControlPoints);
                    }
                }
                if (isMatch) //This BE is a perfect overlap - centre point is less than 0.01 unit away from each other, and every control point of BE1P is in the control points of BE2P - Remove this BE from the building/Spaces
                    toRemove.Add(be2);
            }

            BHE.BuildingElement newBE = beToRemove.GetShallowClone(true) as BHE.BuildingElement;
            if (newBE.BuildingElementGeometry == null)
                newBE.BuildingElementGeometry = beToRemove.BuildingElementGeometry.Copy();
            if (newBE.BuildingElementProperties == null)
                newBE.BuildingElementProperties = beToRemove.BuildingElementProperties.GetShallowClone() as BH.oM.Environmental.Properties.BuildingElementProperties;

            List<BHE.Space> removedFrom = new List<BHE.Space>();
            bool removedFromBuilding = false;
            //Remove the BEs
            foreach(BHE.BuildingElement be2 in toRemove)
            {
                //Map any adjacencies as necessary
                foreach(Guid g in be2.AdjacentSpaces)
                {
                    if (!newBE.AdjacentSpaces.Contains(g))
                        newBE.AdjacentSpaces.Add(g);
                }

                for (int x = 0; x < building.Spaces.Count; x++)
                {
                    for (int y = 0; y < building.Spaces[x].BuildingElements.Count; y++)
                    {
                        if (building.Spaces[x].BuildingElements[y].BHoM_Guid == be2.BHoM_Guid)
                        {
                            building.Spaces[x].BuildingElements.Remove(be2);
                            removedFrom.Add(building.Spaces[x]);
                        }
                    }
                }

                if (building.BuildingElements.Where(x => x.BHoM_Guid == be2.BHoM_Guid).FirstOrDefault() != null)
                {
                    building.BuildingElements.Remove(be2);
                    removedFromBuilding = true;
                }
            }

            foreach(BHE.Space s in removedFrom)
                building.Spaces.Where(x => x.BHoM_Guid == s.BHoM_Guid).First().BuildingElements.Add(newBE);

            if (removedFromBuilding)
                building.BuildingElements.Add(newBE);

            return building;*/
        }

        public static BHE.BuildingElement AmendSingleAdjacencies(this BHE.BuildingElement be, BHE.Building building)
        {
            //Find additional adjacencies from panels that have a similar (within tolerance 'tol') centre point geometry (standard centre point method)
            if (be.AdjacentSpaces.Count == 1)
            {
                double tol = 0.01; //0.01
                Point beCentre = be.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).Centre();
                List<BHE.BuildingElement> foundElements = building.BuildingElements.Where(x => x.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).Centre().Distance(beCentre) < tol).ToList();

                foreach (BHE.BuildingElement be2 in foundElements)
                {
                    if (be2.BHoM_Guid != be.BHoM_Guid)
                    {
                        foreach (Guid g in be2.AdjacentSpaces)
                            if (!be.AdjacentSpaces.Contains(g))
                                be.AdjacentSpaces.Add(g);
                    }
                }
            }

            return be;
        }

        public static BHE.BuildingElement AmendSingleAdjacencies(this BHE.BuildingElement be, BHE.Space space)
        {
            if (space == null)
                return be;

            //Find additional adjacencies from panels with same plane as panels in the space of the single adjacency
            if (be.AdjacentSpaces.Count == 1)
            {
                Polyline be1P = be.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);
                List<BHE.BuildingElement> foundElements = space.BuildingElements.Where(x => x.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).IsCoplanar(be1P)).ToList();

                foreach (BHE.BuildingElement be2 in foundElements)
                {
                    if (be2.BHoM_Guid != be.BHoM_Guid)
                    {
                        foreach (Guid g in be2.AdjacentSpaces)
                            if (!be.AdjacentSpaces.Contains(g))
                                be.AdjacentSpaces.Add(g);
                    }
                }
            }
            return be;
        }

        public static BHE.BuildingElement AmendSingleAdjacencies(this BHE.BuildingElement be, List<BHE.BuildingElement> potentialMatches)
        {
            //Find additional adjacencies from panels that have a similar (within tolerance 'tol') centre point geometry (area centroid method)
            if (be.AdjacentSpaces.Count == 1)
            {
                double tol = 0.01;
                Polyline be1P = be.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);
                Point beCentre = be1P.PolygonCentre();
                List<BHE.BuildingElement> foundElements = potentialMatches.Where(x => x.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).PolygonCentre().Distance(beCentre) < tol && x.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).IsCoplanar(be1P)).ToList();

                foreach(BHE.BuildingElement be2 in foundElements)
                {
                    if(be2.BHoM_Guid != be.BHoM_Guid)
                    {
                        foreach (Guid g in be2.AdjacentSpaces)
                            if (!be.AdjacentSpaces.Contains(g))
                                be.AdjacentSpaces.Add(g);
                    }
                }
            }

            return be;
        }

        public static BHE.BuildingElement AmendXMLType(this BHE.BuildingElement be)
        {
            String dictionaryKey = "SAM_BuildingElementType";

            if (be.BuildingElementProperties == null)
                be.BuildingElementProperties = new BH.oM.Environmental.Properties.BuildingElementProperties();

            if(be.BuildingElementProperties.BuildingElementType == BHE.BuildingElementType.Window || be.BuildingElementProperties.BuildingElementType == BHE.BuildingElementType.Door)
                return be;

            if (!be.BuildingElementProperties.CustomData.ContainsKey(dictionaryKey))
                be.BuildingElementProperties.CustomData.Add(dictionaryKey, "");

            if (be.AdjacentSpaces.Count == 0)
                be.BuildingElementProperties.CustomData[dictionaryKey] = "Shade";
            else
            {
                double tilt = BH.Engine.Environment.Query.Tilt(be.BuildingElementGeometry);

                if (be.AdjacentSpaces.Count == 1)
                {
                    //Elements with 1 adjacent space are either exterior walls or floors
                    if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("", StringComparison.CurrentCultureIgnoreCase))
                    {
                        //New building element type - calculate wall or floor
                        if (tilt >= 70 && tilt <= 120)
                            be.BuildingElementProperties.CustomData[dictionaryKey] = "External Wall";
                        else
                            be.BuildingElementProperties.CustomData[dictionaryKey] = "Exposed Floor";
                    }
                    else if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Internal Wall", StringComparison.CurrentCultureIgnoreCase) || (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Air", StringComparison.CurrentCultureIgnoreCase) && be.BuildingElementProperties.BuildingElementType == BHE.BuildingElementType.Wall)) //Incorrectly categorised as internal wall or air - should be external wall
                        be.BuildingElementProperties.CustomData[dictionaryKey] = "External Wall";
                    else if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Internal Floor", StringComparison.CurrentCultureIgnoreCase) || (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Air", StringComparison.CurrentCultureIgnoreCase) && be.BuildingElementProperties.BuildingElementType == BHE.BuildingElementType.Floor) )//Incorrectly categorised as internal floor or air - should be external (exposed) floor
                        be.BuildingElementProperties.CustomData[dictionaryKey] = "Exposed Floor";
                }
                else if (be.AdjacentSpaces.Count == 2)
                {
                    //Elements with 2 adjacent spaces are either internal walls or floors
                    if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (tilt >= 70 && tilt <= 120)
                            be.BuildingElementProperties.CustomData[dictionaryKey] = "Internal Wall";
                        else
                            be.BuildingElementProperties.CustomData[dictionaryKey] = "Internal Floor";
                    }
                    else
                    {
                        if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Contains("External"))
                        {
                            be.BuildingElementProperties.CustomData[dictionaryKey] = be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Replace("External", "Internal");
                            be.BuildingElementProperties.Name = be.BuildingElementProperties.Name.Replace("EXT", "INT");
                        }
                        else if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Roof", StringComparison.CurrentCultureIgnoreCase))
                            be.BuildingElementProperties.CustomData[dictionaryKey] = "Ceiling";
                        else if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Raised Floor", StringComparison.CurrentCultureIgnoreCase) || be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Exposed Floor", StringComparison.CurrentCultureIgnoreCase))
                            be.BuildingElementProperties.CustomData[dictionaryKey] = "Internal Floor";
                        else if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("Curtain Wall", StringComparison.CurrentCultureIgnoreCase))
                            be.BuildingElementProperties.CustomData[dictionaryKey] = "Internal Wall";
                    }
                }
            }

            return be;
        }

        public static BHE.BuildingElement AmendCadObjectID(this BHE.BuildingElement be, BHE.Building building)
        {
            if (be.CADObjectError() == null) return null; //This element does not have a CAD object error
            else return be.PropertiesFromAdjSrf(be.AdjacentSurface(building), building);

            /*String dictionaryKey = "Family Name";
            if (be.BuildingElementProperties == null)
                be.BuildingElementProperties = new BH.oM.Environmental.Properties.BuildingElementProperties();

            if (be.BuildingElementProperties.Name.Equals("", StringComparison.CurrentCultureIgnoreCase))
                be.BuildingElementProperties.Name = "SIM_INT_SLD";

            if (!be.BuildingElementProperties.CustomData.ContainsKey(dictionaryKey))
                be.BuildingElementProperties.CustomData.Add(dictionaryKey, "");

            //Missing family name
            if (be.BuildingElementProperties.CustomData[dictionaryKey].ToString().Equals("", StringComparison.CurrentCultureIgnoreCase))
            {
                double tilt = BH.Engine.Environment.Query.Tilt(be.BuildingElementGeometry);
                if (tilt >= 70 && tilt <= 120)
                    be.BuildingElementProperties.CustomData[dictionaryKey] = "Basic Wall";
                else if (tilt == 0)
                    be.BuildingElementProperties.CustomData[dictionaryKey] = "Floor";
                else if (tilt == 180)
                    be.BuildingElementProperties.CustomData[dictionaryKey] = "Basic Roof";

                if (!be.BuildingElementProperties.CustomData.ContainsKey("Revit_elementId"))
                    be.BuildingElementProperties.CustomData.Add("Revit_elementId", "");

                be.BuildingElementProperties.CustomData["Revit_elementId"] = "CADObjectID";
            }

            //Wrong CADObjectID
            if (be.AdjacentSpaces.Count == 0) //don't update CAD object ID for shade elements. 
                return be;
            else if (be.BuildingElementProperties.Name.Contains("EXT") && be.AdjacentSpaces.Count != 1 && be.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Window && be.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Door)
                be.BuildingElementProperties.Name = be.BuildingElementProperties.Name.Replace("EXT", "INT");
            else if (be.BuildingElementProperties.Name.Contains("INT") && be.AdjacentSpaces.Count != 2 && be.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Window && be.BuildingElementProperties.BuildingElementType != BHE.BuildingElementType.Door)
            {
                be.BuildingElementProperties.Name = be.BuildingElementProperties.Name.Replace("INT", "EXT");
                be.BuildingElementProperties.Name = be.BuildingElementProperties.Name.Replace("FLR02_RAISED_900", "Exposed");
            }

            return be;*/
        }

        public static bool Match2Of3(this Point pt, Point comp)
        {
            bool match2 = false;
            if (pt.X == comp.X)
            {
                if (pt.Y == comp.Y)
                    match2 = true;
                else if (pt.Z == comp.Z)
                    match2 = true;
            }
            else if (pt.Y == comp.Y && pt.Z == comp.Z)
                match2 = true;

            return match2;
        }

        public static bool MatchBEs(this BHE.BuildingElement be2, Polyline be1P)
        {
            Polyline be2P = be2.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);

            double tol = 0.01;
            if (be2P.Centre().Distance(be1P.Centre()) > tol)
                return false; //Centre points don't match within the tolerance so probably not the same BE

            bool isMatch = true;
            if (be1P.ControlPoints.Count != be2P.ControlPoints.Count)
                isMatch = false;
            else
            {
                foreach (Point px in be1P.ControlPoints)
                {
                    if (!be2P.ControlPoints.Contains(px))
                        isMatch = false;
                }

                if(!isMatch)
                {
                    isMatch = true;
                    foreach (Point px in be1P.ControlPoints)
                        isMatch &= px.OnePointMatchesTol(be2P.ControlPoints);
                }
            }

            return isMatch;
        }

        public static bool MatchBEs(this BHE.BuildingElement be2, BHE.BuildingElement refBE)
        {
            Polyline be1P = refBE.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);
            return be2.MatchBEs(be1P);
        }

        public static bool OnePointMatchesTol(this Point px, List<Point> points, double tol = 0.1)
        {
            foreach(Point p1 in points)
            {
                if (px.Distance(p1) <= tol)
                    return true;
            }

            return false;
        }

        public static bool OnePointMatchesTol(this List<Point> px, List<Point> points, double tol = 0.1)
        {
            bool match = true;
            foreach (Point pt in px)
                match &= pt.OnePointMatchesTol(points, tol);

            return match;
        }

        public static BHE.Space CleanSpace(this BHE.Space space)
        {
            //Make sure there are no duplicate BEs in the space
            for (int x = 0; x < space.BuildingElements.Count; x++)
            {
                BHE.BuildingElement newBE = space.BuildingElements[x].GetShallowClone() as BHE.BuildingElement;
                Polyline be1P = space.BuildingElements[x].BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);
                if (newBE.BuildingElementGeometry == null && space.BuildingElements[x].BuildingElementGeometry != null)
                    newBE.BuildingElementGeometry = space.BuildingElements[x].BuildingElementGeometry.Copy();
                if (newBE.BuildingElementProperties == null && space.BuildingElements[x].BuildingElementProperties != null)
                    newBE.BuildingElementProperties = space.BuildingElements[x].BuildingElementProperties.GetShallowClone() as BH.oM.Environmental.Properties.BuildingElementProperties;

                bool wasRemoved = false;

                for (int y = 0; y < space.BuildingElements.Count; y++)
                {
                    if (space.BuildingElements[y].MatchBEs(be1P))
                    {
                        foreach(Guid g in space.BuildingElements[y].AdjacentSpaces)
                        {
                            if (!newBE.AdjacentSpaces.Contains(g))
                                newBE.AdjacentSpaces.Add(g);
                        }
                        space.BuildingElements.Remove(space.BuildingElements[y]);
                        wasRemoved = true;
                    }
                }

                if(wasRemoved)
                    space.BuildingElements.Add(newBE);
            }

            return space;
        }

        public static BHE.Building CleanBEs(this BHE.Building building, BHE.BuildingElement beToCheck)
        {
            List<BHE.BuildingElement> allBEs = building.GetBuildingElements();

            List<BHE.BuildingElement> matchingBEs = allBEs.Where(x => x.MatchBEs(beToCheck)).ToList();

            for(int x = 1; x < matchingBEs.Count; x++)
            {
                building.BuildingElements.Remove(matchingBEs[x]);
                List<BHE.Space> spaces = building.Spaces.Where(a => a.BuildingElements.Contains(matchingBEs[x])).ToList();
                foreach (BHE.Space s in spaces)
                    s.BuildingElements.Remove(matchingBEs[x]);
            }

            return building;
        }

        public static BHE.Building CleanBuildingDupAdj(this BHE.Building building, BHE.BuildingElement be)
        {
            Polyline be1P = be.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06);
            double tol = 0.1; //0.01
            List<BHE.BuildingElement> nearestElements = building.GetBuildingElements().Where(x => x.BuildingElementGeometry.ICurve().ICollapseToPolyline(1e-06).Centre().Distance(be1P.Centre()) < tol).ToList();

            //Check the Building elements on the building
            foreach(BHE.BuildingElement be2 in nearestElements)
            {
                if (be2.AdjacentSpaces.AdjacenciesMatch(be.AdjacentSpaces) && be2.BHoM_Guid != be.BHoM_Guid)
                {
                    int index3 = building.BuildingElements.IndexOf(be2);
                    if(index3 < building.BuildingElements.Count && index3 != -1)
                        building.BuildingElements[index3] = be;
                    List<BHE.Space> associatedSpaces = building.Spaces.Where(x => be.AdjacentSpaces.Contains(x.BHoM_Guid)).ToList();
                    foreach(BHE.Space s in associatedSpaces)
                    {
                        int index = building.Spaces.IndexOf(s);
                        if (index < building.Spaces.Count && index != -1)
                        {
                            int index2 = building.Spaces[index].BuildingElements.IndexOf(be2);
                            if(index2 < building.Spaces[index].BuildingElements.Count && index2 != -1)
                                building.Spaces[index].BuildingElements[index2] = be;
                        }
                    }
                }
            }

            /*for(int x = 0; x < building.BuildingElements.Count; x++)
            {
                if(building.BuildingElements[x].AdjacentSpaces.AdjacenciesMatch(be.AdjacentSpaces) && building.BuildingElements[x].BHoM_Guid != be.BHoM_Guid)
                {
                    //This BE should be updated to not be duplicate BEs...
                    building.BuildingElements[x] = be;
                }
            }

            //Check the spaces building elements
            for (int x = 0; x < building.Spaces.Count; x++)
            {
                for(int y = 0; y < building.Spaces[x].BuildingElements.Count; y++)
                {
                    if(building.Spaces[x].BuildingElements[y].AdjacentSpaces.AdjacenciesMatch(be.AdjacentSpaces) && building.Spaces[x].BuildingElements[y].BHoM_Guid != be.BHoM_Guid)
                    {
                        building.Spaces[x].BuildingElements[y] = be;
                    }
                }
            }*/

            return building;
        }

        public static bool AdjacenciesMatch(this List<Guid> firstAdj, List<Guid> secondAdj)
        {
            if (firstAdj.Count != secondAdj.Count)
                return false;

            bool rtn = true;
            for (int x = 0; x < firstAdj.Count; x++)
                rtn &= (secondAdj[x] == firstAdj[x]);

            return rtn;
        }
    }
}
