using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

using AudioSwitcher.AudioApi.CoreAudio;
using TigrMixLink.Library;
using System.Text.Json;
using AudioSwitcher.AudioApi;

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
        private AppConfig config;
        private List<string> deviceNames;


        private readonly CoreAudioController controller = new();
        private IEnumerable<CoreAudioDevice> devices;
        

        private int numVolumes = 0;

        private SerialPort serialPort;
        private string comPort = "COM5";
        private int baud = 115200;


        public readonly IConfiguration Configuration;

        public Form1()
        {
            InitializeIcon();
            InitializeComponent();
            this.CreateHandle();
            Configuration = Program.Services!.GetRequiredService<IConfiguration>();
            ChangeToken.OnChange(() => Configuration.GetReloadToken(), OnChange);
            OnChange();


            if (File.Exists("config.json"))
            {
                String json = File.ReadAllText("config.json");
                config = JsonSerializer.Deserialize<AppConfig>(json);
                comPort = config.comPort;
                baud = config.baudRate;
                deviceNames = config.Devices;
            }
            serialPort = new(comPort, baud);
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.Open();

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
            ComboBox comboBox = new()
            {
                Size = new(100, 20),
                Location = new(40 + numVolumes * 145, 50),
                Name = "comboBox" + numVolumes.ToString()
            };

            Button remove = new()
            {
                Size = new(25, 25),
                Location = new(150 + numVolumes * 145, 50),
                Text = "-",
                Name = "remove" + numVolumes.ToString(),
                BackColor = Color.Red
            };
            remove.Click += (s, e) =>
            {
                int index = int.Parse(remove.Name.Substring(6));
                Controls.Remove(comboBox);
                Controls.Remove(remove);
                for (int i = index; i < numVolumes-1; i++)
                {
                    if (Controls["comboBox" + (i + 1).ToString()] == null) break;
                    Controls["comboBox" + (i + 1).ToString()].Name = "comboBox" + i.ToString();
                    Controls["remove" + (i + 1).ToString()].Name = "remove" + i.ToString();
                    Controls["comboBox" + i.ToString()].Location = new(40 + i * 145, 50);
                    Controls["remove" + i.ToString()].Location = new(150 + i * 145, 50);
                }
                numVolumes--;
            };

            numVolumes++;
            Controls.Add(comboBox);
            Controls.Add(remove);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            devices = controller.GetDevices();
            addVolumeController();
        }

        private string prevInput= "";
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            String input = serialPort.ReadExisting();
            if (input != prevInput) UpdateVolumes(input.Split('|'));
            prevInput = input;
        }

        private void UpdateVolumes(String[] volumes)
        {
            for (int i = 0; i < Math.Min(volumes.Length, deviceNames.Count); i++) {
                CoreAudioDevice device = devices.FirstOrDefault(d => d.FullName == deviceNames[i]);
                device.Volume = int.Parse(volumes[i]);
            }
        }
    }
}