using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using ChargingStation.lib.Interfaces;
using ChargingStation.lib.Simulators;
using NSubstitute;

namespace ChargingStation.Test.Unit
{
    [TestFixture]   
    public class TestChargeControl
    {
        private ChargeControl _chargecontrol;
        private Display _display;
        private IUsbCharger _UsbCharger;

        [SetUp]
        public void Setup()
        {
            _display = Substitute.For<Display>();
            _UsbCharger = Substitute.For<IUsbCharger>();
            _chargecontrol = new ChargeControl(_display, _UsbCharger);
            _UsbCharger.PowerEvent += (o, args) => _chargecontrol.OnNewCurrent(o, args);
        }

        [Test]
        public void TestStartCharge()
        {
            _chargecontrol.StartCharge();
            _UsbCharger.Received(1).StartCharge();
        }

        [Test]
        public void TestNormalCharge()
        {
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = 500.0 });
            Assert.That(_chargecontrol.LastCurrent, Is.EqualTo(500.0));

        }

        [Test]
        public void TestNoCharge()
        {
            // Sets the state previous to the test to "Charging at 500 mA"
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = 500.0 });
           // double value = 0.0;

            //_chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            _display.Received(1).TilslutTelefon();

            Assert.That(_chargecontrol.LastCurrent, Is.EqualTo(500.0));
        }

        [Test]
        public void TestFullyCharge()
        {
            double value = 3.0;
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            Assert.That(_chargecontrol.LastCurrent, Is.EqualTo(value));
        }

        [Test]
        public void TestStopCharge()
        {
            double value = 501.0;
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });

            Assert.That(_chargecontrol.LastCurrent, Is.EqualTo(value));
        }

        [Test]
        public void TestOverloadCurrent()
        {
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = 750 });
            _UsbCharger.Received(1).StopCharge();
            Assert.That(_chargecontrol._UsbCharger.PowerValue, Is.EqualTo(0));
        }

        [Test]
        public void TestCase0OnNewCurrent()
        {
            double initialvalue = 500.0;
            double value = 0.0;

            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = initialvalue });
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            Assert.That(_chargecontrol._UsbCharger.PowerValue, Is.EqualTo(0));
        }
    }
}
