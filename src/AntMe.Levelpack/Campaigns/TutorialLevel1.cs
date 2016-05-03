namespace AntMe.Levelpack.Campaigns
{
    //[LevelDescription(
    //    "{1A3D2B7D-B10C-48BF-8728-8689DDC39E8F}", 
    //    typeof(Map), 
    //    "Tutorial Level 1", 
    //    "In diesem Level gehts jetzt erstmal darum, seine Ameise ein paar Schritte in eine Richtung zu lenken",
    //    Hidden = true,
    //    MinPlayerCount = 1,
    //    MaxPlayerCount = 1
    //)]
    //public sealed class TutorialLevel1 : Level
    //{
    //    public TutorialLevel1(ITypeResolver resolver) : base(resolver) { }

    //    protected override void OnInit()
    //    {
    //        var triggerProp = GetProperty<TriggerLevelProperty>();

    //        // Oben
    //        AreaTrigger trigger = new AreaTrigger()
    //        {
    //            UpperLeft = new Vector2(0, 0),
    //            LowerRight = new Vector2(60, 10)
    //        };
    //        trigger.OnItemTrapped += trigger_OnItemTrapped;
    //        triggerProp.RegisterTrigger(trigger);

    //        // Unten
    //        trigger = new AreaTrigger()
    //        {
    //            UpperLeft = new Vector2(0, 50),
    //            LowerRight = new Vector2(60, 60)
    //        };
    //        trigger.OnItemTrapped += trigger_OnItemTrapped;
    //        triggerProp.RegisterTrigger(trigger);

    //        // Links
    //        trigger = new AreaTrigger()
    //        {
    //            UpperLeft = new Vector2(0, 0),
    //            LowerRight = new Vector2(10, 60)
    //        };
    //        trigger.OnItemTrapped += trigger_OnItemTrapped;
    //        triggerProp.RegisterTrigger(trigger);

    //        // Rechts
    //        trigger = new AreaTrigger()
    //        {
    //            UpperLeft = new Vector2(50, 0),
    //            LowerRight = new Vector2(60, 60)
    //        };
    //        trigger.OnItemTrapped += trigger_OnItemTrapped;
    //        triggerProp.RegisterTrigger(trigger);

    //        // Timer
    //        TimerTrigger timer = new TimerTrigger(200);
    //        timer.OnTimeReached += timer_OnTimeReached;
    //        triggerProp.RegisterTrigger(timer);
    //    }

    //    void trigger_OnItemTrapped(ITrigger trigger, Item param)
    //    {
    //        Finish(0);
    //    }

    //    void timer_OnTimeReached(ITrigger trigger)
    //    {
    //        Fail(0);
    //    }

    //}
}
