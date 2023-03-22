using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStation.lib.Interfaces
{
    public class IDoor
    {
        public bool UnlockDoor();
        public bool LockDoor();
       
        private bool IsDoorLocked;
    }
}
