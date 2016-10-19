using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AntMe;

namespace CoreTestClient
{
    public partial class LocalizationTypeContainer : UserControl
    {

        private const int ROWSPACE = 21;

        public string TypeKey { get; private set; }
        public List<string> Keys { get; private set; }

        public LocalizationTypeContainer(string typekey, KeyValueStore keyValueStore)
        {
            Keys = new List<string>();

            InitializeComponent();

            TypeKey = typekey;
            labelType.Text = typekey;

            int currentY = 18;
            int maxX = 0;

            foreach (string key in keyValueStore.Keys.Where(k => k.StartsWith(string.Format("{0}:", TypeKey))).Select(k => k.Substring(k.IndexOf(":") + 1)).ToArray())
            {
                Label KeyLabel = new Label();
                KeyLabel.Name = string.Format("{0}Label", key);
                KeyLabel.Location = new Point(0, currentY + 2);
                KeyLabel.Text = key;

                Controls.Add(KeyLabel);

                TextBox ValueTextBox = new TextBox();
                ValueTextBox.Name = string.Format("{0}TextBoxValue", key);
                ValueTextBox.Location = new Point(KeyLabel.Size.Width + 3, currentY);
                ValueTextBox.Width = 200;
                ValueTextBox.Text = keyValueStore.GetString(string.Format("{0}:{1}", TypeKey, key));

                Controls.Add(ValueTextBox);

                TextBox DescriptionTextBox = new TextBox();
                DescriptionTextBox.Name = string.Format("{0}TextBoxDescription", key);
                DescriptionTextBox.Location = new Point(KeyLabel.Size.Width + 6 + ValueTextBox.Width, currentY);
                DescriptionTextBox.Width = 200;
                DescriptionTextBox.Text = keyValueStore.GetDescription(string.Format("{0}:{1}", TypeKey, key));

                Controls.Add(DescriptionTextBox);
                Keys.Add(key);

                currentY += ROWSPACE;
                maxX = maxX < DescriptionTextBox.Location.X + DescriptionTextBox.Width ? DescriptionTextBox.Location.X + DescriptionTextBox.Width : maxX;



            }

            this.Height = currentY;
            this.Width = maxX + 10;

        }


    }
}
