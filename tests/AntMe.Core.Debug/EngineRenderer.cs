using System.Drawing;
using System.Windows.Forms;
using AntMe.Debug;
using AntMe.ItemProperties.Basics;

namespace AntMe.Core.Debug
{
    public partial class EngineRenderer : PlaygroundRenderer
    {
        private Engine _engine;

        public EngineRenderer()
        {
            InitializeComponent();
        }

        public void SetEngine(Engine engine)
        {
            _engine = engine;

            if (engine != null)
            {
                SetScale(engine.Map.GetCellCount());
            }

            FillTree();
        }

        private void FillTree()
        {
            ItemTree.Nodes.Clear();

            if (_engine != null)
            {
                TreeNode root = new TreeNode("Engine");
                root.Tag = _engine;
                ItemTree.Nodes.Add(root);

                TreeNode mapNode = new TreeNode("Map");
                mapNode.Tag = _engine.Map;
                root.Nodes.Add(mapNode);

                TreeNode itemsNode = new TreeNode("Items");
                root.Nodes.Add(itemsNode);

                foreach (var item in _engine.Items)
                {
                    TreeNode itemNode = new TreeNode(item.ToString());
                    itemNode.Tag = item;
                    itemsNode.Nodes.Add(itemNode);

                    foreach (var prop in item)
                    {
                        TreeNode propNode = new TreeNode(prop.ToString());
                        propNode.Tag = prop;
                        itemNode.Nodes.Add(propNode);
                    }
                }
            }
        }

        protected override void RequestDraw()
        {
            if (_engine != null)
            {
                DrawPlayground(_engine.Map.GetCellCount(), _engine.Map.Tiles);

                foreach (var item in _engine.Items)
                {
                    float? bodySpeed = null;
                    float? bodyDirection = null;
                    float? bodyRadius = null;
                    float? smellableRadius = null;
                    float? viewRange = null;
                    float? viewAngle = null;
                    float? viewDirection = null;
                    float? attackRange = null;

                    // Movement
                    if (item.ContainsProperty<WalkingProperty>())
                    {
                        WalkingProperty prop = item.GetProperty<WalkingProperty>();
                        bodySpeed = prop.Speed;
                        bodyDirection = prop.Direction;
                    }

                    // Kollisionsradius
                    if (item.ContainsProperty<CollidableProperty>())
                    {
                        CollidableProperty prop = item.GetProperty<CollidableProperty>();
                        bodyRadius = prop.CollisionRadius;
                    }

                    // Sicht
                    if (item.ContainsProperty<SightingProperty>())
                    {
                        SightingProperty prop = item.GetProperty<SightingProperty>();
                        viewRange = prop.ViewRange;
                        viewDirection = prop.ViewDirection.Radian;
                        viewAngle = prop.ViewAngle;
                    }

                    // Attack
                    if (item.ContainsProperty<AttackerProperty>())
                    {
                        AttackerProperty prop = item.GetProperty<AttackerProperty>();
                        attackRange = prop.AttackRange;
                    }

                    // Stinkender radius
                    if (item.ContainsProperty<SmellableProperty>())
                    {
                        SmellableProperty prop = item.GetProperty<SmellableProperty>();
                        smellableRadius = prop.SmellableRadius;
                    }

                    DrawItem(item.Id, item.Position, 
                        bodyRadius, bodyDirection, bodySpeed, Color.White, 
                        viewRange, viewDirection, viewAngle, 
                        attackRange, smellableRadius, null);
                }
            }
        }

        protected override void NextRound()
        {
            if (_engine != null)
            {
                //_engine.Update();
                //Redraw(_engine.Round, _engine.Items.Count);
            }
        }
    }
}
