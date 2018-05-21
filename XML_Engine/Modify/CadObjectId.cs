using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.XML;
using BH.oM.Base;
using BHE = BH.oM.Environmental.Elements;
using BHP = BH.oM.Environmental.Properties;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.Engine.Environment;


namespace BH.Engine.XML
{

    public static partial class Modify
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static BHE.Building CadObjectId(this BHE.Building building, List<BHE.BuildingElement> elements)
        {
            foreach (BHE.BuildingElement element in elements)
            {
                //Case 1: No CADObjectId at all - atm we use the default value: SIM_INT_SLD (atm: ERROR)
                if (element.BuildingElementProperties == null || element.BuildingElementProperties.Name == "" || element.BuildingElementProperties.CustomData["Family Name"].ToString() == "")
                {
                    element.BuildingElementProperties = new oM.Environmental.Properties.BuildingElementProperties();
                    building.BuildingElements.Where(x => x.BHoM_Guid == element.BHoM_Guid).FirstOrDefault().BuildingElementProperties = new oM.Environmental.Properties.BuildingElementProperties() { Name = "Error" };

                    building.Spaces.SelectMany(x => x.BuildingElements).Where(x => x.BHoM_Guid == element.BHoM_Guid).FirstOrDefault().BuildingElementProperties = new oM.Environmental.Properties.BuildingElementProperties() { Name = "Error" };

                    if (Environment.Query.Tilt(element.BuildingElementGeometry) >= 70 && Environment.Query.Tilt(element.BuildingElementGeometry) <= 120)
                    {
                        building.BuildingElements.Where(x => x.BHoM_Guid == element.BHoM_Guid).FirstOrDefault().BuildingElementProperties.CustomData.Add("Family Name", "Basic Wall");
                        building.Spaces.SelectMany(x => x.BuildingElements).Where(x => x.BHoM_Guid == element.BHoM_Guid).FirstOrDefault().BuildingElementProperties.CustomData.Add("Family Name", "Basic Wall");
                    }
                    else if (Environment.Query.Tilt(element.BuildingElementGeometry) == 0)
                    {
                        building.BuildingElements.Where(x => x.BHoM_Guid == element.BHoM_Guid).FirstOrDefault().BuildingElementProperties.CustomData.Add("Family Name", "Floor");
                        building.Spaces.SelectMany(x => x.BuildingElements).Where(x => x.BHoM_Guid == element.BHoM_Guid).FirstOrDefault().BuildingElementProperties.CustomData.Add("Family Name", "Floor");
                    }
                    else if (Environment.Query.Tilt(element.BuildingElementGeometry) == 180)
                    {
                        building.BuildingElements.Where(x => x.BHoM_Guid == element.BHoM_Guid).FirstOrDefault().BuildingElementProperties.CustomData.Add("Family Name", "Basic Roof");
                        building.Spaces.SelectMany(x => x.BuildingElements).Where(x => x.BHoM_Guid == element.BHoM_Guid).FirstOrDefault().BuildingElementProperties.CustomData.Add("Family Name", "Basic Roof");
                    }

                    building.BuildingElements.Where(x => x.BHoM_Guid == element.BHoM_Guid).FirstOrDefault().BuildingElementProperties.CustomData.Add("Revit_elementId", "CADObjectID");

                    building.Spaces.SelectMany(x => x.BuildingElements).Where(x => x.BHoM_Guid == element.BHoM_Guid).FirstOrDefault().BuildingElementProperties.CustomData.Add("Revit_elementId", "CADObjectID");

                }

                //Case 2: Wrong CADObjectId
                if (element.BuildingElementProperties.Name.Contains("EXT") && element.AdjacentSpaces.Count != 1)
                {
                    building.BuildingElements.Where(x => x.BHoM_Guid == element.BHoM_Guid).FirstOrDefault().BuildingElementProperties.Name.Replace("EXT", "INT");
                    building.Spaces.SelectMany(x => x.BuildingElements).Where(x => x.BHoM_Guid == element.BHoM_Guid).FirstOrDefault().BuildingElementProperties.Name.Replace("EXT", "INT");
                }


                if (element.BuildingElementProperties.Name.Contains("INT") && element.AdjacentSpaces.Count != 2)
                {
                    building.BuildingElements.Where(x => x.BHoM_Guid == element.BHoM_Guid).FirstOrDefault().BuildingElementProperties.Name.Replace("INT", "EXT");
                    building.Spaces.SelectMany(x => x.BuildingElements).Where(x => x.BHoM_Guid == element.BHoM_Guid).FirstOrDefault().BuildingElementProperties.Name.Replace("INT", "EXT");
                }




                //1. If we dont have CADOBJId at all:
                //SIM_INT_SLD_CADOBjectID

                //2. Wrong CADObjectID. not matching adjacencies. 
                // 2 adjacencies: FamilyName: SIM_INT_SLD
                //1 ajdacency: FamilyName:SIM_EXT_SLD


            }
            return building;
        }

        /***************************************************/


    }
}




