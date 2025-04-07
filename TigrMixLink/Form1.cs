using System;
using System.IO.Ports;
using System.Reflection;

using AudioSwitcher.AudioApi.CoreAudio;
using TigrMixLink.Library;
using System.Text.Json;
using AudioSwitcher.AudioApi;
using System.Runtime.InteropServices;

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
        private List<string> allDeviceNames = new List<string> { };


        private readonly CoreAudioController controller = new();
        private List<CoreAudioDevice> devices;


        private int numVolumes = 0;

        private SerialPort serialPort;
        private string comPort = "COM5";
        private int baud = 115200;


        public Form1()
        {
            InitializeIcon();
            InitializeComponent();


            if (File.Exists("config.json"))
            {
                String json = File.ReadAllText("config.json");
                config = JsonSerializer.Deserialize<AppConfig>(json);
                comPort = config.ComPort;
                baud = config.baudRate;
                deviceNames = config.Devices;
            }
            serialPort = new SerialPort(comPort, baud);
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addVolumeController();
        }

        private void addVolumeController()
        {
            ComboBox comboBox = new()
            {
                Size = new(300, 30),
                Location = new(40 + numVolumes * 345, 50),
                Name = "comboBox" + numVolumes.ToString(),
                DataSource = new List<string>(allDeviceNames),
            };
            if (deviceNames.Count < numVolumes) deviceNames.Add(comboBox.SelectedItem.ToString());

            comboBox.SelectedIndexChanged += (s, e) =>
            {
                int index = int.Parse(comboBox.Name.Substring(8));
                deviceNames[index] = comboBox.SelectedItem.ToString();
            };

            numVolumes++;
            Controls.Add(comboBox);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            devices = controller.GetPlaybackDevices().Where(d => d.State == DeviceState.Active).ToList();
            devices.AddRange(controller.GetCaptureDevices().Where(d => d.State == DeviceState.Active).ToList());
            foreach (var device in devices)
            {
                allDeviceNames.Add(device.FullName);
            }
            string[] test = { "50", "50" };
            UpdateVolumes(test);
            addVolumeController();
        }

        private string prevInput = "";
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine("reading");
            string input = serialPort.ReadExisting();
            Console.WriteLine(input);
            //Console.WriteLine(input.Split('|'));
            //if (input != prevInput) UpdateVolumes(input.Split('|'));
            //prevInput = input;
            //serialPort.DiscardInBuffer();
        }

        private void UpdateVolumes(String[] volumes)
        {
            for (int i = 0; i < Math.Min(volumes.Length, deviceNames.Count); i++)
            {
                CoreAudioDevice device = devices.FirstOrDefault(d => d.FullName == deviceNames[i]);
                if(device != null) device.Volume = int.Parse(volumes[i]);
            }
        }

        private void BTNremoveLast_Click(object sender, EventArgs e)
        {
            if (numVolumes > 0)
            {
                Controls.Remove(Controls["combobox" + numVolumes.ToString()]);
                numVolumes--;
            }
        }
    }
}