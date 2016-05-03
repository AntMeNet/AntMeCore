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

        private TreeNode interopNode;
        private TreeNode factoryInteropPropertiesNode;
        private TreeNode factoryInteropAttachmentPropertiesNode;
        private TreeNode factoryInteropExtenderNode;
        private TreeNode UnitInteropPropertiesNode;
        private TreeNode UnitInteropAttachmentPropertiesNode;
        private TreeNode UnitInteropExtenderNode;
        
        private TreeNode levelNode;
        private TreeNode levelPropertiesNode;
        private TreeNode levelExtenderNode;

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

            interopNode = treeView.Nodes["interopNode"];
            factoryInteropPropertiesNode = interopNode.Nodes["factoryInteropPropertiesNode"];
            factoryInteropAttachmentPropertiesNode = interopNode.Nodes["factoryInteropAttachmentPropertiesNode"];
            factoryInteropExtenderNode = interopNode.Nodes["factoryInteropExtenderNode"];
            UnitInteropPropertiesNode = interopNode.Nodes["UnitInteropPropertiesNode"];
            UnitInteropAttachmentPropertiesNode = interopNode.Nodes["UnitInteropAttachmentPropertiesNode"];
            UnitInteropExtenderNode = interopNode.Nodes["UnitInteropExtenderNode"];

            levelNode = treeView.Nodes["levelNode"];
            levelPropertiesNode = levelNode.Nodes["levelPropertiesNode"];
            levelExtenderNode = levelNode.Nodes["levelExtenderNode"];
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
                else if (treeView.SelectedNode == factoryInteropPropertiesNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.FactoryInteropProperties)
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
                else if (treeView.SelectedNode == UnitInteropPropertiesNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.UnitInteropProperties)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == UnitInteropAttachmentPropertiesNode)
                {
                    foreach (var item in ExtensionLoader.DefaultTypeMapper.UnitInteropAttachments)
                    {
                        var node = listView.Items.Add(item.Name);
                        node.SubItems.Add(item.Type.FullName);
                        node.SubItems.Add(item.ExtensionPack.Name);
                    }
                }
                else if (treeView.SelectedNode == UnitInteropExtenderNode)
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
            }
        }
    }
}
