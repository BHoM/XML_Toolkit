/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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

using BH.Engine.Environment;
using BH.Engine.XML;
using BH.oM.Base;
using BH.oM.Environment.Elements;
using BH.oM.XML;
using BH.oM.XML.Enums;
using BH.oM.XML.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Geometry.SettingOut;

namespace BH.Adapter.XML
{
    public partial class GBXMLSerializer
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static void Serialize<T>(IEnumerable<T> bhomObjects, BH.oM.XML.GBXML gbx, XMLFileSettings filesettings, XMLSettings settings) where T : IObject
        {
            switch (settings.ExportDetail)
            {
                case ExportDetail.Full:
                    SerializeCollectionFull(bhomObjects as dynamic, gbx, settings);
                    break;
                case ExportDetail.BuildingShell:
                    SerializeBuildingShell(bhomObjects as dynamic, gbx, settings);
                    break;
                case ExportDetail.IndividualSpaces:
                    SerializeSpaces(bhomObjects, filesettings, settings);
                    break;
                default:
                    throw new NotImplementedException("The ExportDetail has not been set. Please set the ExportDetail to continue");
            }

            // Document History                          
            DocumentHistory DocumentHistory = new DocumentHistory();
            DocumentHistory.CreatedBy.Date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            gbx.DocumentHistory = DocumentHistory;
        }

        public static void SerializeSpaces<T>(IEnumerable<T> objects, XMLFileSettings fileSettings, XMLSettings settings) where T : IObject
        {
            SerializeBuildingSpaces(objects as dynamic, fileSettings, settings);
        }

        private static void SerializeCollectionFull(IEnumerable<BH.oM.XML.Environment.DocumentBuilder> documents, GBXML gbx, XMLSettings settings)
        {
            foreach (BH.oM.XML.Environment.DocumentBuilder db in documents)
            {
                MongoDB.Bson.BsonDocument bd = BH.Engine.Serialiser.Convert.ToBson(db); //Break the reference clone issue

                BH.oM.XML.Environment.DocumentBuilder dbBroken = (BH.oM.XML.Environment.DocumentBuilder)BH.Engine.Serialiser.Convert.FromBson(bd);
                SerializeLevels(dbBroken.Levels, dbBroken.ElementsAsSpaces, gbx, settings);
                SerializeCollection(dbBroken.ElementsAsSpaces, dbBroken.Levels, dbBroken.UnassignedPanels, gbx, settings);
                SerializeCollection(dbBroken.ShadingElements, gbx, settings);
                SerializeCollection(dbBroken.UnassignedPanels, gbx, settings); //Serialise unassigned panels as shading as an interim measure
            }
        }

        private static void SerializeBuildingShell(IEnumerable<BH.oM.XML.Environment.DocumentBuilder> documents, GBXML gbx, XMLSettings settings)
        {
            foreach (BH.oM.XML.Environment.DocumentBuilder db in documents)
            {
                SerializeLevels(db.Levels, db.ElementsAsSpaces, gbx, settings);
                SerializeCollection(db.ElementsAsSpaces.ExternalElements(), db.Levels, db.UnassignedPanels, gbx, settings);
                SerializeCollection(db.ShadingElements, gbx, settings);
            }
        }

        private static void SerializeBuildingSpaces(IEnumerable<BH.oM.XML.Environment.DocumentBuilder> documents, XMLFileSettings fileSettings, XMLSettings settings)
        {
            if (!System.IO.Directory.Exists(fileSettings.Directory))
                System.IO.Directory.CreateDirectory(fileSettings.Directory);

            foreach (BH.oM.XML.Environment.DocumentBuilder db in documents)
            {
                foreach (List<Panel> space in db.ElementsAsSpaces)
                {
                    if (space.IsExternal(db.ElementsAsSpaces))
                    {
                        string spaceName = "Space-" + space.ConnectedSpaceName();

                        GBXML gbx = new GBXML();
                        SerializeCollection(space, db.Levels, db.UnassignedPanels, gbx, settings);
                        Level level = space.Level(db.Levels);
                        if (level != null)
                            SerializeLevels(new List<Level> { level }, new List<List<Panel>>{ space }, gbx, settings);

                        //Document History
                        DocumentHistory documentHistory = new DocumentHistory();
                        documentHistory.CreatedBy.Date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                        gbx.DocumentHistory = documentHistory;

                        XMLWriter.Save(System.IO.Path.Combine(fileSettings.Directory, fileSettings.FileName + " " + spaceName + ".xml"), gbx);
                    }
                }
            }
        }
    }

}