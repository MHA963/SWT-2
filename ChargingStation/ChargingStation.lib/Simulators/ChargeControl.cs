using ChargingStation.lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargingStation.lib.Simulators;
using ChargingStation.lib.Interfaces;

namespace ChargingStation.lib.Simulators
{
    public class ChargeControl : IChargeControl
    {
        IUsbCharger _UsbCharger;
        IDisplay _display;
        public double lastCurrent { get; private set; }
        public bool IsConnected { get; set; }
        public enum State { Charging, NotCharging, FinishedCharging, Error }
        public State _lastState = State.NotCharging;
        public ChargeControl(IDisplay display, IUsbCharger usbCharger)
        {
            //if (usbCharger == null)
            //{
            //    throw new ArgumentNullException(nameof(usbCharger));
            //}

            _UsbCharger = usbCharger;
            _display = display; 
            _UsbCharger.PowerEvent += OnNewCurrent;
            IsConnected = _UsbCharger.Connected;
            lastCurrent = _UsbCharger.PowerValue;

        }
        public void StartCharge()
        {
            _UsbCharger.StartCharge();
        }

        public void StopCharge()
        {
            _UsbCharger.StopCharge();
        }

        public void OnNewCurrent(object sender, PowerEventArgs e)
        {
            if (e.Power == lastCurrent) return;

            lastCurrent = e.Power;
            IsConnected = _UsbCharger.Connected;

            switch (e.Power)
            {
                case 0.0:
                    if (_lastState == State.NotCharging) return;
                    
                    _lastState = State.NotCharging;
                    break;
                case > 0.0 and <= 5.0:
                    if (_lastState == State.FinishedCharging) return;
                    _display.FjernTelefon();
                    _lastState = State.FinishedCharging;
                    break;
                case > 5.0 and <= 500.0:
                    if (_lastState == State.Charging) return;
                    _display.TelefonTilsluttet();
                    _lastState = State.Charging;
                    break;
                case > 500.0:
                    if (_lastState == State.Error) return;
                    _display.OpladerFejl();
                    _lastState = State.Error;
                    StopCharge();
                    break;
            }
        }




    }
}
