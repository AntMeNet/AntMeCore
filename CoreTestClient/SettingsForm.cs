using AntMe;
using AntMe.Runtime;
using System.Windows.Forms;

namespace CoreTestClient
{
    public partial class SettingsForm : Form
    {
        private Settings settings;

        public SettingsForm(Settings settings)
        {
            InitializeComponent();
            this.settings = settings;
        }

        public SettingsForm() : this(ExtensionLoader.DefaultTypeResolver.GetGlobalSettings()) { }
    }
}
