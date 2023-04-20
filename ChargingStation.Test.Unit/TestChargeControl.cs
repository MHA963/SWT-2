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

        #region Normalcharge

        [TestCase(500.0)]
        [TestCase(5.1)]
        [TestCase(350.0)]
        [TestCase(100.0)]
        public void TestNormalCharge(double value)
        {
            _chargecontrol.UsbCharger().PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            _display.Received(1).TelefonTilsluttet();
        }

        [TestCase(100000)]
        [TestCase(10000)]
        [TestCase(1001)]
        [TestCase(501)]
        public void TestNormalChargeWithOverCurrent(double value)
        {
            _chargecontrol.UsbCharger().PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            _display.DidNotReceive().TelefonTilsluttet();
        }

        [TestCase(5.0)]
        [TestCase(1.0)]
        [TestCase(-10)]
        [TestCase(-100)]
        public void TestNormalChargeWithUnderCurrent(double value)
        {
            _chargecontrol.UsbCharger().PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            _display.DidNotReceive().TelefonTilsluttet();
        }
        #endregion

        #region NotCharging
        [TestCase(0)]
        public void TestNoCharge(double value)
        {
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value + 10 });
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            _display.Received(1).TilslutTelefon();
        }
        [TestCase(1.0)]
        [TestCase(10.0)]
        [TestCase(100.0)]
        [TestCase(1000.0)]
        public void TestNoChargeWithOverCurrent(double value)
        {
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            _display.DidNotReceive().TilslutTelefon();
        }

        [TestCase(-1.0)]
        [TestCase(-10.0)]
        [TestCase(-100.0)]
        [TestCase(-1000.0)]
        public void TestNoChargeWithUnderCurrent(double value)
        {
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            _display.DidNotReceive().TilslutTelefon();
        }

        #endregion

        #region FullyCharged
        [TestCase(0.1)]
        [TestCase(2.0)]
        [TestCase(4.9)]
        [TestCase(3.0)]
        public void TestFullyCharge(double value)
        {
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            _display.Received(1).FjernTelefon();
        }

        [TestCase(5.1)]
        [TestCase(50.0)]
        [TestCase(500)]
        [TestCase(5000)]
        public void TestFullyChargeWithOverCurrent(double value)
        {
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            _display.DidNotReceive().FjernTelefon();
        }

        [TestCase(0.0)]
        [TestCase(-10.0)]
        [TestCase(-100.0)]
        [TestCase(-1000.0)]
        public void TestFullyChargeWithUnderCurrent(double value)
        {
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            _display.DidNotReceive().FjernTelefon();
        }
        #endregion

        #region StogCharging
        [Test]
        public void TestStopCharge()
        {
            _chargecontrol.StopCharge();
            _UsbCharger.Received(1).StopCharge();
        }

        #endregion

        #region OverloadCharging
        [TestCase(750.0)]
        [TestCase(501.0)]
        [TestCase(10000)]
        [TestCase(100000)]
        [TestCase(-1)]
        [TestCase(-100000)]
        public void TestOverloadCurrent(double value)
        {
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            _display.Received(1).OpladerFejl();

        }

        [TestCase(0.1)]
        [TestCase(2.0)]
        [TestCase(4.9)]
        [TestCase(3.0)]
        public void TestOverloadCurrentWithFullyChargedCurrent(double value)
        {
            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            _display.DidNotReceive().OpladerFejl();
        }

        [TestCase(0.0)]
        public void TestOverloadCurrentWithNoChargeCurrent(double value)
        {

            _chargecontrol._UsbCharger.PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            _display.DidNotReceive().OpladerFejl();
        }

        [TestCase(500.0)]
        [TestCase(5.1)]
        [TestCase(350.0)]
        [TestCase(100.0)]
        public void TestOverloadCurrentWithNormalChargeCurrent(double value)
        {
            _chargecontrol.UsbCharger().PowerEvent += Raise.EventWith(new PowerEventArgs() { Power = value });
            _display.DidNotReceive().OpladerFejl();
        }


        #endregion

       
    }
}
