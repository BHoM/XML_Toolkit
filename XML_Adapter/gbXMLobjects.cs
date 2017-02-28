using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XML_Adapter;
using BHoM.Base;
using System.Xml.Serialization;


namespace XML_Adapter
{
    public abstract class gbXMLObject
    {
    }
    [Serializable]
    public class gbXML : gbXMLObject
    {
        // Default attributes for gbXML file.

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
        [XmlAttribute]
        public string schemaLocation = "gbXML http://www.gbxml.org/schema";
        [XmlAttribute]
        public string xmlns = "http://www.gbxml.org/schema";

        public DocumentHistory DocumentHistory = new DocumentHistory();
        [XmlElement("Zone")]
        public List<Zone> Zone = new List<Zone> { new Zone() };
        [XmlElement("Material")]
        public List<Material> Material = new List<Material> { new Material() };
        [XmlElement("Layer")]
        public List<Layer> Layer = new List<Layer> { new Layer() };
        [XmlElement("Construction")]
        public List<Construction> Construction = new List<Construction> { new Construction() };
        public Campus Campus = new Campus();
        public List<string> input
        {
            get;
            set;
        }
    }

    // DocumentHistory Objects
    [Serializable]
    public class DocumentHistory : gbXMLObject
    {
        public ProgramInfo ProgramInfo = new ProgramInfo();
    }

    [Serializable]
    public class ProgramInfo : gbXMLObject
    {
        [XmlAttribute]
        public string id = "BHoMgbXML";
        public string CompanyName = "BuroHappold Engineering";
        public string ProductName = "BHoMgbXML";
        public string Version = "0.0.1";
        public string Platform = "Microsoft Windows";
    }

    // Zone Objects
    [Serializable]
    public class Zone : gbXMLObject
    {
        [XmlAttribute]
        public string id = "ZoneID";
        public string Name = "Zone";
    }

    // Material Objects
    [Serializable]
    public class Material : gbXMLObject
    {
        [XmlAttribute]
        public string id = "MaterialID";
        public string Name = "Material";
        public string Thickness = "0";
    }

    // Layer Objects

    [Serializable]
    public class Layer : gbXMLObject
    {
        [XmlAttribute]
        public string id = "LayerID";
        public string Name = "Layer";
        public MaterialId MaterialId = new MaterialId();
    }
    [Serializable]
    public class MaterialId : gbXMLObject
    {
        [XmlAttribute]
        public string materialIdRef = "MaterialID";
    }

    // Construction Objects

    [Serializable]
    public class Construction : gbXMLObject
    {
        [XmlAttribute]
        public string id = "ConstructionID";
        public string Name = "Construction";
        public LayerId LayerId = new LayerId();
    }
    [Serializable]
    public class LayerId : gbXMLObject
    {
        [XmlAttribute]
        public string layerIdRef = "LayerID";
    }

    // Campus Objects
    [Serializable]
    public class Campus : gbXMLObject
    {
        [XmlAttribute]
        public string id = "CampusID";
        public string Name = "Campus";
        public Location Location = new Location();
        [XmlElement("Building")]
        public List<Building> Building = new List<Building> { new Building() };
        [XmlElement("Surface")]
        public List<Surface> Surface = new List<Surface> { new Surface() };
    }
    [Serializable]
    public class Location : gbXMLObject
    {
        public string Name = "Location";
        public float Longitude = 0;
        public float Latitude = 0;
        public float Elevation = 0;
        public float CADModelAzimuth = 0;
    }
    [Serializable]
    public class Building : gbXMLObject
    {
        [XmlAttribute]
        public string id = "BuildingID";
        [XmlAttribute]
        public string buildingType = "Unknown";
        public string Name = "Building";
        [XmlElement("BuildingStorey")]
        public List<BuildingStorey> BuildingStorey = new List<BuildingStorey> { new BuildingStorey() };
        [XmlElement("Space")]
        public List<Space> Space = new List<Space> { new Space() };
        public float Area = 0;
    }
    [Serializable]
    public class BuildingStorey : gbXMLObject
    {
        [XmlAttribute]
        public string id = "StoreyID";
        public string Name = "Storey";
        public float Level = 0;
    }
    [Serializable]
    public class Space : gbXMLObject
    {
        [XmlAttribute]
        public string id = "SpaceID";
        [XmlAttribute]
        public string zoneIdRef = "ZoneID";
        [XmlAttribute]
        public string buildingStoreyIdRef = "StoreyID";
        public string Name = "Space";
        public ShellGeometry ShellGeometry = new ShellGeometry();
    }
    [Serializable]
    public class ShellGeometry : gbXMLObject
    {
        [XmlAttribute]
        public string id = "ShellGeometryID";
        public string Name = "ShellGeometry";
        public ClosedShell ClosedShell = new ClosedShell();

    }
    [Serializable]
    public class ClosedShell : gbXMLObject
    {
        [XmlElement("Polyloop")]
        public List<Polyloop> Polyloop = new List<Polyloop> { new Polyloop() };
    }
    [Serializable]
    public class Polyloop : gbXMLObject
    {
        [XmlElement("CartesianPoint")]
        public List<CartesianPoint> CartesianPoint = new List<CartesianPoint> { new CartesianPoint() };
    }
    [Serializable]
    public class CartesianPoint : gbXMLObject
    {
        [XmlElement("Coordinate")]
        public List<float> Coordinate = new List<float> { 0 };
    }
    [Serializable]
    public class Surface : gbXMLObject
    {
        [XmlAttribute]
        public string id = "SurfaceID";
        [XmlAttribute]
        public string constructionIdRef = "ConstructionID";
        [XmlAttribute]
        public string surfaceType = "Unknown";
        public string Name = "Surface";
        [XmlElement("AdjacentSpaceId")]
        public List<AdjacentSpaceId> AdjacentSpaceId = new List<AdjacentSpaceId> { new AdjacentSpaceId() };
        public PlanarGeometry PlanarGeometry = new PlanarGeometry();
    }
    [Serializable]
    public class AdjacentSpaceId : gbXMLObject
    {
        [XmlAttribute]
        public string spaceIdRef = "SpaceID";
    }
    [Serializable]
    public class PlanarGeometry : gbXMLObject
    {
        [XmlAttribute]
        public string id = "PlanarGeometryID";
        public Polyloop Polyloop = new Polyloop();
    }
}
