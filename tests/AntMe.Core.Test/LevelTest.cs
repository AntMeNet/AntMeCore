using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AntMe.Runtime;

namespace AntMe.Core.Test
{
    /// <summary>
    /// Testet die richtige Funktionalität der Level-Klasse.
    /// Autor: Tom Wendel
    /// </summary>
    [TestClass]
    public class LevelTest
    {
        private TypeMapper mapper;
        private Settings settings;

        [TestInitialize]
        public void Init()
        {
            mapper = new TypeMapper();
            settings = new Settings();
        }

        [TestCleanup]
        public void Cleanup()
        {
            mapper = null;
        }

        [TestMethod]
        public void CreateLevel()
        {
            DebugLevel level = new DebugLevel(mapper);

            level.Init(mapper, settings.Clone());

            while (level.Mode == LevelMode.Running)
                level.NextState();
        }



        //    private Guid guid = Guid.Parse("{2B362702-DEF0-4CC0-B9CB-D61D299C4276}");
        //    private string name = "Debug Level";
        //    private string description = "Debug Level Description";

        //    private DebugLevel level;

        //    #region Init

        //    [TestInitialize]
        //    public void Init()
        //    {
        //        ITypeResolver resolver = null;
        //        level = new DebugLevel(resolver);

        //        level.DoSettingsAction = DebugLevelAction.DoNothing;
        //        level.GetMapAction = DebugLevelAction.DoNothing;
        //        level.OnInitAction = DebugLevelAction.DoNothing;
        //        level.OnInsertItemAction = DebugLevelAction.DoNothing;
        //        level.OnRemoveItemAction = DebugLevelAction.DoNothing;
        //        level.OnUpdateAction = DebugLevelAction.DoNothing;

        //        level.AddedItem += level_AddedItem;
        //        level.DoSettingsCall += level_DoSettingsCall;
        //        level.GetMapCall += level_GetMapCall;
        //        level.OnInitCall += level_OnInitCall;
        //        level.OnInsertItemCall += level_OnInsertItemCall;
        //        level.OnRemoveItemCall += level_OnRemoveItemCall;
        //        level.OnUpdateCall += level_OnUpdateCall;
        //        level.RemovedItem += level_RemovedItem;
        //    }

        //    [TestCleanup]
        //    public void Cleaup()
        //    {
        //        if (level != null)
        //        {
        //            level.AddedItem -= level_AddedItem;
        //            level.DoSettingsCall -= level_DoSettingsCall;
        //            level.GetMapCall -= level_GetMapCall;
        //            level.OnInitCall -= level_OnInitCall;
        //            level.OnInsertItemCall -= level_OnInsertItemCall;
        //            level.OnRemoveItemCall -= level_OnRemoveItemCall;
        //            level.OnUpdateCall -= level_OnUpdateCall;
        //            level.RemovedItem -= level_RemovedItem;

        //            level = null;
        //        }
        //    }

        //    #endregion

        //    #region Eventhandler

        //    int removedItemCount = 0;
        //    int OnUpdateCount = 0;
        //    int OnRemoveItemCount = 0;
        //    int OnInsertItemCount = 0;
        //    int OnInitCount = 0;
        //    int GetMapCount = 0;
        //    int DoSettingsCount = 0;
        //    int AddedItemCount = 0;

        //    void level_RemovedItem(GameItem item)
        //    {
        //        removedItemCount++;
        //    }

        //    void level_OnUpdateCall(DebugLevel level)
        //    {
        //        OnUpdateCount++;
        //    }

        //    void level_OnRemoveItemCall(DebugLevel level, GameItem item)
        //    {
        //        OnRemoveItemCount++;
        //    }

        //    void level_OnInsertItemCall(DebugLevel level, GameItem item)
        //    {
        //        OnInsertItemCount++;
        //    }

        //    void level_OnInitCall(DebugLevel level)
        //    {
        //        OnInitCount++;
        //    }

        //    void level_GetMapCall(DebugLevel level)
        //    {
        //        GetMapCount++;
        //    }

        //    void level_DoSettingsCall(DebugLevel level)
        //    {
        //        DoSettingsCount++;
        //    }

        //    void level_AddedItem(GameItem item)
        //    {
        //        AddedItemCount++;
        //    }

        //    private void ResetCounter()
        //    {
        //        removedItemCount = 0;
        //        OnUpdateCount = 0;
        //        OnRemoveItemCount = 0;
        //        OnInsertItemCount = 0;
        //        OnInitCount = 0;
        //        GetMapCount = 0;
        //        DoSettingsCount = 0;
        //        AddedItemCount = 0;
        //    }

        //    #endregion

        //    #region Init() Tests

        //    /// <summary>
        //    /// Prüft, ob die Properties vor der Initialisierung die richtigen Werte haben
        //    /// </summary>
        //    [TestMethod]
        //    public void BeforeInitPropertyCheck()
        //    {
        //        Assert.AreEqual(guid, level.Guid);
        //        Assert.AreEqual(name, level.Name);
        //        Assert.AreEqual(description, level.Description);
        //        Assert.AreEqual(null, level.Factions);  // keine Factions
        //        Assert.AreNotEqual(null, level.Items);  // Leere Item-Liste
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Uninit, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);
        //    }

        //    /// <summary>
        //    /// Initialisierung eines Levels ohne LevelDescription-Attribut
        //    /// </summary>
        //    [TestMethod]
        //    public void InitLevelWithoutAttribute()
        //    {
        //        LevelNoAttribute lvl = new LevelNoAttribute();
        //        try
        //        {
        //            lvl.Init();
        //            Assert.Fail("Should throw an exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, lvl.Factions);  // keine Factions
        //        Assert.AreNotEqual(null, lvl.Items);  // Leere Item-Liste
        //        Assert.AreEqual(0, lvl.Items.Count);
        //        Assert.AreEqual(null, lvl.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, lvl.Mode);
        //        Assert.AreEqual(-1, lvl.Round);
        //        Assert.AreEqual(null, lvl.Settings);
        //    }

        //    /// <summary>
        //    /// Initialisiert ein Level ohne factions, obwohl das LevelDescription 
        //    /// Attribut mindestens einen Spieler verlangt.
        //    /// </summary>
        //    [TestMethod]
        //    public void InitLevelWithMinFactionsOf1()
        //    {
        //        LevelMinPlayer1 lvl = new LevelMinPlayer1();
        //        try
        //        {
        //            lvl.Init();
        //            Assert.Fail("Should throw an exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, lvl.Factions);  // keine Factions
        //        Assert.AreNotEqual(null, lvl.Items);  // Leere Item-Liste
        //        Assert.AreEqual(0, lvl.Items.Count);
        //        Assert.AreEqual(null, lvl.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, lvl.Mode);
        //        Assert.AreEqual(-1, lvl.Round);
        //        Assert.AreEqual(null, lvl.Settings);
        //    }

        //    /// <summary>
        //    /// Initialisiert ein Level ohne factions, obwohl das LevelDescription 
        //    /// Attribut mindestens einen Spieler verlangt.
        //    /// </summary>
        //    [TestMethod]
        //    public void InitLevelWithMinFactionsOf1NullParameter()
        //    {
        //        LevelMinPlayer1 lvl = new LevelMinPlayer1();
        //        try
        //        {
        //            lvl.Init(null, null); // Null-Parameter sollten als leere Factions erkannt werden
        //            Assert.Fail("Should throw an exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, lvl.Factions);  // keine Factions
        //        Assert.AreNotEqual(null, lvl.Items);  // Leere Item-Liste
        //        Assert.AreEqual(0, lvl.Items.Count);
        //        Assert.AreEqual(null, lvl.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, lvl.Mode);
        //        Assert.AreEqual(-1, lvl.Round);
        //        Assert.AreEqual(null, lvl.Settings);
        //    }

        //    /// <summary>
        //    /// Initialisiert ein Level, dessen LevelDescription maximal einen Spieler 
        //    /// erlaubt, mit 2 Spielern.
        //    /// </summary>
        //    [TestMethod]
        //    public void InitLevelWithMaxFactionOf1()
        //    {
        //        LevelMaxPlayer1 lvl = new LevelMaxPlayer1();

        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);
        //        Faction f2 = new AntFaction(typeof(RawColony), "German2", PlayerColor.Red);

        //        try
        //        {
        //            lvl.Init(f1, f2);
        //            Assert.Fail("Should throw an exception");
        //            return;
        //        }
        //        catch (NotSupportedException) { }


        //        Assert.AreEqual(null, lvl.Factions);  // keine Factions
        //        Assert.AreNotEqual(null, lvl.Items);  // Leere Item-Liste
        //        Assert.AreEqual(0, lvl.Items.Count);
        //        Assert.AreEqual(null, lvl.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, lvl.Mode);
        //        Assert.AreEqual(-1, lvl.Round);
        //        Assert.AreEqual(null, lvl.Settings);
        //    }

        //    /// <summary>
        //    /// Initialisiert ein Level, dessen LevelDescription maximal einen Spieler 
        //    /// erlaubt, mit einem Spieler im zweiten Slot.
        //    /// </summary>
        //    [TestMethod]
        //    public void InitLevelWithOneFactionInSlot2()
        //    {
        //        LevelMaxPlayer1 lvl = new LevelMaxPlayer1();

        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        lvl.Init(null, f1);

        //        Assert.AreNotEqual(null, lvl.Factions);  // Faction List
        //        Assert.AreEqual(8, lvl.Factions.Length); // 8 Slots initialisiert
        //        Assert.AreEqual(null, lvl.Factions[0]);
        //        Assert.AreEqual(f1, lvl.Factions[1]);
        //        Assert.AreEqual(null, lvl.Factions[2]);
        //        Assert.AreEqual(null, lvl.Factions[3]);
        //        Assert.AreEqual(null, lvl.Factions[4]);
        //        Assert.AreEqual(null, lvl.Factions[5]);
        //        Assert.AreEqual(null, lvl.Factions[6]);
        //        Assert.AreEqual(null, lvl.Factions[7]);
        //        Assert.AreNotEqual(null, lvl.Items);
        //        Assert.AreEqual(1, lvl.Items.Count);
        //        Assert.AreNotEqual(null, lvl.Map);
        //        Assert.AreEqual(LevelMode.Running, lvl.Mode);
        //        Assert.AreEqual(-1, lvl.Round);
        //        Assert.AreNotEqual(null, lvl.Settings);
        //    }

        //    /// <summary>
        //    /// Initialisiert ein Level mit 0 Spielern.
        //    /// </summary>
        //    [TestMethod]
        //    public void InitLevelWithoutPlayer()
        //    {
        //        level.Init();

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length); // 8 Slots initialisiert
        //        Assert.AreEqual(null, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);
        //    }

        //    /// <summary>
        //    /// Initialisiert ein Level mit einem Spieler.
        //    /// </summary>
        //    [TestMethod]
        //    public void InitLevelWith1Player()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.Init(f1);

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length); // 8 Slots initialisiert
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(1, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);
        //    }

        //    /// <summary>
        //    /// Initialisiert ein Level mit einem Spieler in Slot 3.
        //    /// </summary>
        //    [TestMethod]
        //    public void InitLevelWith1PlayerInSlot3()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.Init(null, null, f1);

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length); // 8 Slots initialisiert
        //        Assert.AreEqual(null, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(f1, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(1, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);
        //    }

        //    /// <summary>
        //    /// Initialisiert ein Level mit zwei Spielern in Slot 1 und 3.
        //    /// </summary>
        //    [TestMethod]
        //    public void InitLevelWith2PlayerInSlot1And3()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);
        //        Faction f2 = new AntFaction(typeof(RawColony), "German2", PlayerColor.Red);

        //        level.Init(f1, null, f2);

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length); // 8 Slots initialisiert
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(f2, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(2, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);
        //    }

        //    /// <summary>
        //    /// Initialisiert ein Level mit zwei Spielern in 9 Slots (zu viele)
        //    /// </summary>
        //    [TestMethod]
        //    public void InitLevelWithTooManySlots()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);
        //        Faction f2 = new AntFaction(typeof(RawColony), "German2", PlayerColor.Red);

        //        try
        //        {
        //            level.Init(f1, null, null, null, null, null, null, f2, null);
        //            Assert.Fail("Should fail");
        //        }
        //        catch (ArgumentOutOfRangeException) { }

        //        Assert.AreEqual(null, level.Factions);  // keine Factions
        //        Assert.AreNotEqual(null, level.Items);  // Leere Item-Liste
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);
        //    }

        //    /// <summary>
        //    /// Initialisiert ein Level mit zwei Spielern mit der selben Farbe.
        //    /// </summary>
        //    [TestMethod]
        //    public void InitLevelWith2PlayerSameColor()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);
        //        Faction f2 = new AntFaction(typeof(RawColony), "German2", PlayerColor.Blue);

        //        try
        //        {
        //            level.Init(f1, f2);
        //            Assert.Fail("Should fail");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, level.Factions);  // keine Factions
        //        Assert.AreNotEqual(null, level.Items);  // Leere Item-Liste
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);
        //    }

        //    /// <summary>
        //    /// Initilaisiert ein Level das im Slot 0 nur Bugs erlaubt mit Ants in Slot 0.
        //    /// </summary>
        //    [TestMethod]
        //    public void InitLevelWithFilteredFactionAntToBug()
        //    {
        //        LevelFactionFilterOnlyBugOn1 lvl = new LevelFactionFilterOnlyBugOn1();

        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        try
        //        {
        //            lvl.Init(f1);
        //            Assert.Fail("Should fail");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, lvl.Factions);  // keine Factions
        //        Assert.AreNotEqual(null, lvl.Items);  // Leere Item-Liste
        //        Assert.AreEqual(0, lvl.Items.Count);
        //        Assert.AreEqual(null, lvl.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, lvl.Mode);
        //        Assert.AreEqual(-1, lvl.Round);
        //        Assert.AreEqual(null, lvl.Settings);
        //    }

        //    /// <summary>
        //    /// Initilaisiert ein Level das im Slot 0 nur Bugs, in 1 nur Ants erlaubt mit Ants in Slot 0.
        //    /// </summary>
        //    [TestMethod]
        //    public void InitLevelWithFilteredFactionAntToBug2()
        //    {
        //        LevelFactionFilterBugOn1AndAntOn2 lvl = new LevelFactionFilterBugOn1AndAntOn2();

        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        try
        //        {
        //            lvl.Init(f1);
        //            Assert.Fail("Should fail");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, lvl.Factions);  // keine Factions
        //        Assert.AreNotEqual(null, lvl.Items);  // Leere Item-Liste
        //        Assert.AreEqual(0, lvl.Items.Count);
        //        Assert.AreEqual(null, lvl.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, lvl.Mode);
        //        Assert.AreEqual(-1, lvl.Round);
        //        Assert.AreEqual(null, lvl.Settings);
        //    }

        //    /// <summary>
        //    /// Initilaisiert ein Level das im Slot 0 Bugs und Ants erlaubt mit Ants in Slot 0.
        //    /// </summary>
        //    [TestMethod]
        //    public void InitLevelWithFilteredFactionAntToAnt()
        //    {
        //        LevelFactionFilterBugAndAntOn1 lvl = new LevelFactionFilterBugAndAntOn1();

        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        lvl.Init(f1);

        //        Assert.AreNotEqual(null, lvl.Factions);  // Faction List
        //        Assert.AreEqual(8, lvl.Factions.Length); // 8 Slots initialisiert
        //        Assert.AreEqual(f1, lvl.Factions[0]);
        //        Assert.AreEqual(null, lvl.Factions[1]);
        //        Assert.AreEqual(null, lvl.Factions[2]);
        //        Assert.AreEqual(null, lvl.Factions[3]);
        //        Assert.AreEqual(null, lvl.Factions[4]);
        //        Assert.AreEqual(null, lvl.Factions[5]);
        //        Assert.AreEqual(null, lvl.Factions[6]);
        //        Assert.AreEqual(null, lvl.Factions[7]);
        //        Assert.AreNotEqual(null, lvl.Items);
        //        Assert.AreEqual(1, lvl.Items.Count);
        //        Assert.AreNotEqual(null, lvl.Map);
        //        Assert.AreEqual(LevelMode.Running, lvl.Mode);
        //        Assert.AreEqual(-1, lvl.Round);
        //        Assert.AreNotEqual(null, lvl.Settings);
        //    }

        //    /// <summary>
        //    /// Initilaisiert ein Level das im Slot 0 Bugs, in Slot 1 Ants erlaubt mit Ants in Slot 1.
        //    /// </summary>
        //    [TestMethod]
        //    public void InitLevelWithFilteredFactionAntToAnt2()
        //    {
        //        LevelFactionFilterBugOn1AndAntOn2 lvl = new LevelFactionFilterBugOn1AndAntOn2();

        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        lvl.Init(null, f1);

        //        Assert.AreNotEqual(null, lvl.Factions);  // Faction List
        //        Assert.AreEqual(8, lvl.Factions.Length); // 8 Slots initialisiert
        //        Assert.AreEqual(null, lvl.Factions[0]);
        //        Assert.AreEqual(f1, lvl.Factions[1]);
        //        Assert.AreEqual(null, lvl.Factions[2]);
        //        Assert.AreEqual(null, lvl.Factions[3]);
        //        Assert.AreEqual(null, lvl.Factions[4]);
        //        Assert.AreEqual(null, lvl.Factions[5]);
        //        Assert.AreEqual(null, lvl.Factions[6]);
        //        Assert.AreEqual(null, lvl.Factions[7]);
        //        Assert.AreNotEqual(null, lvl.Items);
        //        Assert.AreEqual(1, lvl.Items.Count);
        //        Assert.AreNotEqual(null, lvl.Map);
        //        Assert.AreEqual(LevelMode.Running, lvl.Mode);
        //        Assert.AreEqual(-1, lvl.Round);
        //        Assert.AreNotEqual(null, lvl.Settings);
        //    }

        //    #endregion

        //    #region AddScreenHighlights() Tests

        //    // TODO: Implement

        //    #endregion

        //    #region InsertItem() Tests

        //    // TODO: Implement

        //    #endregion

        //    #region DoInit Tests

        //    // Exception (fail)
        //    // ScreenHighlight (sollte gehen)
        //    // Draw/Fail/finish (fail)
        //    // Insert/Delete Item (sollte gehen)
        //    // Insert/Delete Trigger (sollte gehen)
        //    // Next State (fail)

        //    /// <summary>
        //    /// OnInit -> Exception (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInitWithException()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInitAction = DebugLevelAction.ThrowException;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotImplementedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(1, OnInsertItemCount); // Anthill
        //        Assert.AreEqual(1, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(1, AddedItemCount); // Anthill
        //    }

        //    /// <summary>
        //    /// OnInit -> AddScreenHighlight (muss gehen)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInitWithAddScreenHighlight()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInitAction = DebugLevelAction.CallAddScreenHighlight;

        //        level.Init(f1);

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(1, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(1, OnInsertItemCount); // Anthill
        //        Assert.AreEqual(1, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(1, AddedItemCount); // Anthill
        //    }

        //    /// <summary>
        //    /// OnInit -> Draw (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInitWithDraw()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInitAction = DebugLevelAction.CallDraw;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(1, OnInsertItemCount); // Anthill
        //        Assert.AreEqual(1, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(1, AddedItemCount); // Anthill
        //    }

        //    /// <summary>
        //    /// OnInit -> Fail (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInitWithFail()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInitAction = DebugLevelAction.CallFail0;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(1, OnInsertItemCount); // Anthill
        //        Assert.AreEqual(1, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(1, AddedItemCount); // Anthill
        //    }

        //    /// <summary>
        //    /// OnInit -> Finish (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInitWithFinish()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInitAction = DebugLevelAction.CallFinish0;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(1, OnInsertItemCount); // Anthill
        //        Assert.AreEqual(1, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(1, AddedItemCount); // Anthill
        //    }

        //    /// <summary>
        //    /// OnInit -> InsertItem (muss gehen)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInitWithInsertItem()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInitAction = DebugLevelAction.CallInsertItem;

        //        level.Init(f1);

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(2, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(2, OnInsertItemCount); // Anthill
        //        Assert.AreEqual(1, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(2, AddedItemCount); // Anthill
        //    }

        //    /// <summary>
        //    /// OnInit -> RemoveItem ohne Insert (muss gehen)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInitWithRemoveItemWithoutAdd()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInitAction = DebugLevelAction.CallRemoveItemWithoutAdd;

        //        level.Init(f1);

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(1, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(1, OnInsertItemCount); // Anthill
        //        Assert.AreEqual(1, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(1, AddedItemCount); // Anthill
        //    }

        //    /// <summary>
        //    /// OnInit -> RemoveItem mit Insert (muss gehen)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInitWithRemoveItemWithAdd()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInitAction = DebugLevelAction.CallRemoveItemWithAdd;

        //        level.Init(f1);

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(1, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(1, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(1, OnRemoveItemCount);
        //        Assert.AreEqual(2, OnInsertItemCount); // Anthill
        //        Assert.AreEqual(1, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(2, AddedItemCount); // Anthill
        //    }

        //    /// <summary>
        //    /// OnInit -> RegisterTrigger (muss gehen)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInitWithRegisterTrigger()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInitAction = DebugLevelAction.CallRegisterTrigger;

        //        level.Init(f1);

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(1, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(1, OnInsertItemCount); // Anthill
        //        Assert.AreEqual(1, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(1, AddedItemCount); // Anthill
        //    }

        //    /// <summary>
        //    /// OnInit -> UnregisterTrigger (muss gehen)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInitWithUnregisterTrigger()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInitAction = DebugLevelAction.CallUnregisterTrigger;

        //        level.Init(f1);

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(1, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(1, OnInsertItemCount); // Anthill
        //        Assert.AreEqual(1, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(1, AddedItemCount); // Anthill
        //    }

        //    /// <summary>
        //    /// OnInit -> NextState (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInitWithNextState()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInitAction = DebugLevelAction.CallNextState;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(1, OnInsertItemCount); // Anthill
        //        Assert.AreEqual(1, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(1, AddedItemCount); // Anthill
        //    }

        //    #endregion

        //    #region DoSettings Tests

        //    // Exception (fail)
        //    // ScreenHighlight (fail)
        //    // Draw/Fail/finish (fail)
        //    // Insert/Delete Item (fail)
        //    // Insert/Delete Trigger (fail)
        //    // Next State (fail)

        //    /// <summary>
        //    /// DoSettings -> Exception (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void DoSettingsWithException()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.DoSettingsAction = DebugLevelAction.ThrowException;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotImplementedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// DoSettings -> ScreenHighlight (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void DoSettingsWithAddScreenHighlight()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.DoSettingsAction = DebugLevelAction.CallAddScreenHighlight;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// DoSettings -> Draw (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void DoSettingsWithDraw()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.DoSettingsAction = DebugLevelAction.CallDraw;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }


        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// DoSettings -> Finish (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void DoSettingsWithFinish()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.DoSettingsAction = DebugLevelAction.CallFinish0;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }


        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// DoSettings -> Fail (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void DoSettingsWithFail()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.DoSettingsAction = DebugLevelAction.CallFail0;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }


        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// DoSettings -> InsertItem (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void DoSettingsWithInsertItem()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.DoSettingsAction = DebugLevelAction.CallInsertItem;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }


        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// DoSettings -> RemoveItem (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void DoSettingsWithRemoveItem()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.DoSettingsAction = DebugLevelAction.CallRemoveItemWithoutAdd;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }


        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// DoSettings -> RegisterTrigger (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void DoSettingsWithRegisterTrigger()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.DoSettingsAction = DebugLevelAction.CallRegisterTrigger;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }


        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// DoSettings -> UnregisterTrigger (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void DoSettingsWithUnregisterTrigger()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.DoSettingsAction = DebugLevelAction.CallUnregisterTrigger;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }


        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// DoSettings -> NextState (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void DoSettingsWithNextState()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.DoSettingsAction = DebugLevelAction.CallNextState;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }


        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount); // Erster Init
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount); // Call durch Init
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    #endregion

        //    #region GetMap Tests

        //    // Exception (fail)
        //    // ScreenHighlight (fail)
        //    // Draw/Fail/finish (fail)
        //    // Insert/Delete Item (fail)
        //    // Insert/Delete Trigger (fail)
        //    // Next State (fail)

        //    /// <summary>
        //    /// GetMap -> Exception (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void GetMapWithException()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.GetMapAction = DebugLevelAction.ThrowException;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotImplementedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(0, DoSettingsCount);
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// GetMap -> AddScreenHighlight (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void GetMapWithAddScreenHighlight()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.GetMapAction = DebugLevelAction.CallAddScreenHighlight;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(0, DoSettingsCount);
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// GetMap -> Draw (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void GetMapWithDraw()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.GetMapAction = DebugLevelAction.CallDraw;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(0, DoSettingsCount);
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// GetMap -> Fail (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void GetMapWithFail()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.GetMapAction = DebugLevelAction.CallFail0;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(0, DoSettingsCount);
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// GetMap -> Finish (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void GetMapWithFinish()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.GetMapAction = DebugLevelAction.CallFinish0;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(0, DoSettingsCount);
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// GetMap -> InsertItem (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void GetMapWithInsertItem()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.GetMapAction = DebugLevelAction.CallInsertItem;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(0, DoSettingsCount);
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// GetMap -> RemoveItem (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void GetMapWithRemoveItem()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.GetMapAction = DebugLevelAction.CallRemoveItemWithoutAdd;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(0, DoSettingsCount);
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// GetMap -> NextState (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void GetMapWithNextState()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.GetMapAction = DebugLevelAction.CallNextState;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(0, DoSettingsCount);
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// GetMap -> RegisterTrigger (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void GetMapWithRegisterTrigger()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.GetMapAction = DebugLevelAction.CallRegisterTrigger;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(0, DoSettingsCount);
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// GetMap -> UnregisterTrigger (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void GetMapWithUnregisterTrigger()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.GetMapAction = DebugLevelAction.CallUnregisterTrigger;
        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(null, level.Factions);  // Faction List
        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(0, level.Items.Count);
        //        Assert.AreEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.InitFailed, level.Mode);
        //        Assert.AreEqual(-1, level.Round);
        //        Assert.AreEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(0, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(0, OnInsertItemCount);
        //        Assert.AreEqual(0, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(0, DoSettingsCount);
        //        Assert.AreEqual(0, AddedItemCount);
        //    }

        //    #endregion

        //    #region OnUpdate Tests

        //    // ThrowException (fail)
        //    // CallAddScreenHighlight (muss gehen)
        //    // CallGetMap (muss gehen)
        //    // CallInit (fail)
        //    // CallInsertItem (muss gehen)
        //    // CallNextState (stackoverflow)
        //    // CallRemoveItemWithoutAdd (muss gehen)
        //    // CallRemoveItemWithAdd (muss gehen)
        //    // CallRegisterTrigger (muss gehen)
        //    // CallUnregisterTrigger (muss gehen)
        //    // CallFinish0 (muss gehen)
        //    // CallFinish2 (fail, weil 2 nicht existiert)
        //    // CallFail0 (muss gehen)
        //    // CallFail2 (fail, weil 2 nicht existiert)
        //    // CallDraw (muss gehen)

        //    /// <summary>
        //    /// OnUpdate -> Exception (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnUpdateWithException()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnUpdateAction = DebugLevelAction.ThrowException;
        //        level.Init(f1);

        //        MainState state = null;
        //        state = level.NextState();

        //        Assert.AreNotEqual(null, state); // leerer State
        //        Assert.AreEqual(LevelMode.FailedSystem, state.Mode);
        //        Assert.IsNotNull(level.LastException);

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);

        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(2, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.FailedSystem, level.Mode);
        //        Assert.AreEqual(0, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(1, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(2, OnInsertItemCount);
        //        Assert.AreEqual(1, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount);
        //        Assert.AreEqual(2, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// OnUpdate -> AddScreenHighlight (muss gehen)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnUpdateWithAddScreenHighlight()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnUpdateAction = DebugLevelAction.CallAddScreenHighlight;
        //        level.Init(f1);
        //        MainState state = level.NextState();

        //        Assert.AreNotEqual(null, state); // kein leerer State
        //        Assert.AreEqual(1, state.ScreenHighlights.Count); // Highlight muss da sein

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);

        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(2, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(0, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(1, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(2, OnInsertItemCount);
        //        Assert.AreEqual(1, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount);
        //        Assert.AreEqual(2, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// OnUpdate -> GetMap (muss gehen)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnUpdateWithGetMap()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnUpdateAction = DebugLevelAction.CallGetMap;
        //        level.Init(f1);
        //        MainState state = level.NextState();

        //        Assert.AreNotEqual(null, state); // kein leerer State

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);

        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(2, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(0, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(1, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(2, OnInsertItemCount);
        //        Assert.AreEqual(1, OnInitCount);
        //        Assert.AreEqual(2, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount);
        //        Assert.AreEqual(2, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// OnUpdate -> Init (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnUpdateWithInit()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnUpdateAction = DebugLevelAction.CallInit;
        //        level.Init(f1);

        //        MainState state = null;
        //        state = level.NextState();

        //        Assert.AreNotEqual(null, state); // leerer State
        //        Assert.AreEqual(LevelMode.FailedSystem, state.Mode);
        //        Assert.IsNotNull(level.LastException);

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);

        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(2, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.FailedSystem, level.Mode);
        //        Assert.AreEqual(0, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(1, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(2, OnInsertItemCount);
        //        Assert.AreEqual(1, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount);
        //        Assert.AreEqual(2, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// OnUpdate -> InsertItem (muss gehen)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnUpdateWithInsertItem()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnUpdateAction = DebugLevelAction.CallInsertItem;
        //        level.Init(f1);
        //        MainState state = level.NextState();

        //        Assert.AreNotEqual(null, state); // kein leerer State

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);

        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(3, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(0, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(1, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(3, OnInsertItemCount);
        //        Assert.AreEqual(1, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount);
        //        Assert.AreEqual(3, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// OnUpdate -> RemoveItem with Insert (muss gehen)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnUpdateWithRemoveItemWithInsert()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnUpdateAction = DebugLevelAction.CallRemoveItemWithAdd;
        //        level.Init(f1);
        //        MainState state = level.NextState();

        //        Assert.AreNotEqual(null, state); // kein leerer State

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);

        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(2, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(0, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(1, removedItemCount);
        //        Assert.AreEqual(1, OnUpdateCount);
        //        Assert.AreEqual(1, OnRemoveItemCount);
        //        Assert.AreEqual(3, OnInsertItemCount);
        //        Assert.AreEqual(1, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount);
        //        Assert.AreEqual(3, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// OnUpdate -> RemoveItem without Insert (muss gehen)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnUpdateWithRemoveItemWithoutInsert()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnUpdateAction = DebugLevelAction.CallRemoveItemWithoutAdd;
        //        level.Init(f1);
        //        MainState state = level.NextState();

        //        Assert.AreNotEqual(null, state); // kein leerer State

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);

        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(2, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(0, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(1, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(2, OnInsertItemCount);
        //        Assert.AreEqual(1, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount);
        //        Assert.AreEqual(2, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// OnUpdate -> RegisterTrigger (muss gehen)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnUpdateWithRegisterTrigger()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnUpdateAction = DebugLevelAction.CallRegisterTrigger;
        //        level.Init(f1);
        //        MainState state = level.NextState();

        //        Assert.AreNotEqual(null, state); // kein leerer State

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);

        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(2, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(0, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(1, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(2, OnInsertItemCount);
        //        Assert.AreEqual(1, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount);
        //        Assert.AreEqual(2, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// OnUpdate -> UnregisterTrigger (muss gehen)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnUpdateWithUnregisterTrigger()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnUpdateAction = DebugLevelAction.CallUnregisterTrigger;
        //        level.Init(f1);
        //        MainState state = level.NextState();

        //        Assert.AreNotEqual(null, state); // kein leerer State

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);

        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(2, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //        Assert.AreEqual(0, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(1, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(2, OnInsertItemCount);
        //        Assert.AreEqual(1, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount);
        //        Assert.AreEqual(2, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// OnUpdate -> Finish0 (muss gehen)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnUpdateWithFinish0()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnUpdateAction = DebugLevelAction.CallFinish0;
        //        level.Init(f1);
        //        MainState state = level.NextState();

        //        Assert.AreNotEqual(null, state); // kein leerer State

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);

        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(2, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.FinishedPlayer1, level.Mode);
        //        Assert.AreEqual(0, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(1, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(2, OnInsertItemCount);
        //        Assert.AreEqual(1, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount);
        //        Assert.AreEqual(2, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// OnUpdate -> Finish2 (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnUpdateWithFinish2()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnUpdateAction = DebugLevelAction.CallFinish2;
        //        level.Init(f1);

        //        MainState state = null;
        //        state = level.NextState();

        //        Assert.AreNotEqual(null, state); // leerer State
        //        Assert.AreEqual(LevelMode.FailedSystem, state.Mode);
        //        Assert.IsNotNull(level.LastException);

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);

        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(2, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.FailedSystem, level.Mode);
        //        Assert.AreEqual(0, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(1, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(2, OnInsertItemCount);
        //        Assert.AreEqual(1, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount);
        //        Assert.AreEqual(2, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// OnUpdate -> Fail0 (muss gehen)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnUpdateWithFail0()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnUpdateAction = DebugLevelAction.CallFail0;
        //        level.Init(f1);
        //        MainState state = level.NextState();

        //        Assert.AreNotEqual(null, state); // kein leerer State

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);

        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(2, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.FailedPlayer1, level.Mode);
        //        Assert.AreEqual(0, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(1, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(2, OnInsertItemCount);
        //        Assert.AreEqual(1, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount);
        //        Assert.AreEqual(2, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// OnUpdate -> Fail2 (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnUpdateWithFail2()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnUpdateAction = DebugLevelAction.CallFail2;
        //        level.Init(f1);

        //        MainState state = null;
        //        state = level.NextState();

        //        Assert.AreNotEqual(null, state); // leerer State
        //        Assert.AreEqual(LevelMode.FailedSystem, state.Mode);
        //        Assert.IsNotNull(level.LastException);

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);

        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(2, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.FailedSystem, level.Mode);
        //        Assert.AreEqual(0, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(1, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(2, OnInsertItemCount);
        //        Assert.AreEqual(1, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount);
        //        Assert.AreEqual(2, AddedItemCount);
        //    }

        //    /// <summary>
        //    /// OnUpdate -> Draw (muss gehen)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnUpdateWithDraw()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnUpdateAction = DebugLevelAction.CallDraw;
        //        level.Init(f1);
        //        MainState state = level.NextState();

        //        Assert.AreNotEqual(null, state); // kein leerer State

        //        Assert.AreNotEqual(null, level.Factions);  // Faction List
        //        Assert.AreEqual(8, level.Factions.Length);
        //        Assert.AreEqual(f1, level.Factions[0]);
        //        Assert.AreEqual(null, level.Factions[1]);
        //        Assert.AreEqual(null, level.Factions[2]);
        //        Assert.AreEqual(null, level.Factions[3]);
        //        Assert.AreEqual(null, level.Factions[4]);
        //        Assert.AreEqual(null, level.Factions[5]);
        //        Assert.AreEqual(null, level.Factions[6]);
        //        Assert.AreEqual(null, level.Factions[7]);

        //        Assert.AreNotEqual(null, level.Items);
        //        Assert.AreEqual(2, level.Items.Count);
        //        Assert.AreNotEqual(null, level.Map);
        //        Assert.AreEqual(LevelMode.Draw, level.Mode);
        //        Assert.AreEqual(0, level.Round);
        //        Assert.AreNotEqual(null, level.Settings);

        //        Assert.AreEqual(0, removedItemCount);
        //        Assert.AreEqual(1, OnUpdateCount);
        //        Assert.AreEqual(0, OnRemoveItemCount);
        //        Assert.AreEqual(2, OnInsertItemCount);
        //        Assert.AreEqual(1, OnInitCount);
        //        Assert.AreEqual(1, GetMapCount); // Call durch Init
        //        Assert.AreEqual(1, DoSettingsCount);
        //        Assert.AreEqual(2, AddedItemCount);
        //    }

        //    #endregion

        //    #region OnInsert (Init) Tests

        //    // --- INIT ---
        //    // ThrowException (fail)
        //    // CallInit (fail)
        //    // CallNextState (fail)
        //    // CallFinish0 (fail)
        //    // CallFinish2 (fail)
        //    // CallFail0 (fail)
        //    // CallFail2 (fail)
        //    // CallDraw (fail)
        //    // CallAddScreenHighlight (muss gehen)
        //    // CallGetMap (muss gehen)
        //    // CallInsertItem (muss gehen)
        //    // CallRemoveItemWithoutAdd (muss gehen)
        //    // CallRemoveItemWithAdd (muss gehen)
        //    // CallRegisterTrigger (muss gehen)
        //    // CallUnregisterTrigger (muss gehen)


        //    /// <summary>
        //    /// OnInsertItem (Init) -> Exception (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInsertItemInInitWithException()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInsertItemAction = DebugLevelAction.ThrowException;

        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotImplementedException) { }

        //        Assert.AreEqual(LevelMode.FailedPlayer1, level.Mode);
        //    }

        //    /// <summary>
        //    /// OnInsertItem (Init) -> Init (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInsertItemInInitWithInit()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInsertItemAction = DebugLevelAction.CallInit;

        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(LevelMode.FailedPlayer1, level.Mode);
        //    }

        //    /// <summary>
        //    /// OnInsertItem (Init) -> NextState (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInsertItemInInitWithNextState()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInsertItemAction = DebugLevelAction.CallNextState;

        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(LevelMode.FailedPlayer1, level.Mode);
        //    }

        //    /// <summary>
        //    /// OnInsertItem (Init) -> Fail0 (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInsertItemInInitWithFail0()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInsertItemAction = DebugLevelAction.CallFail0;

        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(LevelMode.FailedPlayer1, level.Mode);
        //    }

        //    /// <summary>
        //    /// OnInsertItem (Init) -> Finish0 (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInsertItemInInitWithFinish0()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInsertItemAction = DebugLevelAction.CallFinish0;

        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(LevelMode.FailedPlayer1, level.Mode);
        //    }

        //    /// <summary>
        //    /// OnInsertItem (Init) -> Draw (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInsertItemInInitWithDraw()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInsertItemAction = DebugLevelAction.CallDraw;

        //        try
        //        {
        //            level.Init(f1);
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(LevelMode.FailedPlayer1, level.Mode);
        //    }

        //    /// <summary>
        //    /// OnInsertItem (Init) -> AddScreenHighlight (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInsertItemInInitWithScreenHighlight()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInsertItemAction = DebugLevelAction.CallAddScreenHighlight;
        //        level.Init(f1);

        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //    }

        //    /// <summary>
        //    /// OnInsertItem (Init) -> GetMap (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInsertItemInInitWithGetMap()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInsertItemAction = DebugLevelAction.CallGetMap;
        //        level.Init(f1);

        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //    }

        //    /// <summary>
        //    /// OnInsertItem (Init) -> InsertItem (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInsertItemInInitWithInsertItem()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInsertItemAction = DebugLevelAction.CallInsertItem;
        //        level.Init(f1);

        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //    }

        //    /// <summary>
        //    /// OnInsertItem (Init) -> RemoveItem with Insert (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInsertItemInInitWithRemoveItemWithInsert()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInsertItemAction = DebugLevelAction.CallRemoveItemWithAdd;
        //        level.Init(f1);

        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //    }

        //    /// <summary>
        //    /// OnInsertItem (Init) -> RemoveItem without Insert (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInsertItemInInitWithRemoveItemWithoutInsert()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInsertItemAction = DebugLevelAction.CallRemoveItemWithoutAdd;
        //        level.Init(f1);

        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //    }

        //    /// <summary>
        //    /// OnInsertItem (Init) -> Register Trigger (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInsertItemInInitWithRegisterTrigger()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInsertItemAction = DebugLevelAction.CallRegisterTrigger;
        //        level.Init(f1);

        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //    }

        //    /// <summary>
        //    /// OnInsertItem (Init) -> Unregister Trigger (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void OnInsertItemInInitWithUnregisterTrigger()
        //    {
        //        Faction f1 = new AntFaction(typeof(RawColony), "German1", PlayerColor.Blue);

        //        level.OnInsertItemAction = DebugLevelAction.CallUnregisterTrigger;
        //        level.Init(f1);

        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //    }

        //    #endregion

        //    #region OnInsert (Update) Tests

        //    // --- UPDATE ---
        //    // ThrowException (fail)
        //    // CallAddScreenHighlight (muss gehen)
        //    // CallGetMap (muss gehen)
        //    // CallInit (fail)
        //    // CallInsertItem (muss gehen)
        //    // CallNextState (stackoverflow)
        //    // CallRemoveItemWithoutAdd (muss gehen)
        //    // CallRemoveItemWithAdd (muss gehen)
        //    // CallRegisterTrigger (muss gehen)
        //    // CallUnregisterTrigger (muss gehen)
        //    // CallFinish0 (muss gehen)
        //    // CallFinish2 (fail, weil 2 nicht existiert)
        //    // CallFail0 (muss gehen)
        //    // CallFail2 (fail, weil 2 nicht existiert)
        //    // CallDraw (muss gehen)

        //    #endregion

        //    #region MethodCalls Uninit

        //    // AddScreenHighlight (fail)
        //    // GetMap (läuft)
        //    // Init (läuft)
        //    // InsertItem (fail)
        //    // RemoveItem (fail)
        //    // NextState (fail)

        //    /// <summary>
        //    /// Uninit -> AddScreenHighlight (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void UninitCallAddScreenHighlight()
        //    {
        //        try
        //        {
        //            level.AddScreenHighlight(new ScreenNotification());
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }
        //    }

        //    /// <summary>
        //    /// Uninit -> GetMap (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void UninitCallGetMap()
        //    {
        //        Map map = level.GetMap();
        //        Assert.IsNotNull(map);
        //    }

        //    /// <summary>
        //    /// Uninit -> Init (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void UninitCallInit()
        //    {
        //        level.Init();
        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //    }

        //    /// <summary>
        //    /// Uninit -> InsertItem (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void UninitCallInsertItem()
        //    {
        //        try
        //        {
        //            level.InsertItem(new DebugGameItem());
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }
        //    }

        //    /// <summary>
        //    /// Uninit -> RemoveItem (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void UninitCallRemoveItem()
        //    {
        //        try
        //        {
        //            level.RemoveItem(new DebugGameItem());
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }
        //    }

        //    /// <summary>
        //    /// Uninit -> NextState (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void UninitCallNextState()
        //    {
        //        try
        //        {
        //            level.NextState();
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }
        //    }

        //    #endregion

        //    #region NethodCalls InitFailed

        //    // AddScreenHighlight (fail)
        //    // GetMap (läuft)
        //    // Init (fail)
        //    // InsertItem (fail)
        //    // RemoveItem (fail)
        //    // NextState (fail)

        //    /// <summary>
        //    /// InitFailed -> AddScreenHighlight (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void InitFailedCallAddScreenHighlight()
        //    {
        //        level.OnInitAction = DebugLevelAction.ThrowException;
        //        try
        //        {
        //            level.Init();
        //        }
        //        catch { }

        //        try
        //        {
        //            level.AddScreenHighlight(new ScreenMapArea());
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //    }

        //    /// <summary>
        //    /// InitFailed -> GetMap (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void InitFailedCallGetMap()
        //    {
        //        level.OnInitAction = DebugLevelAction.ThrowException;
        //        try
        //        {
        //            level.Init();
        //        }
        //        catch { }

        //        level.GetMap();
        //    }

        //    /// <summary>
        //    /// InitFailed -> Init (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void InitFailedCallInit()
        //    {
        //        level.OnInitAction = DebugLevelAction.ThrowException;
        //        try
        //        {
        //            level.Init();
        //        }
        //        catch { }

        //        try
        //        {
        //            level.Init();
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }
        //    }

        //    /// <summary>
        //    /// InitFailed -> InsertItem (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void InitFailedCallInsertItem()
        //    {
        //        level.OnInitAction = DebugLevelAction.ThrowException;
        //        try
        //        {
        //            level.Init();
        //        }
        //        catch { }
        //        try
        //        {
        //            level.InsertItem(new DebugGameItem());
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //    }

        //    /// <summary>
        //    /// InitFailed -> RemoveItem (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void InitFailedCallRemoveItem()
        //    {
        //        level.OnInitAction = DebugLevelAction.ThrowException;
        //        try
        //        {
        //            level.Init();
        //        }
        //        catch { }
        //        try
        //        {
        //            level.RemoveItem(new DebugGameItem());
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //    }

        //    /// <summary>
        //    /// InitFailed -> NextState (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void InitFailedCallNextState()
        //    {
        //        level.OnInitAction = DebugLevelAction.ThrowException;
        //        try
        //        {
        //            level.Init();
        //        }
        //        catch { }
        //        try
        //        {
        //            level.NextState();
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //    }

        //    #endregion

        //    #region MethodCalls Running

        //    // AddScreenHighlight (läuft)
        //    // GetMap (läuft)
        //    // Init (fail)
        //    // InsertItem (läuft)
        //    // RemoveItem (läuft)
        //    // NextState (läuft)

        //    /// <summary>
        //    /// Running -> AddScreenHighlight (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void RunningCallAddScreenHighlight()
        //    {
        //        level.Init();

        //        level.AddScreenHighlight(new ScreenNotification());
        //    }

        //    /// <summary>
        //    /// Running -> GetMap (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void RunningCallGetMap()
        //    {
        //        level.Init();

        //        Map map = level.GetMap();
        //        Assert.IsNotNull(map);
        //    }

        //    /// <summary>
        //    /// Running -> Init (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void RunningCallInit()
        //    {
        //        level.Init();

        //        try
        //        {
        //            level.Init();
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }

        //        Assert.AreEqual(LevelMode.Running, level.Mode);
        //    }

        //    /// <summary>
        //    /// Running -> InsertItem (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void RunningCallInsertItem()
        //    {
        //        level.Init();
        //        level.InsertItem(new DebugGameItem());
        //    }

        //    /// <summary>
        //    /// Running -> RemoveItem (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void RunningCallRemoveItem()
        //    {
        //        level.Init();
        //        level.RemoveItem(new DebugGameItem());
        //    }

        //    /// <summary>
        //    /// Running -> NextState (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void RunningCallNextState()
        //    {
        //        level.Init();
        //        level.NextState();
        //    }

        //    #endregion

        //    #region MethodCalls Finished

        //    // AddScreenHighlight (fail)
        //    // GetMap (läuft)
        //    // Init (fail)
        //    // InsertItem (fail)
        //    // RemoveItem (fail)
        //    // NextState (läuft)

        //    /// <summary>
        //    /// Finished -> AddScreenHighlight (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void FinishedCallAddScreenHighlight()
        //    {
        //        level.Init();
        //        level.OnUpdateAction = DebugLevelAction.CallDraw;
        //        level.NextState();
        //        level.OnUpdateAction = DebugLevelAction.DoNothing;

        //        try
        //        {
        //            level.AddScreenHighlight(new ScreenNotification());
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }
        //        Assert.AreEqual(LevelMode.Draw, level.Mode);
        //    }

        //    /// <summary>
        //    /// Finished -> GetMap (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void FinishedCallGetMap()
        //    {
        //        level.Init();
        //        level.OnUpdateAction = DebugLevelAction.CallDraw;
        //        level.NextState();
        //        level.OnUpdateAction = DebugLevelAction.DoNothing;

        //        Map map = level.GetMap();
        //        Assert.IsNotNull(map);
        //        Assert.AreEqual(LevelMode.Draw, level.Mode);
        //    }

        //    /// <summary>
        //    /// Finished -> Init (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void FinishedCallInit()
        //    {
        //        level.Init();
        //        level.OnUpdateAction = DebugLevelAction.CallDraw;
        //        level.NextState();
        //        level.OnUpdateAction = DebugLevelAction.DoNothing;

        //        try
        //        {
        //            level.InsertItem(new DebugGameItem());
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }
        //        Assert.AreEqual(LevelMode.Draw, level.Mode);
        //    }

        //    /// <summary>
        //    /// Finished -> InsertItem (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void FinishedCallInsertItem()
        //    {
        //        level.Init();
        //        level.OnUpdateAction = DebugLevelAction.CallDraw;
        //        level.NextState();
        //        level.OnUpdateAction = DebugLevelAction.DoNothing;

        //        try
        //        {
        //            level.InsertItem(new DebugGameItem());
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }
        //        Assert.AreEqual(LevelMode.Draw, level.Mode);
        //    }

        //    /// <summary>
        //    /// Finished -> RemoveItem (fail)
        //    /// </summary>
        //    [TestMethod]
        //    public void FinishedCallRemoveItem()
        //    {
        //        level.Init();
        //        level.OnUpdateAction = DebugLevelAction.CallDraw;
        //        level.NextState();
        //        level.OnUpdateAction = DebugLevelAction.DoNothing;

        //        try
        //        {
        //            level.RemoveItem(new DebugGameItem());
        //            Assert.Fail("Should throw Exception");
        //        }
        //        catch (NotSupportedException) { }
        //        Assert.AreEqual(LevelMode.Draw, level.Mode);
        //    }

        //    /// <summary>
        //    /// Finished -> NextState (läuft)
        //    /// </summary>
        //    [TestMethod]
        //    public void FinishedCallNextState()
        //    {
        //        level.Init();
        //        level.OnUpdateAction = DebugLevelAction.CallDraw;
        //        MainState state = level.NextState();
        //        level.OnUpdateAction = DebugLevelAction.DoNothing;

        //        int round = state.Round;

        //        state = level.NextState();
        //        Assert.AreEqual(LevelMode.Draw, level.Mode);
        //        Assert.AreEqual(round, state.Round);
        //    }

        //    #endregion
        //}

        //#region Helper Levels

        //internal class LevelNoAttribute : Level
        //{
        //    public override Guid Guid
        //    {
        //        get { return Guid.NewGuid(); }
        //    }

        //    public override string Name
        //    {
        //        get { return "LevelNoAttribute"; }
        //    }

        //    public override string Description
        //    {
        //        get { return "LevelNoAttribute Description"; }
        //    }
        //}

        //[LevelDescription(MinPlayerCount = 1)]
        //internal class LevelMinPlayer1 : Level
        //{
        //    public override Guid Guid
        //    {
        //        get { return Guid.NewGuid(); }
        //    }

        //    public override string Name
        //    {
        //        get { return "LevelMinPlayer1"; }
        //    }

        //    public override string Description
        //    {
        //        get { return "LevelMinPlayer1 Description"; }
        //    }
        //}

        //[LevelDescription(MaxPlayerCount = 1)]
        //internal class LevelMaxPlayer1 : Level
        //{
        //    public override Guid Guid
        //    {
        //        get { return Guid.NewGuid(); }
        //    }

        //    public override string Name
        //    {
        //        get { return "LevelMaxPlayer1"; }
        //    }

        //    public override string Description
        //    {
        //        get { return "LevelMaxPlayer1 Description"; }
        //    }
        //}

        //[LevelDescription]
        //[FactionFilter(PlayerIndex = 0, FactionType = typeof(BugFaction))]
        //internal class LevelFactionFilterOnlyBugOn1 : Level
        //{
        //    public override Guid Guid
        //    {
        //        get { return Guid.NewGuid(); }
        //    }

        //    public override string Name
        //    {
        //        get { return "LevelFactionFilterOnlyBugOn1"; }
        //    }

        //    public override string Description
        //    {
        //        get { return "LevelFactionFilterOnlyBugOn1 Description"; }
        //    }
        //}

        //[LevelDescription]
        //[FactionFilter(PlayerIndex = 0, FactionType = typeof(BugFaction))]
        //[FactionFilter(PlayerIndex = 0, FactionType = typeof(AntFaction))]
        //internal class LevelFactionFilterBugAndAntOn1 : Level
        //{
        //    public override Guid Guid
        //    {
        //        get { return Guid.NewGuid(); }
        //    }

        //    public override string Name
        //    {
        //        get { return "LevelFactionFilterBugAndAntOn1"; }
        //    }

        //    public override string Description
        //    {
        //        get { return "LevelFactionFilterBugAndAntOn1 Description"; }
        //    }
        //}

        //[LevelDescription]
        //[FactionFilter(PlayerIndex = 0, FactionType = typeof(BugFaction))]
        //[FactionFilter(PlayerIndex = 1, FactionType = typeof(AntFaction))]
        //internal class LevelFactionFilterBugOn1AndAntOn2 : Level
        //{
        //    public override Guid Guid
        //    {
        //        get { return Guid.NewGuid(); }
        //    }

        //    public override string Name
        //    {
        //        get { return "LevelFactionFilterBugAndAntOn1"; }
        //    }

        //    public override string Description
        //    {
        //        get { return "LevelFactionFilterBugAndAntOn1 Description"; }
        //    }
        //}

        //#endregion
    }
}
