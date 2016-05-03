using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AntMe.Simulation.Factions.Ants;
using AntMe.Simulation.Items;
using AntMe.Simulation.Factions.Bugs;
using AntMe.Debug;
using AntMe.Core;

namespace AntMe.Simulation.Debug
{
    public partial class StateRenderer : PlaygroundRenderer
    {
        private MainState _state;

        public StateRenderer()
        {
            InitializeComponent();
        }

        public void SetMainState(MainState state)
        {
            if (_state == null)
            {
                SetScale(state.Map.GetCellCount());
            }
            _state = state;
        }

        protected override void RequestDraw()
        {
            MainState _currentState = _state;
            if (_currentState != null)
            {
                Index2 cells = _currentState.Map.GetCellCount();
                DrawPlayground(cells, _currentState.Map.Tiles);

                foreach (var item in _currentState.Items)
                {
                    if (item is AnthillState)
                    {
                        AnthillState anthillState = item as AnthillState;
                        DrawItem(item.Id, item.Position, anthillState.Radius, null, null, Color.Brown, null, null, null, null, null, null);
                    }

                    if (item is AntState)
                    {
                        AntState antState = item as AntState;
                        // DrawItem(item.Id, item.Position, antState.Radius, Color.Black);
                    }

                    if (item is SugarState)
                    {
                        SugarState sugarState = item as SugarState;
                        // DrawItem(item.Id, item.Position, sugarState.Radius, Color.White);
                    }

                    if (item is AppleState)
                    {
                        AppleState appleState = item as AppleState;
                        // DrawItem(item.Id, item.Position, 5, Color.LightGreen);
                    }

                    if (item is MarkerState)
                    {
                        MarkerState markerState = item as MarkerState;
                        // DrawItem(item.Id, item.Position, 10, Color.Yellow);
                    }

                    if (item is BugState)
                    {
                        // DrawItem(item.Id, item.Position, 10, Color.Blue);
                    }
                }
            }
        }

        protected override void NextRound()
        {
        }
    }
}
