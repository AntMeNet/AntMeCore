using AntMe.Runtime;
using System.Windows.Forms;

namespace CoreTestClient
{
    public partial class ExtensionsForm : Form
    {
        private TreeNode extensionPacksNode;
        private TreeNode enginePropertiesNode;

        private TreeNode itemsNode;
        private TreeNode itemPropertiesNode;
        private TreeNode itemAttachmentPropertiesNode;
        private TreeNode itemExtenderNode;

        private TreeNode factionsNode;
        private TreeNode factionPropertiesNode;
        private TreeNode factionAttachmentPropertiesNode;
        private TreeNode factionExtenderNode;

        private TreeNode factoryInteropNode;
        private TreeNode factoryInteropAttachmentPropertiesNode;
        private TreeNode factoryInteropExtenderNode;
        private TreeNode unitInteropNode;
        private TreeNode unitInteropAttachmentPropertiesNode;
        private TreeNode unitInteropExtenderNode;
        
        private TreeNode levelNode;
        private TreeNode levelPropertiesNode;
        private TreeNode levelExtenderNode;

        private TreeNode mapNode;
        private TreeNode mapPropertiesNode;
        private TreeNode mapExtenderNode;

        private TreeNode materialsNode;

        private TreeNode mapTilesNode;
        private TreeNode mapTilePropertiesNode;
        private TreeNode mapTileAttachmentPropertiesNode;
        private TreeNode mapTileExtenderNode;


        public ExtensionsForm()
        {
            InitializeComponent();

            // Collecting Nodes
            extensionPacksNode = treeView.Nodes["extensionPacksNode"];
            enginePropertiesNode = treeView.Nodes["enginePropertiesNode"];

            itemsNode = treeView.Nodes["itemsNode"];
            itemPropertiesNode = itemsNode.Nodes["itemPropertiesNode"];
            itemAttachmentPropertiesNode = itemsNode.Nodes["itemAttachmentPropertiesNode"];
            itemExtenderNode = itemsNode.Nodes["itemExtenderNode"];

            factionsNode = treeView.Nodes["factionsNode"];
            factionPropertiesNode = factionsNode.Nodes["factionPropertiesNode"];
            factionAttachmentPropertiesNode = factionsNode.Nodes["factionAttachmentPropertiesNode"];
            factionExtenderNode = factionsNode.Nodes["factionExtenderNode"];

            factoryInteropNode = treeView.Nodes["factoryInteropNode"];
            factoryInteropAttachmentPropertiesNode = factoryInteropNode.Nodes["factoryInteropAttachmentPropertiesNode"];
            factoryInteropExtenderNode = factoryInteropNode.Nodes["factoryInteropExtenderNode"];

            unitInteropNode = treeView.Nodes["unitInteropNode"];
            unitInteropAttachmentPropertiesNode = unitInteropNode.Nodes["UnitInteropAttachmentPropertiesNode"];
            unitInteropExtenderNode = unitInteropNode.Nodes["UnitInteropExtenderNode"];

            levelNode = treeView.Nodes["levelNode"];
            levelPropertiesNode = levelNode.Nodes["levelPropertiesNode"];
            levelExtenderNode = levelNode.Nodes["levelExtenderNode"];

            mapNode = treeView.Nodes["mapNode"];
            mapPropertiesNode = mapNode.Nodes["mapPropertiesNode"];
            mapExtenderNode = mapNode.Nodes["mapExtenderNode"];

            materialsNode = treeView.Nodes["materialsNode"];

            mapTilesNode = treeView.Nodes["mapTilesNode"];
            mapTilePropertiesNode = mapTilesNode.Nodes["mapTilePropertiesNode"];
            mapTileAttachmentPropertiesNode = mapTilesNode.Nodes["mapTileAttachmentPropertiesNode"];
            mapTileExtenderNode = mapTilesNode.Nodes["mapTileExtenderNode"];
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listView.Items.Clear();
            if (treeView.SelectedNode != null)
            {
                if (treeView.SelectedNode == extensionPacksNode)
                {
                    foreach (var item in ExtensionLoader.ExtensionPacks)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add("");
                        node.SubItems.Add(item.Name);
                    }
                }
                else if (treeView.SelectedNode == enginePropertiesNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.EngineProperties)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == itemsNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.Items)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == itemPropertiesNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.ItemProperties)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == itemAttachmentPropertiesNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.ItemAttachments)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == itemExtenderNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.ItemExtender)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == factionsNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.Factions)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == factionPropertiesNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.FactionProperties)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == factionAttachmentPropertiesNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.FactionAttachments)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == factionExtenderNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.FactionExtender)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == factoryInteropAttachmentPropertiesNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.FactoryInteropAttachments)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == factoryInteropExtenderNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.FactoryInteropExtender)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == unitInteropAttachmentPropertiesNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.UnitInteropAttachments)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == unitInteropExtenderNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.UnitInteropExtender)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == levelPropertiesNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.LevelProperties)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == levelExtenderNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.LevelExtender)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == mapPropertiesNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.MapProperties)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == mapExtenderNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.MapExtender)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == materialsNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.MapMaterials)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == mapTilesNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.MapTiles)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == mapTilePropertiesNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.MapTileProperties)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == mapTileAttachmentPropertiesNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.MapTileAttachments)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == mapTileExtenderNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.MapTileExtender)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
            }
        }
    }
}
