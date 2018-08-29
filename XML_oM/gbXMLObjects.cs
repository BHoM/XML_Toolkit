using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using BH.oM.Base;

namespace BH.oM.XML
{
    public abstract class GBXMLObject : IObject
    {
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class GBXML : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; } = "GBXML http://www.GBXML.org/schema";
        [XmlAttribute(AttributeName = "temperatureUnit")]
        public string TemperatureUnit { get; set; } = "C";
        [XmlAttribute(AttributeName = "lengthUnit")]
        public string LengthUnit { get; set; } = "Meters";
        [XmlAttribute(AttributeName = "areaUnit")]
        public string AreaUnit { get; set; } = "SquareMeters";
        [XmlAttribute(AttributeName = "volumeUnit")]
        public string VolumeUnit { get; set; } = "CubicMeters";
        [XmlAttribute(AttributeName = "useSIUnitsForResults")]
        public string UseSIUnitsForResults { get; set; } = "true";
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; } = "0.37";
        [XmlElement("Campus")]
        public Campus Campus { get; set; } = new Campus();
        [XmlElement("Construction")]
        public Construction[] Construction { get; set; }
        [XmlElement("Layer")]
        public Layer[] Layer { get; set; }
        [XmlElement("Material")]
        public Material[] Material { get; set; }
        [XmlElement("Zone")]
        public Zone[] Zone { get; set; } = new List<Zone> { new Zone() }.ToArray();
        [XmlElement("DocumentHistory")]
        public DocumentHistory DocumentHistory { get; set; } = new DocumentHistory();
    }

    /***************************************************/
    /**** Document History Objects                  ****/
    /***************************************************/

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class DocumentHistory : GBXMLObject, IObject
    {
        [XmlElement("ProgramInfo")]
        public ProgramInfo ProgramInfo { get; set; } = new ProgramInfo();
        [XmlElement("CreatedBy")]
        public CreatedBy CreatedBy { get; set; } = new CreatedBy();
        [XmlElement("PersonInfo")]
        public PersonInfo PersonInfo { get; set; } = new PersonInfo();
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class ProgramInfo : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "BHoMGBXML";
        [XmlElement("CompanyName")]
        public string CompanyName { get; set; } = "BuroHappold Engineering";
        [XmlElement("ProductName")]
        public string ProductName { get; set; } = "Autodesk Revit 2018 BEES";
        [XmlElement("Version")]
        public string Version { get; set; } = "2018 20170223_1515(x64)";
        [XmlElement("Platform")]
        public string Platform { get; set; } = "Microsoft Windows";
        [XmlElement("ProjectEntity")]
        public ProjectEntity ProjectEntity { get; set; } = new ProjectEntity();
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class CreatedBy : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "personId")]
        public string PersonID { get; set; } = "BuroHappold";
        [XmlAttribute(AttributeName = "programId")]
        public string ProgramID { get; set; } = "BHoMGBXML";
        [XmlAttribute(AttributeName = "date")]
        public string Date { get; set; } = "00-00-00";
        [XmlElement("CADModelId")]
        public string CADModelID { get; set; } = "Unknown";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class PersonInfo : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "BuroHappold";
    }

    /***************************************************/
    /**** Zone Objects                              ****/
    /***************************************************/

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Zone : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "ZoneID";
        [XmlElement("AirChangesperHour")]
        public double AirChangesPerHour { get; set; } = 0;
        [XmlElement("OAFlowPerArea")]
        public OAFlowPerArea OAFlowPerArea { get; set; } = new OAFlowPerArea();
        [XmlElement("OAFlowPerPerson")]
        public OAFlowPerPerson OAFlowPerPerson { get; set; } = new OAFlowPerPerson();
        [XmlElement("DesignHeatT")]
        public DesignHeatT DesignHeatT { get; set; } = new DesignHeatT();
        [XmlElement("DesignCoolT")]
        public DesignCoolT DesignCoolT { get; set; } = new DesignCoolT();
        [XmlElement("TypeCode")]
        public double TypeCode { get; set; } = 0;
        [XmlElement("Name")]
        public string Name { get; set; } = "Zone";
        [XmlElement("CADObjectId")]
        public string CADObjectID { get; set; } = "Unknown";
    }

    /***************************************************/
    /**** Material Objects                          ****/
    /***************************************************/

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Material : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "MaterialID";
        [XmlElement("Name")]
        public string Name { get; set; } = "Material";
        [XmlElement("R-value")]
        public double RValue { get; set; } = 0;
        [XmlElement("Thickness")]
        public double Thickness { get; set; } = 0.001;
        [XmlElement("Conductivity")]
        public double Conductivity { get; set; } = 0;
        [XmlElement("Density")]
        public double Density { get; set; } = 0;
        [XmlElement("SpecificHeat")]
        public double SpecificHeat { get; set; } = 0;
    }

    /***************************************************/
    /**** Layer Objects                             ****/
    /***************************************************/

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Layer : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "LayerID";
        [XmlElement("MaterialId")]
        public MaterialId MaterialID { get; set; } = new MaterialId();
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class MaterialId : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "materialIdRef")]
        public string MaterialIDRef { get; set; } = "MaterialID";
    }

    /***************************************************/
    /**** Construction Objects                      ****/
    /***************************************************/

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Construction : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "ConstructionID";
        [XmlElement("U-value")]
        public UValue UValue { get; set; } = new UValue();
        [XmlElement("Absorptance")]
        public Absorptance Absorptance { get; set; } = new Absorptance();
        [XmlElement("Roughness")]
        public Roughness Roughness { get; set; } = new Roughness();
        [XmlElement("LayerId")]
        public LayerId LayerID { get; set; } = new LayerId();
        [XmlElement("Name")]
        public string Name { get; set; } = "Construction";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class LayerId : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "layerIdRef")]
        public string LayerIDRef { get; set; } = "LayerID";
    }

    /***************************************************/
    /**** Campus Objects                            ****/
    /***************************************************/

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Campus : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "CampusID";
        [XmlElement("Location")]
        public Location Location { get; set; } = new Location();
        [XmlElement("Building")]
        public Building[] Building { get; set; } = new List<Building> { new Building() }.ToArray();
        [XmlElement("Surface")]
        public List<Surface> Surface { get; set; } = new List<Surface>();
        [XmlElement("DaylightSavings")]
        public string DaylightSavings { get; set; } = "false";
        [XmlElement("Name")]
        public string Name { get; set; } = "Campus";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Location : GBXMLObject, IObject
    {
        [XmlElement("StationId")]
        public StationId StationID { get; set; } = new StationId();
        [XmlElement("ZipcodeOrPostalCode")]
        public string ZipcodeOrPostalCode { get; set; } = "";
        [XmlElement("Longitude")]
        public double Longitude { get; set; } = 0;
        [XmlElement("Latitude")]
        public double Latitude { get; set; } = 0;
        [XmlElement("Elevation")]
        public double Elevation { get; set; } = 0;
        [XmlElement("CADModelAzimuth")]
        public double CADModelAzimuth { get; set; } = 0;
        [XmlElement("Name")]
        public string Name { get; set; } = "Location";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Building : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "buildingType")]
        public string BuildingType { get; set; } = "Unknown";
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "BuildingID";
        [XmlElement("StreetAddress")]
        public string StreetAddress { get; set; } = "Unknown";
        [XmlElement("Area")]
        public float Area { get; set; } = 0;
        [XmlElement("Space")]
        public List<Space> Space { get; set; } = new List<Space>();
        [XmlElement("BuildingStorey")]
        public BuildingStorey[] BuildingStorey { get; set; } = new List<BuildingStorey> { new BuildingStorey() }.ToArray();
        [XmlElement("Name")]
        public string Name { get; set; } = "Building";
        [XmlElement("Description")]
        public string Description { get; set; } = "None";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class BuildingStorey : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "StoreyID";
        [XmlElement("Level")]
        public float Level { get; set; } = 0;
        [XmlElement("PlanarGeometry")]
        public PlanarGeometry PlanarGeometry { get; set; } = new PlanarGeometry();
        [XmlElement("Name")]
        public string Name { get; set; } = "Storey";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Space : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "zoneIdRef")]
        public string ZoneIDRef { get; set; } = "ZoneID";
        [XmlAttribute(AttributeName = "conditionType")]
        public string ConditionType { get; set; } = "Unknown";
        [XmlAttribute(AttributeName = "buildingStoryIdRef")]
        public string BuildingStoreyIDRef { get; set; } = "StoreyID";
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "SpaceID";
        [XmlElement("Area")]
        public double Area { get; set; } = 0;
        [XmlElement("Volume")]
        public double Volume { get; set; } = 0;
        [XmlElement("PlanarGeometry")]
        public PlanarGeometry PlanarGeoemtry { get; set; } = new PlanarGeometry();
        [XmlElement("ShellGeometry")]
        public ShellGeometry ShellGeometry { get; set; } = new ShellGeometry();
        [XmlElement("SpaceBoundary")]
        public SpaceBoundary[] SpaceBoundary { get; set; } = new List<SpaceBoundary> { new SpaceBoundary() }.ToArray();
        [XmlElement("Name")]
        public string Name { get; set; } = "Space";
        [XmlElement("Description")]
        public string Description { get; set; } = "None";
        [XmlElement("CADObjectId")]
        public string CADObjectID { get; set; } = "xxxxxx";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class ShellGeometry : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "ShellGeometryID";
        [XmlElement("ClosedShell")]
        public ClosedShell ClosedShell { get; set; } = new ClosedShell();
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class ClosedShell : GBXMLObject, IObject
    {
        [XmlElement("PolyLoop")]
        public Polyloop[] PolyLoop { get; set; } = new List<Polyloop> { new Polyloop() }.ToArray();
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Polyloop : GBXMLObject, IObject
    {
        [XmlElement("CartesianPoint")]
        public CartesianPoint[] CartesianPoint { get; set; } = new List<CartesianPoint> { new CartesianPoint() }.ToArray();
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class CartesianPoint : GBXMLObject, IObject
    {
        [XmlElement("Coordinate")]
        public string[] Coordinate { get; set; } = new List<string> { "0" }.ToArray();
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Surface : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "surfaceType")]
        public string SurfaceType { get; set; } = "Unknown";
        [XmlAttribute(AttributeName = "exposedToSun")]
        public string ExposedToSun { get; set; } = "false";
        [XmlAttribute(AttributeName = "constructionIdRef")]
        public string ConstructionIDRef { get; set; } = "";
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "SurfaceID";
        [XmlElement("AdjacentSpaceId")]
        public AdjacentSpaceId[] AdjacentSpaceID { get; set; }
        [XmlElement("RectangularGeometry")]
        public RectangularGeometry RectangularGeometry { get; set; } = new RectangularGeometry();
        [XmlElement("PlanarGeometry")]
        public PlanarGeometry PlanarGeometry { get; set; } = new PlanarGeometry();
        [XmlElement("Opening")]
        public Opening[] Opening { get; set; }
        [XmlElement("CADObjectId")]
        public string CADObjectID { get; set; } = "";
        [XmlElement("Name")]
        public string Name { get; set; } = "Surface";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class AdjacentSpaceId : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "spaceIdRef")]
        public string SpaceIDRef { get; set; } = "AdjacentSpaceID";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class PlanarGeometry : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "PlanarGeometryID";
        [XmlElement("PolyLoop")]
        public Polyloop PolyLoop { get; set; } = new Polyloop();
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class RectangularGeometry : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "RectangularGeometryID";
        [XmlElement("Azimuth")]
        public double Azimuth { get; set; }
        [XmlElement("CartesianPoint")]
        public CartesianPoint CartesianPoint { get; set; } = new CartesianPoint();
        [XmlElement("Tilt")]
        public double Tilt { get; set; }
        [XmlElement("Width")]
        public double Width { get; set; }
        [XmlElement("Height")]
        public double Height { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class RectangularGeometryOpenings : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "RectangularGeometryID";
        [XmlElement("CartesianPoint")]
        public CartesianPoint CartesianPoint { get; set; } = new CartesianPoint();
        [XmlElement("Width")]
        public double Width { get; set; }
        [XmlElement("Height")]
        public double Height { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Opening : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "constructionIdRef")]
        public string ConstructionIDRef { get; set; } = "";
        [XmlAttribute(AttributeName = "openingType")]
        public string OpeningType { get; set; } = "FixedWindow";
        [XmlAttribute(AttributeName = "id")]
        public string ID { get; set; } = "OpeningID";
        [XmlElement("RectangularGeometry")]
        public RectangularGeometryOpenings RectangularGeometry { get; set; } = new RectangularGeometryOpenings();
        [XmlElement("PlanarGeometry")]
        public PlanarGeometry PlanarGeometry { get; set; } = new PlanarGeometry();
        [XmlElement("CADObjectId")]
        public string CADObjectID { get; set; } = "WinInst: SIM_EXT_GLZ [xxxxxx]";
        [XmlElement("Name")]
        public string Name { get; set; } = "Opening";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class SpaceBoundary : GBXMLObject, IObject
    {
        [XmlAttribute(AttributeName = "isSecondLevelBoundary")]
        public string IsSecondLevelBoundary { get; set; } = "false";
        [XmlAttribute(AttributeName = "surfaceIdRef")]
        public string SurfaceIDRef { get; set; } = "";
        [XmlElement("PlanarGeometry")]
        public PlanarGeometry PlanarGeometry { get; set; } = new PlanarGeometry();
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class ProjectEntity : GBXMLObject, IObject
    {
        [XmlElement("URI")]
        public string URI { get; set; } = "Unknown";
        [XmlElement("GUID")]
        public string GUID { get; set; } = "Unknown";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class StationId : GBXMLObject, IObject
    {
        [XmlAttribute("IDType")]
        public string IDType { get; set; } = "Unknown";
        [XmlAttribute("ID")]
        public string ID { get; set; } = "";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class OAFlowPerArea : GBXMLObject, IObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "LPerSecPerSquareM";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class OAFlowPerPerson : GBXMLObject, IObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "LPerSec";
    }

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class DesignHeatT : GBXMLObject, IObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "C";
    }

    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class DesignCoolT : GBXMLObject, IObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "C";
    }

    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class UValue : GBXMLObject, IObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "WPerSquareMeterK";
    }

    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.GBXML.org/schema")]
    public class Absorptance : GBXMLObject, IObject
    {
        [XmlAttribute("unit")]
        public string Unit { get; set; } = "Fraction";
        [XmlAttribute("type")]
        public string Type { get; set; } = "ExtIR";
    }

    public class Roughness : GBXMLObject, IObject
    {
        [XmlAttribute("value")]
        public string Value { get; set; } = "VeryRough"; //TODO: what should the default value be?
    }
}
