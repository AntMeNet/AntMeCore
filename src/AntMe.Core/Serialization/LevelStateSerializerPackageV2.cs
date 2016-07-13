namespace AntMe.Serialization
{
    /// <summary>
    /// List of Package Types within a Frame.
    /// </summary>
    internal enum LevelStateSerializerPackageV2
    {
        // 0   >  Type (0) or Instance (1)
        // 1   >  Base (0) or Property (1)
        // 2   \  Operation: Insert = 01, Update = 10, Delete = 11
        // 3   /
        // 4   \
        // 5    \ Category: Level = 0x01, Map = 0x02, MapTile = 0x03, Material = 0x04, Faction = 0x05, Item = 0x06
        // 6    / 
        // 7   /

        /* First 4 Bits (Operation)
         * =========================
         * 
         * 0x0* -
         * 0x1* (Insert Base Type)
         * 0x2* (Update Base Type)
         * 0x3* (Delete Base Type)
         * 0x4* -
         * 0x5* (Insert Property Type)
         * 0x6* (Update Property Type)
         * 0x7* (Delete Property Type)
         * 0x8* -
         * 0x9* (Insert Base Instance)
         * 0xA* (Update Base Instance)
         * 0xB* (Delete Base Instance)
         * 0xC* -
         * 0xD* (Insert Property Instance)
         * 0xE* (Update Property Instance)
         * 0xF* (Delete Property Instance)
         * 
         */


        /* Last 4 Bits (Type)
         * ===================
         * 
         * 0x*0 -
         * 0x*1 Level
         * 0x*2 Map
         * 0x*3 MapTile
         * 0x*4 Material
         * 0x*5 Faction
         * 0x*6 Item
         * 0x*7 FactionItem
         * 0x*8 -
         * 0x*9 -
         * 0x*A -
         * 0x*B -
         * 0x*C -
         * 0x*D -
         * 0x*E -
         * 0x*F -
         * 
         */

        // Level
        LevelInsert = 0x91,
        LevelUpdate = 0xA1,
        LevelDelete = 0xB1,
        LevelTypeInsert = 0x11,
        LevelTypeUpdate = 0x21,
        LevelTypeDelete = 0x31,
        LevelPropertyInsert = 0xD1,
        LevelPropertyUpdate = 0xE1,
        LevelPropertyDelete = 0xF1,
        LevelPropertyTypeInsert = 0x01,
        LevelPropertyTypeUpdate = 0x01,
        LevelPropertyTypeDelete = 0x01,

        // Map
        MapInsert = 0x92,
        MapUpdate = 0xA2,
        MapDelete = 0xB2,
        MapTypeInsert = 0x12,
        MapTypeUpdate = 0x22,
        MapTypeDelete = 0x32,
        MapPropertyInsert = 0xD2,
        MapPropertyUpdate = 0xE2,
        MapPropertyDelete = 0xF2,
        MapPropertyTypeInsert = 0x52,
        MapPropertyTypeUpdate = 0x62,
        MapPropertyTypeDelete = 0x72,

        // MapTile
        MapTileInsert = 0x93,
        MapTileUpdate = 0xA3,
        MapTileDelete = 0xB3,
        MapTileTypeInsert = 0x13,
        MapTileTypeUpdate = 0x23,
        MapTileTypeDelete = 0x33,
        MapTilePropertyInsert = 0xD3,
        MapTilePropertyUpdate = 0xE3,
        MapTilePropertyDelete = 0xF3,
        MapTilePropertyTypeInsert = 0x53,
        MapTilePropertyTypeUpdate = 0x63,
        MapTilePropertyTypeDelete = 0x73,

        // Material
        MaterialInsert = 0x94,
        MaterialUpdate = 0xA4,
        MaterialDelete = 0xB4,
        MaterialTypeInsert = 0x14,
        MaterialTypeUpdate = 0x24,
        MaterialTypeDelete = 0x34,
        MaterialPropertyInsert = 0xD4,
        MaterialPropertyUpdate = 0xE4,
        MaterialPropertyDelete = 0xF4,
        MaterialPropertyTypeInsert = 0x54,
        MaterialPropertyTypeUpdate = 0x64,
        MaterialPropertyTypeDelete = 0x74,

        // Faction
        FactionInsert = 0x95,
        FactionUpdate = 0xA5,
        FactionDelete = 0xB5,
        FactionTypeInsert = 0x15,
        FactionTypeUpdate = 0x25,
        FactionTypeDelete = 0x35,
        FactionPropertyInsert = 0xD5,
        FactionPropertyUpdate = 0xE5,
        FactionPropertyDelete = 0xF5,
        FactionPropertyTypeInsert = 0x55,
        FactionPropertyTypeUpdate = 0x65,
        FactionPropertyTypeDelete = 0x75,

        // Item
        ItemInsert = 0x96,
        ItemUpdate = 0xA6,
        ItemDelete = 0xB6,
        ItemTypeInsert = 0x16,
        ItemTypeUpdate = 0x26,
        ItemTypeDelete = 0x36,
        ItemPropertyInsert = 0xD6,
        ItemPropertyUpdate = 0xE6,
        ItemPropertyDelete = 0xF6,
        ItemPropertyTypeInsert = 0x56,
        ItemPropertyTypeUpdate = 0x66,
        ItemPropertyTypeDelete = 0x76,

        // FactionItem
        FactionItemInsert = 0x97,
        FactionItemUpdate = 0xA7,
        FactionItemDelete = 0xB7,
        FactionItemTypeInsert = 0x17,
        FactionItemTypeUpdate = 0x27,
        FactionItemTypeDelete = 0x37,
        FactionItemPropertyInsert = 0xD7,
        FactionItemPropertyUpdate = 0xE7,
        FactionItemPropertyDelete = 0xF7,
        FactionItemPropertyTypeInsert = 0x57,
        FactionItemPropertyTypeUpdate = 0x67,
        FactionItemPropertyTypeDelete = 0x77,
    }
}
