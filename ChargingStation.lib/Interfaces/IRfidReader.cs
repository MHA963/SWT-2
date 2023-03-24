using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStation.lib.Interfaces
{
    public class RfidReader : EventArgs
    {
        public int Id { get; set; }
    }

    public interface IRfidReader
    {
        event EventHandler<RfidReader> RfidEvent;
        void RfidDetected(int id);
    }
}
