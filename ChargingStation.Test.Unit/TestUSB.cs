using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargingStation.lib.Interfaces;
using ChargingStation.lib.Simulators;

namespace ChargingStation.Test.Unit
{
    [TestFixture]
    internal class TestUSB
    {
        private UsbChargerSimulator usb;

        [SetUp]
        public void Setup()
        {
            usb = new UsbChargerSimulator();
        }

        [Test]
        public void TestUsbChargerSimulatorIsConnected()
        {
            Assert.That(usb.Connected, Is.True);
        }

        [Test]
        public void TestUsbChargerSimulatorThenLaterOverloading()
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            double powerValue = 0.0;

            usb.PowerEvent += (o, args) =>
            {
                powerValue = args.Power;
                pause.Set();
            };

            usb.StartCharge();

            usb.SimulateOverload(true);

            pause.Reset();

            pause.WaitOne(1000);

            Assert.That(powerValue, Is.GreaterThan(500));
        }

        [Test]
        public void TestSimulatedStartThenDisconnection()
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            double Value = 1000.0;
            usb.PowerEvent += (o, args) =>
            {
                Value = args.Power;
                pause.Set();
            };

            usb.StartCharge();

            usb.SimulateConnected(false);

            pause.Reset();

            pause.WaitOne(1000);

            Assert.That(Value, Is.EqualTo(0.0));

        }

        [Test]
        public void TestUsbChargerSimulatorWithImmediateOverloading()
        {
            double value = 0;

            usb.PowerEvent += (o, args) =>
            {
                value = args.Power;
            };

            usb.SimulateOverload(true);

            usb.StartCharge();

            Assert.That(value, Is.GreaterThan(500));
        }

        [Test]
        public void TestUsbChargerSimulatorTStopChargingAfterReceivingNoValue()
        {
            double value = 1000;
            usb.PowerEvent += (o, args) =>
            {
                value = args.Power;
            };

            usb.StartCharge();

            System.Threading.Thread.Sleep(1000);

            usb.StopCharge();

            Assert.That(value, Is.EqualTo(0.0));
        }

        [Test]
        public void TestUsbChargerSimulatorStopChargingWithNoValue()
        {
            usb.StartCharge();

            System.Threading.Thread.Sleep(1000);

            usb.StopCharge();

            Assert.That(usb.PowerValue, Is.EqualTo(0.0));

        }

        [Test]
        public void TestUsbChargerSimulatorStopChargingWhenReceivingNoPower()
        {
            double value = 1000;
            usb.PowerEvent += (o, args) =>
            {
                value = args.Power;
            };
            usb.StartCharge();

            System.Threading.Thread.Sleep(1000);

            usb.StopCharge();

            value = 1000;

            System.Threading.Thread.Sleep(1000);

            Assert.That(value, Is.EqualTo(1000));
        }

        [Test]
        public void TestUsbChargerSimulatorPowerIsZero()
        {
            Assert.That(usb.PowerValue, Is.EqualTo(0.0));
        }

        [Test]
        public void TestUsbChargerSimulatorDisconnected()
        {
            usb.SimulateConnected(false);
            Assert.That(usb.Connected, Is.False);
        }

        [Test]
        public void TestUsbChargerSimulatorGettingMultiplePowerValues()
        {
            var value = 0;
            usb.PowerEvent += (o, args) =>
            {
                value++;
            };

            usb.StartCharge();

            System.Threading.Thread.Sleep(3000);

            Assert.That(value, Is.GreaterThan(10));
        }

        [Test]
        public void TestUsbChargerSimulatorChangingValue()
        {
            var value = 1000.0;
            usb.PowerEvent += (o, args) =>
            {
                value = args.Power;
            };

            usb.StartCharge();

            System.Threading.Thread.Sleep(1000);

            Assert.That(value, Is.InRange(0, 500));
        }

        [Test]
        public void TestUsbChargerSimulatorStartChargingWithoutBeingConnected()
        {
            usb.Connected = false;
            usb.StartCharge();
            Assert.That(usb.PowerValue, Is.Zero);
        }
    }
}
