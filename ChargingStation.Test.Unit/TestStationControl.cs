using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ChargingStation.lib;
using ChargingStation.lib.Interfaces;
using ChargingStation.lib.Simulators;
using static ChargingStation.lib.StationControl.LadeskabState;
using NSubstitute;


namespace ChargingStation.Test.Unit
{
    public class TestStationControl
    {
        // Brug interfaces for dependencies
        private StationControl _uut;
        private IDoor _door;
        private IRfidReader _rfidReader;
        private IDisplay _display;
        private IChargeControl _chargeControl;
        private ILog _log;
        private IUsbCharger _usbCharger;


        [SetUp]
        public void Setup()
        {
            // Brug interfaces når fakes laves med Substitute.For<>
            // Ellers vil testene altid bestå
            _display = Substitute.For<IDisplay>();
            _door = Substitute.For<IDoor>();
            _rfidReader = Substitute.For<IRfidReader>();
            _log = Substitute.For<ILog>();
            _usbCharger = Substitute.For<IUsbCharger>();
            _chargeControl = Substitute.For<IChargeControl>();
            _uut = new StationControl(_rfidReader, _chargeControl, _door, _display, _log);
        }

        [Test]
        public void TestRfidDetected_lock()
        {
            //Assert
            _uut._state = StationControl.LadeskabState.Locked;
            _uut._oldId = 12345;

            // Dette er white box kald af event handleren
            _uut.RfidDetected(this, new RfidReader(){Id = 12345});

            // Dette er ikke asserts
            _uut._charger.StopCharge();
            _uut._door.UnlockDoor();
            _uut._log.WriteLogEntry("Skab er låst op med rfid, 12345");

            _uut._display.FjernTelefon();
            Assert.That(_uut._state,Is.EqualTo(Available));

        }


        [Test]
        public void TestRfidDetected_locked_iswrong()
        {
            _uut._state = Locked;
            _uut._oldId = 456;
            _uut.RfidDetected(this,new RfidReader(){Id = 12345});
            _uut._display.RFIDFejl();
            Assert.That(_uut._state,Is.EqualTo(Locked));

        }

        [Test]
        public void TestRfidDetected_DoorOpen()
        {
            _uut._state = DoorOpen;
            _uut._oldId = 12345;
            _uut.RfidDetected(this, new RfidReader(){Id=12345});
            Assert.That(_uut._state, Is.EqualTo(DoorOpen));
        }

        [Test]
        public void TestRfidDetected_PhoneConnected()
        {
            RfidReader rfidReader = new RfidReader() { Id = 12345 };
            _uut._charger.IsConnected = true;
            _uut._state = Available;
            _uut.RfidDetected(this, rfidReader);
            _uut._door.Received(1).LockDoor();
            _uut._charger.Received(1).StartCharge();
            _uut._log.Received(1).WriteLogEntry("Skab er låst med RFID", rfidReader.Id);
            _uut._display.Received(1).LadeskabOptaget();
            _uut._display.Received(1).TilslutTelefon();
            Assert.That(_uut._state, Is.EqualTo(Locked));
        }

        [Test]
        public void TestRfidDetected_PhoneNotConnected()
        {
            _uut._charger.IsConnected = false;
            _uut._state = Available;
            _uut.RfidDetected(this, new RfidReader(){Id=12345});
            _uut._display.Received(1).TilslutTelefon();
            Assert.That(_uut._state,Is.EqualTo(Available));

        }

        [Test]

        public void TestRfidDetected_PhoneConnected_DoorOpen()
        {
            _uut._charger.IsConnected = true;
            _uut._state = DoorOpen;
            _uut.RfidDetected(this, new RfidReader() { Id=12345});
            _uut._display.Received(1).TelefonTilsluttet();
            Assert.That(_uut._state,Is.EqualTo(DoorOpen));
        }

        [Test]

        public void TestRfidDetected_PhoneConnected_Locked()
        {
            _uut._charger.IsConnected = true;
            _uut._state = Locked;
            _uut.RfidDetected(this, new RfidReader() { Id=12345});
            _uut._display.Received(1).TelefonTilsluttet();
            Assert.That(_uut._state,Is.EqualTo(Locked));
        }

        [Test]

        public void TestRfidDetected_PhoneConnected_Locked_iswrong()
        {
            _uut._charger.IsConnected = true;
            _uut._state = Locked;
            _uut.RfidDetected(this, new RfidReader() { Id=123456});
            _uut._display.Received(1).RFIDFejl();
            Assert.That(_uut._state,Is.EqualTo(Locked));
        }

        [Test]

        public void TestDoorClosedStateAvaialble()
        {
            _uut._state = Available;
            _uut.DoorLocked(this,new DoorEventArgs(){DoorIsOpen = false});
            Assert.That(_uut._state,Is.EqualTo(Available));

        }

        [Test]
        public void TestAvailableStateDoorOpen()
        {
            _uut._state = Available;
            _uut.DoorOpen(this,new DoorEventArgs(){DoorIsOpen = true});
            Assert.That(_uut._state,Is.EqualTo(DoorOpen));

        }

        [Test]
        public void TestDoorLockedStateLocked()
        {
            _uut._state = DoorOpen;
            _uut.DoorLocked(this,new DoorEventArgs(){DoorIsOpen = false});
            _uut._charger.IsConnected=true;
            _uut._display.RFIDLåst();
            Assert.That(_uut._state,Is.EqualTo(Locked));
        }


        [Test]
        public void TestDoorLockedStateLocked_PhoneNotConnected()
        {
            _uut._state = Locked;
            _uut.DoorLocked(this,new DoorEventArgs() { DoorIsOpen = false});
            _uut._charger.IsConnected=false;
            _uut._display.TilslutTelefon();
            Assert.That(_uut._state,Is.EqualTo(Locked));
        }

        [Test]

        public void TestDoorOpenStateDoorOpen()
        {
            _uut._state = DoorOpen;
            _uut.DoorOpen(this,new DoorEventArgs() { DoorIsOpen = true});
            _uut._charger.IsConnected=true;
            _uut._display.RFIDLåst();
            Assert.That(_uut._state,Is.EqualTo(DoorOpen));
        }

        [Test]
        public void TestDoorOpenStateLocked()
        {
            _uut._state = Locked;
            _uut.IsConnected = false;
            _uut.DoorOpen(this, new DoorEventArgs() { DoorIsOpen = true });
            Assert.That(_uut._state, Is.EqualTo(Locked));
        }



    }
}
