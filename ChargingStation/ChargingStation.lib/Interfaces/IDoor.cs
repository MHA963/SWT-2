using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStation.lib.Interfaces
{
    public class DoorEventArgs : EventArgs
    {
        public bool DoorIsOpen { get; set; }
    }

   
    public interface IDoor
    {

        public event EventHandler<DoorEventArgs>? DoorEvent;
        public bool UnlockDoor();
        public bool LockDoor();
        
    }
}
