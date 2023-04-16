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
        // Brug altid interfaces for dependencies i tests

        private IDisplay _display;
        private IUsbCharger _UsbCharger;

        [SetUp]
        public void Setup()
        {
            // Brug altid interface, når Substitute.For<> bruges
            // ellers vil testen altid bestå
            _display = Substitute.For<IDisplay>();
            _UsbCharger = Substitute.For<IUsbCharger>();
            _chargecontrol = new ChargeControl(_display, _UsbCharger);

            // Unødvendig tilknytning til dependency, som allerede er 
            // foretaget i ChargeControls constructor
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
            // White box test af ChargeControl
            Assert.That(_chargecontrol.LastCurrent, Is.EqualTo(500.0));

        }

        [Test]
        public void TestNoCharge()
        {
            // Sets the state previous to the test to "Charging at 500 mA"
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = 500.0 });

            // Denne test ville altid bestå, når I bruger Display i stedet for IDisplay
            // Og i tester for det forkerte
            _display.Received(1).TilslutTelefon();

            // Gentagen, unødvendig white box test
            Assert.That(_chargecontrol.LastCurrent, Is.EqualTo(500.0));
        }

        [Test]
        public void TestFullyCharge()
        {
            double value = 2.5;
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            // Gentagen, unødvendig white box test
            Assert.That(_chargecontrol.LastCurrent, Is.EqualTo(value));
        }

        [Test]
        public void TestStopCharge()
        {
            // Denne test tester ikke, hvad dens navn indikerer
            double value = 501.0;
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });

            // Gentagen, unødvendig white box test
            Assert.That(_chargecontrol.LastCurrent, Is.EqualTo(value));
        }

        [Test]
        public void TestOverloadCurrent()
        {
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = 750 });
            // Her er den rigtige test
            _UsbCharger.Received(1).StopCharge();

            // Dette tester faken - helt unødvendigt
            Assert.That(_chargecontrol._UsbCharger.PowerValue, Is.EqualTo(0));
        }

        [Test]
        public void TestCase0OnNewCurrent()
        {
            double initialvalue = 500.0;
            double value = 0.0;

            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = initialvalue });
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            // Dette tester faken - helt unødvendigt
            Assert.That(_chargecontrol._UsbCharger.PowerValue, Is.EqualTo(0));
        }
    }
}
