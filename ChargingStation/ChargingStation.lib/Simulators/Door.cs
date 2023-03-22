using ChargingStation.lib.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargingStation.lib.Interfaces;


namespace ChargingStation.lib.Simulators
{
    internal class Door : IDoor
    {
        public bool IsDoorLocked { get; set; }
        public bool IsDoorOpen { get; set; }
        public int CurrentID { get; set; } = 0;

        private DoorEventArgs _doorEvent = new DoorEventArgs()
        {
            IsDoorOpen = false
        };

        public event EventHandler<DoorEventArgs> DoorEvent;

        public Door()
        {
            IsDoorLocked = false;
            IsDoorOpen = false;
        }

        public bool UnlockDoor()
        {
            if (!IsDoorOpen && IsDoorLocked)
                IsDoorLocked = false;
            return IsDoorOpen;
        }

        public bool LockDoor()
        {
            if (IsDoorOpen)
                IsDoorLocked = true;
            return IsDoorLocked;
        }

        public virtual void DoorOpened()
        {
            IsDoorOpen = true;
            _doorEvent.IsDoorOpen = true;
            DoorEvent?.Invoke(this, _doorEvent);
        }

        public virtual void DoorClosed()
        {
            IsDoorOpen = false;
            _doorEvent.IsDoorOpen = false;
            DoorEvent?.Invoke(this, _doorEvent);
        }
    }

}
