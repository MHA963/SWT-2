using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargingStation.lib.Interfaces;

namespace ChargingStation.lib.Simulators
{
    internal class RFEDReader : IRfidReader
    {
        public event EventHandler<RfidReader> RfidEvent;

        public void RfidDetected(int id)
        {
            RfidEvent?.Invoke(this,new RfidReader {Id= id});
        }


    }
}
