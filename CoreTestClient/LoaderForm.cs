using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using AntMe.Runtime;

namespace CoreTestClient
{
    internal partial class LoaderForm : Form
    {
        private ProgressToken token = new ProgressToken();

        public LoaderForm()
        {
            InitializeComponent();
            Size = new Size(Size.Width, 133);
            updateTimer.Enabled = true;

            Task t = new Task(() =>
            {
                try
                {
                    string[] pathes = new string[] {
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Extensions",
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Extensions\\netstandard2.0",
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\AntMe\\Extensions"
                    };

                    ExtensionLoader.LoadExtensions(pathes, token, true);
                    Invoke((MethodInvoker)delegate
                    {
                        HandleSuccess();
                    });

                }
                catch (AggregateException ex)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        HandleExceptions(ex);
                    });
                }
            });
            t.Start();
        }

        private void HandleSuccess()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    Close();
                });
            }
            else
            {
                Close();
            }
        }

        private void HandleExceptions(AggregateException ex)
        {
            Size = new Size(Size.Width, 380);

            foreach (var exception in ex.InnerExceptions)
                errorList.Items.Add(exception.Message);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            updateTimer.Enabled = false;
            base.OnFormClosing(e);
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            progressBar.Maximum = token.TotalTasks;
            progressBar.Value = token.CurrentTask;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
