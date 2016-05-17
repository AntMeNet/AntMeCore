using AntMe.Generator;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CoreTestClient
{
    public partial class LocalizationForm : Form
    {
        private Dictionary<Type, List<string>> dictionary;

        public LocalizationForm()
        {
            InitializeComponent();

            dictionary = ModpackGenerator.GetLocaKeys();

            foreach (var type in dictionary.Keys)
            {
                var node = typeList.Items.Add(type.FullName);
                node.Tag = type;
            }
        }

        private void typeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            itemList.Items.Clear();
            if (typeList.SelectedItems.Count > 0)
            {
                foreach (var item in dictionary[typeList.SelectedItems[0].Tag as Type])
                {
                    itemList.Items.Add(item);
                }
            }
        }
    }
}
