using System;
using System.Timers;
using ChargingStation.lib.Interfaces;


namespace ChargingStation.lib.Simulators
{
    public class UsbChargerSimulator : IUsbCharger
    {
        // Constants
        private const double MaxCurrent = 500.0; // mA
        private const double FullyChargedCurrent = 2.5; // mA
        private const double OverloadCurrent = 750; // mA
        private const int ChargeTimeMinutes = 20; // minutes
        private const int CurrentTickInterval = 250; // ms


        public event EventHandler<PowerEventArgs>? PowerEvent;
        public double PowerValue { get; private set; }
        public bool Connected { get; set; }

        private bool _overload;
        private bool _charging;
        private System.Timers.Timer _timer;
        private int _ticksSinceStart;

        public UsbChargerSimulator()
        {
            PowerValue = 0.0;
            Connected = true;
            _overload = false;

            _timer = new System.Timers.Timer();
            _timer.Enabled = false;
            _timer.Interval = CurrentTickInterval;
            _timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            // Only execute if charging
            if (_charging)
            {
                _ticksSinceStart++;
                if (Connected && !_overload)
                {
                    double newValue = MaxCurrent - 
                                      _ticksSinceStart * (MaxCurrent - FullyChargedCurrent) / (ChargeTimeMinutes * 60 * 1000 / CurrentTickInterval);
                    PowerValue = Math.Max(newValue, FullyChargedCurrent);
                }
                else if (Connected && _overload)
                {
                    PowerValue = OverloadCurrent;
                }
                else if (!Connected)
                {
                    PowerValue = 0.0;
                }

                OnNewCurrent();
            }
        }

        public void SimulateConnected(bool connected)
        {
            Connected = connected;
        }

        public void SimulateOverload(bool overload)
        {
            _overload = overload;
        }

        public void StartCharge()
        {
            // Ignore if already charging
            if (!_charging)
            {
                if (Connected && !_overload)
                {
                    PowerValue = 500;
                }
                else if (Connected && _overload)
                {
                    PowerValue = OverloadCurrent;
                }
                else if (!Connected)
                {
                    PowerValue = 0.0;
                }

                OnNewCurrent();
                _ticksSinceStart = 0;

                _charging = true;

                _timer.Start();
            }
        }

        public void StopCharge()
        {
            _timer.Stop();

            PowerValue = 0.0;
            OnNewCurrent();

            _charging = false;
        }

        private void OnNewCurrent()
        {
            PowerEvent?.Invoke(this, new PowerEventArgs() {Power = this.PowerValue });
        }
    }
}
