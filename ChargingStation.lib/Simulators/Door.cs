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
    public class Door : IDoor
    {
        public bool IsDoorLocked { get; set; }
        public bool IsDoorOpen { get; set; }

        private DoorEventArgs _doorEvent = new DoorEventArgs()
        {
            DoorIsOpen = false
        };

        public event EventHandler<DoorEventArgs>? DoorEvent;

        public Door()
        {
            IsDoorLocked = false;
            IsDoorOpen = false;
        }

        public void UnlockDoor()
        {
            if (!IsDoorLocked)
            {
                return;
            }
            IsDoorLocked = false;
        }

        public void LockDoor()
        {
            if (IsDoorOpen || IsDoorLocked)
            {
                return;
            }

            IsDoorLocked = true;
        }

        public virtual void DoorOpened()
        {
            IsDoorOpen = true;
            _doorEvent.DoorIsOpen = true;
            DoorEvent?.Invoke(this, _doorEvent);
        }

        public virtual void DoorClosed()
        {
            IsDoorOpen = false;
            _doorEvent.DoorIsOpen = false;
            DoorEvent?.Invoke(this, _doorEvent);
        }
    }

}
