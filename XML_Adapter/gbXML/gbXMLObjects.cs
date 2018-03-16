using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XML_Adapter.gbXML;
using BH.oM.Base;
using System.Xml.Serialization;


namespace XML_Adapter.gbXML
{
    public abstract class gbXMLObject
    {
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]

    public class gbXML : gbXMLObject
    {
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string schemaLocation = "gbXML http://www.gbxml.org/schema";
        [XmlAttribute]
        public string temperatureUnit = "C";
        [XmlAttribute]
        public string lengthUnit = "Meters";
        [XmlAttribute]
        public string areaUnit = "SquareMeters";
        [XmlAttribute]
        public string volumeUnit = "CubicMeters";
        [XmlAttribute]
        public string useSIUnitsForResults = "true";
        [XmlAttribute]
        public string version = "0.37";
        [XmlElement("DocumentHistory")]
        public DocumentHistory DocumentHistory = new DocumentHistory();
        [XmlElement("Zone")]
        public Zone[] Zone = new List<Zone> { new Zone() }.ToArray();
        [XmlElement("Material")]
        public Material[] Material = new List<Material> { new Material() }.ToArray();
        [XmlElement("Layer")]
        public Layer[] Layer = new List<Layer> { new Layer() }.ToArray();
        [XmlElement("Construction")]
        public Construction[] Construction = new List<Construction> { new Construction() }.ToArray();
        [XmlElement("Campus")]
        public Campus Campus = new Campus();
    }

    // DocumentHistory Objects
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class DocumentHistory : gbXMLObject
    {
        [XmlElement("ProgramInfo")]
        public ProgramInfo ProgramInfo = new ProgramInfo();
        [XmlElement("CreatedBy")]
        public CreatedBy CreatedBy = new CreatedBy();
        [XmlElement("PersonInfo")]
        public PersonInfo PersonInfo = new PersonInfo();
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class ProgramInfo : gbXMLObject
    {
        [XmlAttribute]
        public string id = "BHoMgbXML";
        [XmlElement("CompanyName")]
        public string CompanyName = "BuroHappold Engineering";
        [XmlElement("ProductName")]
        public string ProductName = "BHoMgbXML";
        [XmlElement("Version")]
        public string Version = "0.0.1";
        [XmlElement("Platform")]
        public string Platform = "Microsoft Windows";
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class CreatedBy : gbXMLObject
    {
        [XmlAttribute]
        public string personId = "BuroHappold";
        [XmlAttribute]
        public string programId = "BHoMgbXML";
        [XmlAttribute]
        public string date = "00-00-00";
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class PersonInfo : gbXMLObject
    {
        [XmlAttribute]
        public string id = "BuroHappold";
    }

    // Zone Objects
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Zone : gbXMLObject
    {
        [XmlAttribute]
        public string id = "ZoneID";
        [XmlElement]
        public string Name = "Zone";
    }

    // Material Objects
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Material : gbXMLObject
    {
        [XmlAttribute]
        public string id = "MaterialID";
        [XmlElement]
        public string Name = "Material";
        [XmlElement]
        public double Thickness = 0.001;
    }

    // Layer Objects

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Layer : gbXMLObject
    {
        [XmlAttribute]
        public string id = "LayerID";
        [XmlElement]
        public string Name = "Layer";
        [XmlElement]
        public MaterialId MaterialId = new MaterialId();
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class MaterialId : gbXMLObject
    {
        [XmlAttribute]
        public string materialIdRef = "MaterialID";
    }

    // Construction Objects

    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Construction : gbXMLObject
    {
        [XmlAttribute]
        public string id = "ConstructionID";
        [XmlElement]
        public string Name = "Construction";
        [XmlElement]
        public LayerId LayerId = new LayerId();
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class LayerId : gbXMLObject
    {
        [XmlAttribute]
        public string layerIdRef = "LayerID";
    }

    // Campus Objects
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Campus : gbXMLObject
    {
        [XmlAttribute]
        public string id = "CampusID";
        [XmlElement]
        public string Name = "Campus";
        [XmlElement]
        public Location Location = new Location();
        [XmlElement("Building")]
        public Building[] Building = new List<Building> { new Building() }.ToArray();
        [XmlElement("Surface")]
        //public Surface[] Surface = new List<Surface> { new Surface() }.ToArray();
        public List<Surface> Surface = new List<Surface>();
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Location : gbXMLObject
    {
        [XmlElement]
        public string Name = "Location";
        [XmlElement]
        public double Longitude = 0;
        [XmlElement]
        public double Latitude = 0;
        [XmlElement]
        public double Elevation = 0;
        [XmlElement]
        public double CADModelAzimuth = 0;
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Building : gbXMLObject
    {
        [XmlAttribute]
        public string id = "BuildingID";
        [XmlAttribute]
        public string buildingType = "Unknown";
        [XmlElement]
        public string Name = "Building";
        [XmlElement("BuildingStorey")]
        public BuildingStorey[] BuildingStorey = new List<BuildingStorey> { new BuildingStorey() }.ToArray();
        [XmlElement("Space")]
        //public Space[] Space = new List<Space> { new XML_Adapter.gbXML.Space() }.ToArray() ;
        public List<Space> Space = new List<Space>();
        [XmlElement]
        public float Area = 0;
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class BuildingStorey : gbXMLObject
    {
        [XmlAttribute]
        public string id = "StoreyID";
        [XmlElement]
        public string Name = "Storey";
        [XmlElement]
        public float Level = 0;
        [XmlElement]
        public PlanarGeometry PlanarGeoemtry = new PlanarGeometry();
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Space : gbXMLObject
    {
        [XmlAttribute]
        public string id = "SpaceID";
        [XmlAttribute]
        public string zoneIdRef = "ZoneID";
        [XmlAttribute]
        public string buildingStoreyIdRef = "StoreyID";
        [XmlElement]
        public string Name = "Space";
        [XmlElement]
        public double Area = 0;
        [XmlElement]
        public double Volume = 0;
        [XmlElement]
        public ShellGeometry ShellGeometry = new ShellGeometry();
        [XmlElement]
        public PlanarGeometry PlanarGeoemtry = new PlanarGeometry();
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class ShellGeometry : gbXMLObject
    {
        [XmlAttribute]
        public string id = "ShellGeometryID";
        [XmlElement]
        public ClosedShell ClosedShell = new ClosedShell();

    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class ClosedShell : gbXMLObject
    {
        [XmlElement("PolyLoop")]
        public Polyloop[] PolyLoop = new List<Polyloop> {new Polyloop()}.ToArray();
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Polyloop : gbXMLObject
    {
        [XmlElement("CartesianPoint")]
        public CartesianPoint[] CartesianPoint = new List<CartesianPoint> {new CartesianPoint() }.ToArray();
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class CartesianPoint : gbXMLObject
    {
        [XmlElement("Coordinate")]
        public string[] Coordinate = new List<string> { "0" }.ToArray();
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Surface : gbXMLObject
    {
        [XmlAttribute]
        public string id = "SurfaceID";
        [XmlAttribute]
        public string constructionIdRef = "ConstructionID";
        [XmlAttribute]
        public string surfaceType = "Unknown";
        [XmlElement("Name")]
        public string Name = "Surface";
        [XmlElement("AdjacentSpaceId")]
        public AdjacentSpaceId[] AdjacentSpaceId = new List<AdjacentSpaceId> { new AdjacentSpaceId() }.ToArray();
        [XmlElement("PlanarGeometry")]
        public PlanarGeometry PlanarGeometry = new PlanarGeometry();
        [XmlElement("RectangularGeometry")]
        public RectangularGeometry RectangularGeometry = new RectangularGeometry();
        [XmlElement("Opening")]
        public Opening[] Opening;
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class AdjacentSpaceId : gbXMLObject
    {
        [XmlAttribute]
        public string spaceIdRef = "SpaceID";
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class PlanarGeometry : gbXMLObject
    {
        [XmlAttribute]
        public string id = "PlanarGeometryID";
        [XmlElement("PolyLoop")]
        public Polyloop PolyLoop = new Polyloop();
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class RectangularGeometry : gbXMLObject
    {
        [XmlAttribute]
        public string id = "RectangularGeometryID";
        [XmlAttribute]
        public string unit = "UnitID";
        [XmlElement]
        public double Azimuth = 0;
        [XmlElement("Tilt")]
        public double Tilt = 0;
        [XmlElement("Width")]
        public double Width = 0;
        [XmlElement("Height")]
        public double Height = 0;
        [XmlElement("CartesianPoint")]
        public CartesianPoint CartesianPoint = new CartesianPoint();
        [XmlElement("PolyLoop")]
        public Polyloop Polyloop = new Polyloop();
    }
    [Serializable]
    [XmlRoot(ElementName = "gbXML", IsNullable = false, Namespace = "http://www.gbxml.org/schema")]
    public class Opening : gbXMLObject
    {
        [XmlAttribute]
        public string openingType = "unknown";
        [XmlAttribute]
        public string id = "OpeningID";
        [XmlElement("Name")]
        public string Name = "Opening";
        //[XmlElement]
        //public RectangularGeometry RectangularGeometry = new RectangularGeometry();
        [XmlElement]
        public PlanarGeometry PlanarGeometry = new PlanarGeometry();
    }
}
