using System.Windows.Forms;

namespace AntMe.Runtime.Debug
{
    public partial class LoaderPreview : UserControl
    {
        TreeNode extensionPacksNode;
        TreeNode engineExtensionsNode;
        TreeNode propertiesNode;
        TreeNode rootNode;
        TreeNode itemsNode;
        TreeNode levelsNode;
        TreeNode campaignsNode;
        TreeNode factionsNode;
        TreeNode messagesNode;
        TreeNode playersNode;
        
        LoaderInfo info;

        public LoaderPreview()
        {
            InitializeComponent();

            rootNode = treeView.Nodes["rootNode"];
            extensionPacksNode = rootNode.Nodes["extensionPacksNode"];
            engineExtensionsNode = rootNode.Nodes["engineExtensionsNode"];
            propertiesNode = rootNode.Nodes["propertiesNode"];
            itemsNode = rootNode.Nodes["itemsNode"];
            levelsNode = rootNode.Nodes["levelsNode"];
            campaignsNode = rootNode.Nodes["campaignsNode"];
            factionsNode = rootNode.Nodes["factionsNode"];
            messagesNode = rootNode.Nodes["messagesNode"];
            playersNode = rootNode.Nodes["playersNode"];

            SetLoaderInfo(null);
        }

        public void SetLoaderInfo(LoaderInfo info)
        {
            rootNode.Collapse(false);
            itemsNode.Nodes.Clear();
            levelsNode.Nodes.Clear();
            campaignsNode.Nodes.Clear();
            factionsNode.Nodes.Clear();
            messagesNode.Nodes.Clear();
            playersNode.Nodes.Clear();

            if (info != null)
            {
                if (info.Campaigns != null)
                {
                    foreach (var item in info.Campaigns)
                    {
                        TreeNode node = campaignsNode.Nodes.Add(item.Name);
                        node.Tag = item;
                    }
                    campaignsNode.Expand();
                }

                if (info.Levels != null)
                {
                    foreach (var item in info.Levels)
                    {
                        TreeNode node = levelsNode.Nodes.Add(item.LevelDescription.Name);
                        node.Tag = item;
                    }
                    levelsNode.Expand();
                }

                if (info.Errors != null)
                {
                    foreach (var item in info.Errors)
                    {
                        TreeNode node = messagesNode.Nodes.Add(item.Message);
                        node.Tag = item;
                    }
                    messagesNode.Expand();
                }
                rootNode.Expand();

                if (info.Players != null)
                {
                    foreach (var item in info.Players)
                    {
                        TreeNode node = playersNode.Nodes.Add(item.Name);
                        node.Tag = item;
                    }
                    playersNode.Expand();
                }
            }
        }

        public void Refresh(TypeMapper typeMapper)
        {
            // Extension Packs
            extensionPacksNode.Nodes.Clear();
            foreach (var extensionPack in ExtensionLoader.ExtensionPacks)
                extensionPacksNode.Nodes.Add(extensionPack.Name + " (" + extensionPack.Version.ToString() + ")");

            // Engine Extensions
            engineExtensionsNode.Nodes.Clear();
            foreach (var extension in typeMapper.EngineProperties)
                engineExtensionsNode.Nodes.Add(extension.Name);

            // Item Properties
            propertiesNode.Nodes.Clear();
            foreach (var property in typeMapper.ItemProperties)
                propertiesNode.Nodes.Add(property.Name);

            // Factions
            factionsNode.Nodes.Clear();
            foreach (var faction in typeMapper.Factions)
                factionsNode.Nodes.Add(faction.Name);

            // Game Items
            //itemsNode.Nodes.Clear();
            //foreach (var item in typeMapper.GameItems)
            //    itemsNode.Nodes.Add(item.Name);
        }
    }
}
