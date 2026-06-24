using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactoryLineMKYA.Models
{
    public class SapConnectionConfig
    {
        public string RfcName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Client { get; set; } = string.Empty;
        public string Lang { get; set; } = string.Empty;
        public string AppServerHost { get; set; } = string.Empty;
        public string SystemID { get; set; } = string.Empty;
        public string SystemNumber { get; set; } = string.Empty;
        public int PoolSize { get; set; }
        public int MaxPoolSize { get; set; }
    }
}
