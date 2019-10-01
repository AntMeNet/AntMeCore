namespace AntMe.Levelpack.Levels
{
    //[LevelDescription(
    //    "{5C5A40C3-7196-4F1D-9BDF-D3C954CC2491}",
    //    typeof(Map),
    //    "The Maze Race",
    //    "[UGKA] Ein kleines Rennen durch das Labyrinth. Wer ist am schnellsten am Zielort und wessen Ameisen haben den besten Orientierungssinn?",
    //    Hidden = false,
    //    MinPlayerCount = 2,
    //    MaxPlayerCount = 2
    //)]
    //public sealed class MazeRace : Level
    //{
    //    public MazeRace(ITypeResolver resolver) : base(resolver) { }

    //    public Map GetMap()
    //    {
    //        using (MemoryStream stream = new MemoryStream(AntMe.Levelpack.Properties.Resources.maze))
    //        {
    //            return Map.Deserialize(stream);
    //        }
    //    }

    //    protected override void OnInit()
    //    {
    //        var triggerProp = GetProperty<TriggerLevelProperty>();

    //        var trigger = new AreaTrigger()
    //        {
    //            LowerRight = new Vector2(),
    //            UpperLeft = new Vector2()
    //        };
    //        trigger.OnItemTrapped += trigger_OnItemTrapped;
    //        trigger.Enabled = true;

    //        triggerProp.RegisterTrigger(trigger);
    //    }

    //    void trigger_OnItemTrapped(ITrigger trigger, Item param)
    //    {
    //        if (param is FactionItem)
    //        {
    //            Finish((param as FactionItem).Faction.SlotIndex);
    //        }
    //    }
    //}
}