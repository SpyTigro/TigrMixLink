using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace TigrMixLink
{
    public partial class Form1 : Form
    {
        #region InitializeIcon

        public void InitializeIcon()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "TigrMixLink.Resources.application.ico";
            if (assembly != null)
            {
                using Stream? stream = assembly.GetManifestResourceStream(resourceName);
                if (stream != null)
                {
                    this.Icon = new Icon(stream);
                }
            }
        }

        #endregion
        private int numVolumes = 0;

        public readonly IConfiguration Configuration;

        public Form1()
        {
            InitializeIcon();
            InitializeComponent();
            this.CreateHandle();
            Configuration = Program.Services!.GetRequiredService<IConfiguration>();
            ChangeToken.OnChange(() => Configuration.GetReloadToken(), OnChange);
            OnChange();
        }

        private void OnChange()
        {
            this.Invoke((System.Windows.Forms.MethodInvoker)delegate { this.Text = Configuration.GetSection("Settings:Subkey1:Value1").Get<string>(); });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addVolumeController();
        }

        private void addVolumeController()
        {
            ComboBox comboBox = new();
            comboBox.Size = new(100, 20);
            comboBox.Location = new(40 + numVolumes * 120, 50);
            numVolumes++;
            Controls.Add(comboBox);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            addVolumeController();
        }
    }
}