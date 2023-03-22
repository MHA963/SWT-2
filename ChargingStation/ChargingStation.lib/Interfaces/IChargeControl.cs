using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStation.lib.Interfaces
{
    internal class IChargeControl
    {
        public bool IsConnected { get; set; }
        public void StartCharge() { }
        public void StopCharge() { }

    }
}
