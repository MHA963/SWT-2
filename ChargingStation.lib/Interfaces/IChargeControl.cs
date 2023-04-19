using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargingStation.lib.Simulators;

namespace ChargingStation.lib.Interfaces
{
    public interface IChargeControl
    {
        public bool IsConnected { get; set; }

        public double LastCurrent { get; set; }

        public void OnNewCurrent(object sender, PowerEventArgs e);
        public void StartCharge();
        public void StopCharge();

        public IUsbCharger UsbCharger();


    }
}
