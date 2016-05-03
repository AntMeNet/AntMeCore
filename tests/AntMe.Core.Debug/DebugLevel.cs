using AntMe.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Simulation.Debug
{
    [LevelDescription(
        MinPlayerCount = 0, 
        MaxPlayerCount = 8, 
        Hidden = false
    )]
    public class DebugLevel : Level
    {
        public override Guid Guid { get { return Guid.Parse("{2B362702-DEF0-4CC0-B9CB-D61D299C4276}"); } }

        public override string Name { get { return "Debug Level"; } }

        public override string Description { get { return "Debug Level Description"; } }

        public DebugLevelAction DoSettingsAction { get; set; }
        public DebugLevelAction GetMapAction { get; set; }
        public DebugLevelAction OnInitAction { get; set; }
        public DebugLevelAction OnInsertItemAction { get; set; }
        public DebugLevelAction OnRemoveItemAction { get; set; }
        public DebugLevelAction OnUpdateAction { get; set; }

        protected override void DoSettings()
        {
            if (DoSettingsCall != null)
                DoSettingsCall(this);

            Do(DoSettingsAction);
        }

        public override Core.Map GetMap()
        {
            if (GetMapCall != null)
                GetMapCall(this);

            Do(GetMapAction);

            return base.GetMap();
        }

        protected override void OnInit()
        {
            if (OnInitCall != null)
                OnInitCall(this);

            Do(OnInitAction);
        }

        protected override void OnInsertItem(GameItem item)
        {
            if (OnInsertItemCall != null)
                OnInsertItemCall(this, item);

            Do(OnInsertItemAction);
        }

        protected override void OnRemoveItem(GameItem item)
        {
            if (OnRemoveItemCall != null)
                OnRemoveItemCall(this, item);

            Do(OnRemoveItemAction);
        }

        protected override void OnUpdate()
        {
            if (OnUpdateCall != null)
                OnUpdateCall(this);

            Do(OnUpdateAction);
        }

        private ScreenHighlight highlight = new ScreenDialog();
        private GameItem gameItem = new DebugGameItem();
        private ITrigger trigger = new AreaTrigger();

        private void Do(DebugLevelAction call)
        {
            switch (call)
            {
                case DebugLevelAction.ThrowException:
                    throw new NotImplementedException();
                case DebugLevelAction.CallAddScreenHighlight:
                    this.AddScreenHighlight(highlight);
                    break;
                case DebugLevelAction.CallGetMap:
                    this.GetMap();
                    break;
                case DebugLevelAction.CallInit:
                    this.Init(null);
                    break;
                case DebugLevelAction.CallInsertItem:
                    this.InsertItem(gameItem);
                    break;
                case DebugLevelAction.CallNextState:
                    this.NextState();
                    break;
                case DebugLevelAction.CallRemoveItemWithoutAdd:
                    this.RemoveItem(gameItem);
                    break;
                case DebugLevelAction.CallRemoveItemWithAdd:
                    this.InsertItem(gameItem);
                    this.RemoveItem(gameItem);
                    break;
                case DebugLevelAction.CallRegisterTrigger:
                    this.RegisterTrigger(trigger);
                    break;
                case DebugLevelAction.CallUnregisterTrigger:
                    this.UnregisterTrigger(trigger);
                    break;
                case DebugLevelAction.CallFinish0:
                    this.Finish(0);
                    break;
                case DebugLevelAction.CallFinish2:
                    this.Finish(2);
                    break;
                case DebugLevelAction.CallFail0:
                    this.Fail(0);
                    break;
                case DebugLevelAction.CallFail2:
                    this.Fail(2);
                    break;
                case DebugLevelAction.CallDraw:
                    this.Draw();
                    break;
            }
        }

        public delegate void OnNothingDelegate(DebugLevel level);
        public delegate void OnItemDelegate(DebugLevel level, GameItem item);

        public event OnNothingDelegate DoSettingsCall;
        public event OnNothingDelegate GetMapCall;
        public event OnNothingDelegate OnInitCall;
        public event OnItemDelegate OnInsertItemCall;
        public event OnItemDelegate OnRemoveItemCall;
        public event OnNothingDelegate OnUpdateCall;
    }

    public enum DebugLevelAction
    {
        DoNothing,
        ThrowException,
        CallAddScreenHighlight,
        CallGetMap,
        CallInit,
        CallInsertItem,
        CallNextState,
        CallRemoveItemWithoutAdd,
        CallRemoveItemWithAdd,
        CallRegisterTrigger,
        CallUnregisterTrigger,
        CallFinish0,
        CallFinish2,
        CallFail0,
        CallFail2,
        CallDraw,
    }
}
