using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using ChargingStation.lib.Interfaces;
using ChargingStation.lib.Simulators;
using NSubstitute;
using NSubstitute.Core.DependencyInjection;
using static ChargingStation.lib.Simulators.ChargeControl;

namespace ChargingStation.Test.Unit
{
    [TestFixture]
    public class TestChargeControl
    {
        private ChargeControl _chargecontrol;
        //// Brug altid interfaces for dependencies i tests


        //private IChargeControl _chargecontrol;
        private IDisplay _display;
        private IUsbCharger _UsbCharger;

        [SetUp]
        public void Setup()
        {

            _display = Substitute.For<IDisplay>();
            _UsbCharger = Substitute.For<IUsbCharger>();
            _chargecontrol = new ChargeControl(_display, _UsbCharger);

        }

        [Test]
        public void TestStartCharge()
        {
            _chargecontrol.StartCharge();
            _UsbCharger.Received(1).StartCharge();
        }

        [TestCase(500.0)]
        [TestCase(4.9)]
        [TestCase(250.0)]
        [TestCase(501.0)]
        public void TestNormalCharge(double value)
        {
            _chargecontrol.UsbCharger().PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            Assert.That(_chargecontrol._lastState == State.Charging);
        }

        [TestCase(0.0)]
        [TestCase(1.0)]
        [TestCase(-1.0)]
        public void TestNoCharge(double value)
        {
            // Sets the state previous to the test to "Charging at 500 mA"
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });


            Assert.That(_chargecontrol._lastState == State.NotCharging);

        }


        [TestCase(0.0)]
        [TestCase(5.1)]
        [TestCase(2.5)]
        [TestCase(4.0)]
        public void TestFullyCharge(double value)
        {
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });

            Assert.That(_chargecontrol._lastState == State.FinishedCharging);
        }


        [Test]
        public void TestStopCharge()
        {
            _chargecontrol.StopCharge();
            _UsbCharger.Received(1).StopCharge();
        }

        [TestCase(750.0)]
        [TestCase(500.0)]
        [TestCase(10000)]
        public void TestOverloadCurrent(double value)
        {

            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            _UsbCharger.Received(1).StopCharge();

        }

        //[Test]
        //public void TestCase0OnNewCurrent()
        //{
        //    //double initialvalue = 500.0;
        //    double value = 1.0;


        //    //_chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = initialvalue });
        //    _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
        //    // Dette tester faken - helt unødvendigt
        //    Assert.That(_chargecontrol._UsbCharger.PowerValue, Is.EqualTo(value));

        //}
    }
}
