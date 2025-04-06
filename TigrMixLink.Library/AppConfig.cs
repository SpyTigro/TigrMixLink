using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TigrMixLink.Library
{
    public class AppConfig
    {
        public string comPort { get; set; }
        public int baudRate { get; set; }  
        public List<string> Devices { get; set; } 
    }
}

