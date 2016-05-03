using AntMe.Runtime;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreTestClient
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            using (LoaderForm form = new LoaderForm())
            {
                form.ShowDialog(this);
            }
        }

        private void closeMenu_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void extensionsMenu_Click(object sender, EventArgs e)
        {
            using (ExtensionsForm form = new ExtensionsForm())
            {
                form.ShowDialog(this);
            }
        }
    }
}
