using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ComponentModel;

namespace AntMe
{
    /// <summary>
    /// Represents the topological information about a level Map.
    /// </summary>
    public sealed class Map : PropertyList<MapProperty>, ISerializableState
    {
        #region Constants

        /// <summary>
        /// Size of a single Cell in World Units.
        /// </summary>
        public const float Cellsize = 50f;

        /// <summary>
        /// Minimum Number of Cells in X Direction.
        /// </summary>
        public const int MinWidth = 10;

        /// <summary>
        /// Minimum Number of Cells in Y Direction.
        /// </summary>
        public const int MinHeight = 10;

        /// <summary>
        /// Maximum Number of Cells in X Direction.
        /// </summary>
        public const int MaxWidth = 100;

        /// <summary>
        /// Maximum Number of Cells in Y Direction.
        /// </summary>
        public const int MaxHeight = 100;

        /// <summary>
        /// Minimum Count of Start Points.
        /// </summary>
        public const int MinStartpoints = 1;

        /// <summary>
        /// Maximum Count of Start Points.
        /// </summary>
        public const int MaxStartpoints = Level.MaxSlots;

        /// <summary>
        /// Minimum Hight of Units.
        /// </summary>
        public const float MinZ = 0f;

        /// <summary>
        /// Maximum Hight of Units.
        /// </summary>
        public const float MaxZ = 50f;

        /// <summary>
        /// Height of a single Height Level.
        /// </summary>
        public const float Levelheight = 20f;

        /// <summary>
        /// Maximum Height Level.
        /// </summary>
        public const byte MaxLevel = 2;

        #endregion

        private MapState _state;

        private readonly SimulationContext _context;

        /// <summary>
        /// Holds all Map Tiles.
        /// </summary>
        private readonly MapTile[,] _tiles;

        /// <summary>
        /// Gets or sets the border behavior of this Map.
        /// </summary>
        [DisplayName("Block Border")]
        [Description("Gets or sets the border behavior of this Map.")]
        public bool BlockBorder { get; set; }

        /// <summary>
        /// Gets or sets the base Height Level for this Map.
        /// </summary>
        [DisplayName("Base Height Level")]
        [Description("Gets or sets the base Height Level for this Map.")]
        public byte BaseLevel { get; set; }

        /// <summary>
        /// Dictionary of missing Properties during Deserialization.
        /// </summary>
        [Browsable(false)]
        public Dictionary<string, byte[]> UnknownProperties { get; private set; }

        /// <summary>
        /// Gets or sets the Map Tiles.
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <returns>Map Tile</returns>
        public MapTile this[int x, int y]
        {
            get => _tiles[x, y];
            set => _tiles[x, y] = value;
        }

        /// <summary>
        /// List of Start Point for the available Player Slots.
        /// </summary>
        [Browsable(false)]
        public Index2[] StartPoints { get; set; }

        /// <summary>
        /// Default Constructor to initialize the Tile Array.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="width">Number of Cols</param>
        /// <param name="height">Number of Rows</param>
        public Map(SimulationContext context, int width, int height) : this(context, width, height, 0) { }

        /// <summary>
        /// Constructor with predefined Player Startpoints.
        /// </summary>
        /// <param name="context">Simulation Context</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="playerCount"></param>
        public Map(SimulationContext context, int width, int height, int playerCount)
        {
            _context = context;

            UnknownProperties = new Dictionary<string, byte[]>();

            // Check Parameter
            if (width < MinWidth)
                throw new ArgumentOutOfRangeException(nameof(width), $"Map must have at least {MinWidth} Columns");
            if (width > MaxWidth)
                throw new ArgumentOutOfRangeException(nameof(width), $"Map must have a max of {MaxWidth} Columns");
            if (height < MinHeight)
                throw new ArgumentOutOfRangeException(nameof(height), $"Map must have at least {MinHeight} Rows");
            if (height > MaxHeight)
                throw new ArgumentOutOfRangeException(nameof(height), $"Map must have a max of {MaxHeight} Rows");
            if (playerCount < 0)
                throw new ArgumentOutOfRangeException(nameof(playerCount), "Player Count can't me smaller than zero");
            if (playerCount > Level.MaxSlots)
                throw new ArgumentOutOfRangeException(nameof(playerCount),
                    $"Player Count can't be greater than {Level.MaxSlots}");

            // Create Tiles
            _tiles = new MapTile[width, height];

            // Create Players
            var dx = width / 6;
            var dy = height / 6;
            StartPoints = new Index2[playerCount];
            if (playerCount >= 1)
                StartPoints[0] = new Index2(dx, dy);
            if (playerCount >= 2)
                StartPoints[1] = new Index2(5 * dx, 5 * dy);
            if (playerCount >= 3)
                StartPoints[2] = new Index2(5 * dx, dy);
            if (playerCount >= 4)
                StartPoints[3] = new Index2(dx, 5 * dy);
            if (playerCount >= 5)
                StartPoints[4] = new Index2(3 * dx, dy);
            if (playerCount >= 6)
                StartPoints[5] = new Index2(3 * dx, 5 * dy);
            if (playerCount >= 7)
                StartPoints[6] = new Index2(dx, 3 * dy);
            if (playerCount >= 8)
                StartPoints[7] = new Index2(5 * dx, 3 * dy);

            _context.Resolver.ResolveMap(context, this);
        }

        /// <summary>
        /// Updates the Map and its Properties.
        /// </summary>
        /// <param name="round">Current Round</param>
        public void Update(int round)
        {
            foreach (var property in Properties)
                property.Update(round);
        }

        /// <summary>
        /// Creates a Map State.
        /// </summary>
        /// <returns>Map State</returns>
        public MapState GetState()
        {
            // On first Call create State
            if (_state == null)
                _state = _context.Resolver.CreateMapState(this);

            // Update Tile States
            Index2 size = GetCellCount();
            for (int y = 0; y < size.Y; y++)
                for (int x = 0; x < size.X; x++)
                    _state.Tiles[x, y] = this[x, y].GetState();

            return _state;
        }

        /// <summary>
        ///     Führt eine Plausibilitätsprüfung der Karten-Einstellungen durch
        /// </summary>
        public void ValidateMap()
        {
            var exceptions = new List<Exception>();

            // Validate Tiles Stuff
            if (_tiles != null)
            {
                Index2 cells = GetCellCount();

                // Check Map Dimensions
                if (cells.X < MinWidth)
                    exceptions.Add(new InvalidMapException($"Map must have at least {MinWidth} Columns"));
                if (cells.X > MaxWidth)
                    exceptions.Add(new InvalidMapException($"Map must have a maximum of {MaxWidth} Columns"));
                if (cells.Y < MinHeight)
                    exceptions.Add(new InvalidMapException($"Map must have at least {MinHeight} Rows"));
                if (cells.Y > MaxHeight)
                    exceptions.Add(new InvalidMapException($"Map must have a maximum of {MaxHeight} Rows"));

                // Check Individual Cells
                var result = new List<Exception>();
                for (var y = 0; y < cells.Y; y++)
                {
                    for (var x = 0; x < cells.X; x++)
                    {
                        var tile = _tiles[x, y];

                        // Check if Map Tile exists
                        if (tile == null)
                        {
                            exceptions.Add(new InvalidMapTileException(new Index2(x, y), "Map Tile is missing"));
                            continue;
                        }

                        // Check if Material exists
                        if (tile.Material == null)
                        {
                            exceptions.Add(new InvalidMapTileException(new Index2(x, y), "Material is missing"));
                        }

                        // Collect neighbours
                        var northTile = (y <= 0 ? null : _tiles[x, y - 1]);
                        var southTile = (y >= cells.Y - 1 ? null : _tiles[x, y + 1]);
                        var westTile = (x <= 0 ? null : _tiles[x - 1, y]);
                        var eastTile = (x >= cells.X - 1 ? null : _tiles[x + 1, y]);

                        // Check Height Map
                        if (y <= 0)
                        {
                            if (tile.ConnectionLevelNorth != BaseLevel)
                                exceptions.Add(new InvalidMapTileException(new Index2(x, y), "Wrong Connection Level at the Border (North)"));
                        }
                        else
                        {
                            if (tile.ConnectionLevelNorth != northTile?.ConnectionLevelSouth)
                                exceptions.Add(new InvalidMapTileException(new Index2(x, y), "Wrong Connection Level to the North"));
                        }

                        if (y >= cells.Y - 1)
                        {
                            if (tile.ConnectionLevelSouth != BaseLevel)
                                exceptions.Add(new InvalidMapTileException(new Index2(x, y), "Wrong Connection Level at the Border (South)"));
                        }
                        else
                        {
                            if (tile.ConnectionLevelSouth != southTile?.ConnectionLevelNorth)
                                exceptions.Add(new InvalidMapTileException(new Index2(x, y), "Wrong Connection Level to the South"));
                        }

                        if (x <= 0)
                        {
                            if (tile.ConnectionLevelWest != BaseLevel)
                                exceptions.Add(new InvalidMapTileException(new Index2(x, y), "Wrong Connection Level at the Border (West)"));
                        }
                        else
                        {
                            if (tile.ConnectionLevelWest != westTile?.ConnectionLevelEast)
                                exceptions.Add(new InvalidMapTileException(new Index2(x, y), "Wrong Connection Level to the West"));
                        }

                        if (x >= cells.X - 1)
                        {
                            if (tile.ConnectionLevelEast != BaseLevel)
                                exceptions.Add(new InvalidMapTileException(new Index2(x, y), "Wrong Connection Level at the Border (East)"));
                        }
                        else
                        {
                            if (tile.ConnectionLevelEast != eastTile?.ConnectionLevelWest)
                                exceptions.Add(new InvalidMapTileException(new Index2(x, y), "Wrong Connection Level to the East"));
                        }

                        // Check Neighbours
                        result.Clear();
                        if (!tile.ValidateTileToTheNorth(northTile, result))
                        {
                            foreach (var ex in result)
                                exceptions.Add(new InvalidMapTileException(new Index2(x, y), "Invalid Neighbour Map Tile to the North", ex));
                        }

                        result.Clear();
                        if (!tile.ValidateTileToTheSouth(southTile, result))
                        {
                            foreach (var ex in result)
                                exceptions.Add(new InvalidMapTileException(new Index2(x, y), "Invalid Neighbour Map Tile to the South", ex));
                        }

                        result.Clear();
                        if (!tile.ValidateTileToTheWest(westTile, result))
                        {
                            foreach (var ex in result)
                                exceptions.Add(new InvalidMapTileException(new Index2(x, y), "Invalid Neighbour Map Tile to the West", ex));
                        }

                        result.Clear();
                        if (!tile.ValidateTileToTheEast(eastTile, result))
                        {
                            foreach (var ex in result)
                                exceptions.Add(new InvalidMapTileException(new Index2(x, y), "Invalid Neighbour Map Tile to the East", ex));
                        }
                    }
                }
            }
            else
            {
                exceptions.Add(new InvalidMapException("Tiles Array is null"));
            }

            // Startpunkte überprüfen
            if (StartPoints != null)
            {
                // Spieleranzahl prüfen
                if (GetPlayerCount() < MinStartpoints)
                    exceptions.Add(new InvalidMapException($"There must be at least {MinStartpoints} player"));
                if (GetPlayerCount() > MaxStartpoints)
                    exceptions.Add(new Exception($"The maximum Player Count is {MaxStartpoints}"));

                // Alle Startpunkte überprüfen
                for (int i = 0; i < StartPoints.Length; i++)
                {
                    // Prüfen, ob die Zelle existiert
                    Index2 startPoint = StartPoints[i];
                    if (startPoint.X < 0 || startPoint.X >= _tiles.GetLength(0) ||
                        startPoint.Y < 0 || startPoint.Y >= _tiles.GetLength(1))
                        exceptions.Add(new InvalidMapException($"StartPoint {i} is out of map bounds"));

                    // Prüfen, ob noch ein anderer Startpoint auf der selben Zelle ist.
                    for (int j = 0; j < StartPoints.Length; j++)
                        if (i != j && StartPoints[i] == StartPoints[j])
                            exceptions.Add(new InvalidMapException($"StartPoints {i} and {j} are on the same Cell"));
                }
            }
            else
            {
                exceptions.Add(new InvalidMapException("The List of StartPoints is null"));
            }

            // Throw aggregated Validation Exceptions
            if (exceptions.Count > 0)
                throw new AggregateException("There are several Map Validation Errors", exceptions);
        }

        /// <summary>
        /// Finds the Cell Index for a given Position.
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Cell Index</returns>
        public Index2 GetCellIndex(Vector3 position)
        {
            return GetCellIndex(position.X, position.Y);
        }

        /// <summary>
        /// Finds the Cell Index for a given Position.
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Cell Index</returns>
        public Index2 GetCellIndex(Vector2 position)
        {
            return GetCellIndex(position.X, position.Y);
        }

        /// <summary>
        /// Finds the Cell Index for a given Position.
        /// </summary>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        /// <returns>Cell Index</returns>
        public Index2 GetCellIndex(float x, float y)
        {
            // Wenns keine Map gibt
            if (_tiles == null)
                return Index2.Zero;

            Index2 limit = GetCellCount();

            var indexX = (int)(x / Cellsize);
            var indexY = (int)(y / Cellsize);

            if (indexX < 0)
                indexX = 0;
            if (indexY < 0)
                indexY = 0;
            if (indexX >= limit.X)
                indexX = limit.X - 1;
            if (indexY >= limit.Y)
                indexY = limit.Y - 1;

            return new Index2(indexX, indexY);
        }

        /// <summary>
        /// Calculates the local Postion within the current Cell.
        /// </summary>
        /// <param name="position">Global Position</param>
        /// <returns>Local Postion</returns>
        public Vector2 GetLocalPosition(Vector2 position)
        {
            return GetLocalPosition(position.X, position.Y);
        }

        /// <summary>
        /// Calculates the local Postion within the current Cell.
        /// </summary>
        /// <param name="position">Global Position</param>
        /// <returns>Local Postion</returns>
        public Vector2 GetLocalPosition(Vector3 position)
        {
            return GetLocalPosition(position.X, position.Y);
        }

        /// <summary>
        /// Calculates the local Postion within the current Cell.
        /// </summary>
        /// <param name="x">Global X Position</param>
        /// <param name="y">Global Y Position</param>
        /// <returns>Local Postion</returns>
        public Vector2 GetLocalPosition(float x, float y)
        {
            return new Vector2(x % Cellsize, y % Cellsize);
        }

        /// <summary>
        /// Returns the Count of Rows and Cols of this Map.
        /// </summary>
        /// <returns>Cell Count</returns>
        public Index2 GetCellCount()
        {
            if (_tiles != null)
                return new Index2(
                    _tiles.GetLength(0),
                    _tiles.GetLength(1));
            return Index2.Zero;
        }

        /// <summary>
        /// Caclulates the total size of the map in Game-Units.
        /// </summary>
        /// <returns>Map Dimensions</returns>
        public Vector2 GetSize()
        {
            Index2 cells = GetCellCount();
            return new Vector2(
                cells.X * Cellsize,
                cells.Y * Cellsize);
        }

        /// <summary>
        /// Returns the Number of SlotPositions.
        /// </summary>
        /// <returns>Count</returns>
        public int GetPlayerCount()
        {
            if (StartPoints != null)
                return StartPoints.Length;
            return 0;
        }

        /// <summary>
        /// Gets the Map Height at the given Position
        /// </summary>
        /// <param name="position">Global Position</param>
        /// <returns>Height</returns>
        public float GetHeight(Vector2 position)
        {
            return GetHeight(position.X, position.Y);
        }

        /// <summary>
        /// Gets the Map Height at the given Position
        /// </summary>
        /// <param name="position">Global Position</param>
        /// <returns>Height</returns>
        public float GetHeight(Vector3 position)
        {
            return GetHeight(position.X, position.Y);
        }

        /// <summary>
        /// Gets the Map Height at the given Position
        /// </summary>
        /// <param name="x">Global X Position</param>
        /// <param name="y">Global Y Position</param>
        /// <returns>Height</returns>
        public float GetHeight(float x, float y)
        {
            Index2 cell = GetCellIndex(x, y);
            Vector2 local = GetLocalPosition(x, y);
            return this[cell.X, cell.Y].GetHeight(local);
        }

        /// <summary>
        /// Serializes the Map.
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public void SerializeFirst(BinaryWriter writer, byte version)
        {
            switch (version)
            {
                case 2: SerializeV2(writer); break;
                default: throw new ArgumentException("File Version not supported");
            }
        }

        /// <summary>
        /// Serializes the Map in Stream Format 2.
        /// </summary>
        /// <param name="writer">Output Writer</param>
        private void SerializeV2(BinaryWriter writer)
        {
            var size = GetCellCount();

            // Map Data
            writer.Write(BlockBorder);
            writer.Write(BaseLevel);

            // Startpoints
            writer.Write((byte)StartPoints.Length);
            for (int i = 0; i < StartPoints.Length; i++)
            {
                writer.Write((short)StartPoints[i].X);
                writer.Write((short)StartPoints[i].Y);
            }

            // Map Properties
            writer.Write((byte)(Properties.Count() + UnknownProperties.Count));
            foreach (var property in Properties)
            {
                writer.Write(property.GetType().FullName ?? throw new InvalidOperationException());
                SerializeType(writer, property);
            }

            // Add Data from unknown Property
            foreach (var unknownProperty in UnknownProperties)
            {
                writer.Write(unknownProperty.Key);
                writer.Write((short)unknownProperty.Value.Length);
                writer.Write(unknownProperty.Value, 0, unknownProperty.Value.Length);
            }

            // Collect MapTile/Material/TileProperty Types
            List<string> mapTileTypes = new List<string>();
            List<string> materialTypes = new List<string>();
            List<string> propertyTypes = new List<string>();
            for (int y = 0; y < size.Y; y++)
            {
                for (int x = 0; x < size.X; x++)
                {
                    MapTile tile = this[x, y];

                    if (tile == null)
                    {
                        // No Tile -> skip
                        continue;
                    }
                    else if (tile is UnknownMapTile)
                    {
                        // Unknown Tile Type
                        UnknownMapTile unknownTile = tile as UnknownMapTile;
                        if (!mapTileTypes.Contains(unknownTile.MapTileType))
                            mapTileTypes.Add(unknownTile.MapTileType);
                    }
                    else
                    {
                        // Common Tile Type
                        Type mapTileType = tile.GetType();
                        if (!mapTileTypes.Contains(mapTileType.FullName))
                            mapTileTypes.Add(mapTileType.FullName);
                    }

                    // Common Properties
                    foreach (var property in tile.Properties)
                    {
                        Type propertyType = property.GetType();
                        if (!propertyTypes.Contains(propertyType.FullName))
                            propertyTypes.Add(propertyType.FullName);
                    }

                    // Unknown Properties
                    foreach (var property in tile.UnknownProperties.Keys)
                    {
                        if (!propertyTypes.Contains(property))
                            propertyTypes.Add(property);
                    }

                    // Materials
                    if (tile.Material == null)
                    {
                        // No Material -> Skip
                    }
                    else if (tile.Material is UnknownMaterial unknownMaterial)
                    {
                        // Unknown Material
                        if (!materialTypes.Contains(unknownMaterial.MaterialType))
                            materialTypes.Add(unknownMaterial.MaterialType);
                    }
                    else
                    {
                        // Common Material
                        var materialType = tile.Material.GetType();
                        if (!materialTypes.Contains(materialType.FullName))
                            materialTypes.Add(materialType.FullName);
                    }
                }
            }

            // Write Map Tile Types
            writer.Write((byte)mapTileTypes.Count);
            foreach (var type in mapTileTypes)
                writer.Write(type);

            // Write Map Tile Property Types
            writer.Write((byte)propertyTypes.Count);
            foreach (var type in propertyTypes)
                writer.Write(type);

            // Write Material Types
            writer.Write((byte)materialTypes.Count);
            foreach (var type in materialTypes)
                writer.Write(type);

            // Zelleninfos
            for (int y = 0; y < size.Y; y++)
            {
                for (int x = 0; x < size.X; x++)
                {
                    MapTile tile = this[x, y];

                    // Map Tile
                    if (tile != null)
                    {
                        int index;
                        if (tile is UnknownMapTile) index = mapTileTypes.IndexOf((tile as UnknownMapTile).MapTileType) + 1;
                        else index = mapTileTypes.IndexOf(tile.GetType().FullName) + 1;

                        writer.Write((byte)index);
                        SerializeType(writer, tile);

                        // Material
                        if (tile.Material != null)
                        {
                            int materialIndex;
                            if (tile.Material is UnknownMaterial)
                                materialIndex = materialTypes.IndexOf(((UnknownMaterial) tile.Material).MaterialType) + 1;
                            else materialIndex = materialTypes.IndexOf(tile.Material.GetType().FullName) + 1;
                            writer.Write((byte)materialIndex);
                            SerializeType(writer, tile.Material);
                        }
                        else
                        {
                            writer.Write((byte)0);
                        }

                        // Properties
                        writer.Write((byte)(tile.Properties.Count() + tile.UnknownProperties.Count));
                        foreach (var property in tile.Properties)
                        {
                            int propertyIndex = propertyTypes.IndexOf(property.GetType().FullName);
                            writer.Write((byte)propertyIndex);
                            SerializeType(writer, property);
                        }
                        foreach (var property in tile.UnknownProperties)
                        {
                            int propertyIndex = propertyTypes.IndexOf(property.Key);
                            writer.Write((byte)propertyIndex);
                            writer.Write(property.Value.Length);
                            writer.Write(property.Value, 0, property.Value.Length);
                        }
                    }
                    else
                    {
                        writer.Write((byte)0);
                    }
                }
            }
        }

        private void DeserializeType(BinaryReader reader, ISerializableState state)
        {
            int dumpCount = reader.ReadInt16();
            byte[] dump = reader.ReadBytes(dumpCount);

            using (var mem = new MemoryStream(dump))
            {
                using (var memReader = new BinaryReader(mem))
                {
                    state.DeserializeFirst(memReader, 2);
                }
            }
        }

        /// <summary>
        /// Serializes a single Type into the output Stream.
        /// Format: length(byte), payload(byte[length])
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="state"></param>
        private void SerializeType(BinaryWriter writer, ISerializableState state)
        {
            using (var mem = new MemoryStream())
            {
                using (var memWriter = new BinaryWriter(mem))
                {
                    // Serialize
                    state.SerializeFirst(memWriter, 2);

                    // Copy to Buffer
                    var count = (int)mem.Position;
                    var buffer = new byte[count];
                    mem.Position = 0;
                    mem.Read(buffer, 0, count);

                    // Write to main Stream
                    writer.Write((short)count);
                    writer.Write(buffer, 0, count);
                }
            }
        }

        /// <summary>
        /// Serializes following Frames. (Not supported with the Map)
        /// </summary>
        /// <param name="writer">Output Stream</param>
        /// <param name="version">Protocol Version</param>
        public void SerializeUpdate(BinaryWriter writer, byte version)
        {
            throw new NotSupportedException("Update is not supported for Map");
        }

        /// <summary>
        /// Deserializes the Map.
        /// </summary>
        /// <param name="reader">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public void DeserializeFirst(BinaryReader reader, byte version)
        {
            switch (version)
            {
                case 2: DeserializeV2(reader); break;
                default: throw new ArgumentException("File Version not supported");
            }
        }

        private void DeserializeV2(BinaryReader reader)
        {
            UnknownProperties.Clear();
            Index2 size = GetCellCount();

            // Map Data
            BlockBorder = reader.ReadBoolean();
            BaseLevel = reader.ReadByte();

            // Startpoints
            StartPoints = new Index2[reader.ReadByte()];
            for (int i = 0; i < StartPoints.Length; i++)
                StartPoints[i] = new Index2(reader.ReadInt16(), reader.ReadInt16());

            // Map Properties
            int mapPropertyCount = reader.ReadByte();
            for (int i = 0; i < mapPropertyCount; i++)
            {
                string propertyName = reader.ReadString();
                var property = Properties.FirstOrDefault(p =>
                {
                    var fullName = p.GetType().FullName;
                    return fullName != null && fullName.Equals(propertyName);
                });
                if (property != null)
                {
                    DeserializeType(reader, property);
                }
                else
                {
                    // Dump Data in Unknown Properties
                    int dumpCount = reader.ReadInt16();
                    byte[] dump = reader.ReadBytes(dumpCount);
                    UnknownProperties.Add(propertyName, dump);
                }
            }

            // Read Map Tile Types
            int mapTileTypeCount = reader.ReadByte();
            List<string> mapTileTypes = new List<string>(mapTileTypeCount);
            for (int i = 0; i < mapTileTypeCount; i++)
                mapTileTypes.Add(reader.ReadString());

            // Read Property Types
            int propertyTypeCount = reader.ReadByte();
            List<string> propertyTypes = new List<string>(propertyTypeCount);
            for (int i = 0; i < propertyTypeCount; i++)
                propertyTypes.Add(reader.ReadString());

            // Read Material Types
            int materialTypeCount = reader.ReadByte();
            List<string> materialTypes = new List<string>(materialTypeCount);
            for (int i = 0; i < materialTypeCount; i++)
                materialTypes.Add(reader.ReadString());

            for (int y = 0; y < size.Y; y++)
            {
                for (int x = 0; x < size.X; x++)
                {
                    short tileTypeIndex = reader.ReadByte();

                    // Empty Cell
                    if (tileTypeIndex == 0)
                        continue;

                    // Identify Map Tile Type
                    string tileTypeName = mapTileTypes[tileTypeIndex - 1];
                    var tileTypeMap = _context.Mapper.MapTiles.FirstOrDefault(t => t.Type.FullName != null && t.Type.FullName.Equals(tileTypeName));
                    MapTile mapTile;
                    if (tileTypeMap != null)
                    {
                        // Deserialize known Type
                        mapTile = Activator.CreateInstance(tileTypeMap.Type, _context) as MapTile;
                        DeserializeType(reader, mapTile);
                    }
                    else
                    {
                        // Handle missing Map Tile Type
                        int bufferSize = reader.ReadInt16();
                        byte[] payload = reader.ReadBytes(bufferSize);
                        mapTile = new UnknownMapTile(_context, tileTypeName, payload);
                    }
                    this[x, y] = mapTile;

                    // Handle Material
                    short materialIndex = reader.ReadByte();
                    if (materialIndex > 0)
                    {
                        string materialTypeName = materialTypes[materialIndex - 1];
                        var materialMap = _context.Mapper.MapMaterials.FirstOrDefault(m => m.Type.FullName != null && m.Type.FullName.Equals(materialTypeName));
                        if (materialMap != null)
                        {
                            // Deserialize known Type
                            if (mapTile != null)
                            {
                                mapTile.Material = Activator.CreateInstance(materialMap.Type, _context) as MapMaterial;
                                DeserializeType(reader, mapTile.Material);
                            }
                        }
                        else
                        {
                            // Handle missing Material Type
                            int bufferSize = reader.ReadInt16();
                            byte[] payload = reader.ReadBytes(bufferSize);
                            if (mapTile != null)
                                mapTile.Material = new UnknownMaterial(_context, materialTypeName, payload);
                        }
                    }

                    // Handle Map Tile Properties
                    int propertyCount = reader.ReadByte();
                    for (int i = 0; i < propertyCount; i++)
                    {
                        int propertyIndex = reader.ReadByte();
                        string propertyName = propertyTypes[propertyIndex];
                        if (mapTile != null)
                        {
                            var property = mapTile.Properties.FirstOrDefault(p =>
                            {
                                var fullName = p.GetType().FullName;
                                return fullName != null && fullName.Equals(propertyName);
                            });
                            if (property != null)
                            {
                                // Deserialize Data
                                DeserializeType(reader, property);
                            }
                            else
                            {
                                // Handle missing Property Type
                                int bufferSize = reader.ReadInt16();
                                byte[] payload = reader.ReadBytes(bufferSize);
                                UnknownProperties.Add(propertyName, payload);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes all following Frames. (Not supported with the Map)
        /// </summary>
        /// <param name="stream">Input Stream</param>
        /// <param name="version">Protocol Version</param>
        public void DeserializeUpdate(BinaryReader stream, byte version)
        {
            throw new NotSupportedException("Update is not supported for Map");
        }
    }
}