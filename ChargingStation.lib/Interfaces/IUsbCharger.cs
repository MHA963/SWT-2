using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStation.lib.Interfaces
{
    public class PowerEventArgs : EventArgs
    {
        public double Power { get; set; }
    }
    public interface IUsbCharger
    {
        //Event triggered on new power value
        event EventHandler<PowerEventArgs> PowerEvent;
        
        //Direct access to the current power value        
        double PowerValue { get; }

        public bool Connected { get; set; }
        void StartCharge();
        void StopCharge();

    }
}
