using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AntMe.Simulation.Test
{
    /// <summary>
    /// Testet die Funktionalität der Faction-Klasse.
    /// Autor: Tom Wendel
    /// Status: TODOs
    /// </summary>
    [TestClass]
    public class FactionTest
    {
        //// TODO: Init
        //// TODO: whatever
        //DebugLevel level;
        //DebugFaction faction1;
        //DebugFactionSettings settings1;
        //DebugFaction faction2;
        //DebugFactionSettings settings2;

        //#region Init

        //[TestInitialize]
        //public void Init()
        //{
        //    level = new DebugLevel();

        //    settings1 = new DebugFactionSettings();
        //    settings1.CheckSettingsCall += settings_CheckSettingsCall;
            
        //    faction1 = new DebugFaction(settings1, "Test1", PlayerColor.Black);
        //    faction1.GetStateCall += faction_GetStateCall;
        //    faction1.InitCall += faction_InitCall;
        //    faction1.UpdateCall += faction_UpdateCall;
        //}

        //[TestCleanup]
        //public void Cleanup()
        //{
        //    if (settings1 != null) 
        //    {
        //        settings1.CheckSettingsCall -= settings_CheckSettingsCall;
        //        settings1 = null;
        //    }

        //    if (faction1 != null)
        //    {
        //        faction1.GetStateCall -= faction_GetStateCall;
        //        faction1.InitCall -= faction_InitCall;
        //        faction1.UpdateCall -= faction_UpdateCall;
        //        faction1 = null;
        //    }

        //    settings2 = null;
        //    faction2 = null;
        //}

        //private void CreateSecond()
        //{
        //    settings2 = new DebugFactionSettings();
        //    faction2 = new DebugFaction(settings2, "Test2", PlayerColor.Black);
        //}

        //#endregion

        //#region Eventhandler

        //int UpdateCount = 0;
        //int InitCount = 0;
        //int GetStateCount = 0;
        //int CheckSettingsCount = 0;

        //void faction_UpdateCall(int round)
        //{
        //    UpdateCount++;
        //}

        //void faction_InitCall()
        //{
        //    InitCount++;
        //}

        //void faction_GetStateCall()
        //{
        //    GetStateCount++;
        //}

        //void settings_CheckSettingsCall()
        //{
        //    CheckSettingsCount++;
        //}

        //private void ResetCounter()
        //{
        //    UpdateCount = 0;
        //    InitCount = 0;
        //    GetStateCount = 0;
        //    CheckSettingsCount = 0;
        //}

        //#endregion

        ///// <summary>
        ///// Prüft die Eigenschaften vor der Initialisierung.
        ///// </summary>
        //[TestMethod]
        //public void CheckPropertiesBeforeInit()
        //{
        //    Assert.AreEqual(Vector2.Zero, faction1.Home);
        //    Assert.AreEqual(null, faction1.Level);
        //    Assert.AreEqual(0, faction1.PlayerIndex);
        //    Assert.AreEqual(null, faction1.Random);
        //    Assert.AreEqual(settings1, faction1.Settings);
        //}
    }
}
