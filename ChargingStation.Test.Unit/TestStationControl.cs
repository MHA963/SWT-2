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


        [TestCase(12345)]
        public void TestRfidDetected(int newrfid)
        {
            //Arrange
            _uut._state = StationControl.LadeskabState.Locked;
            _uut._oldId = 12345;

            //Act
            _uut.RfidDetected(this, new RfidReader() { Id = newrfid});

            //Assert

            _uut._charger.Received(1).StopCharge();
            _uut._door.Received(1).UnlockDoor();
            _uut._display.Received(1).FjernTelefon();

            _uut.DoorOpen(this, new DoorEventArgs());
            Assert.That(_uut._state,Is.EqualTo(Available));
        }

        [TestCase(54321)]
        public void TestOnRfidDetected_WrongId(int newrfid)
        {
            //Arrange
            _uut._state = StationControl.LadeskabState.Locked;
            _uut._oldId = 12345;

            //Act
            _uut.RfidDetected(this, new RfidReader() { Id = newrfid});

            //Assert
            _uut._display.Received(1).RFIDFejl();
            Assert.That(_uut._state,Is.EqualTo(Locked));
        }

        [TestCase(12345678)]
        [TestCase(8765432)]
        public void test(int newRfid)
        {
            _uut._state = StationControl.LadeskabState.Available;
            _uut._charger.IsConnected = true; 
            //Act
            _uut.RfidDetected(this, new RfidReader { Id = newRfid });
        
            //Assert
            Assert.That(_uut._oldId, Is.EqualTo(newRfid));
        }


        [Test]
        public void TestRfidDetected_unlock()
        {
            //Assert
            _uut._state = StationControl.LadeskabState.Locked;
            _uut._oldId = 12345;

            // Dette er white box kald af event handleren
            _uut.RfidDetected(this, new RfidReader(){Id = 12345});

            // asserts
            _uut._charger.Received(1).StopCharge();
            _uut._door.Received(1).UnlockDoor();
            _uut._display.Received(1).FjernTelefon();
            Assert.That(_uut._state,Is.EqualTo(Available));

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
            DoorEventArgs doorEvent = new DoorEventArgs();
            _uut._charger.IsConnected = true;
            _uut._state = Available;
          

            _uut.RfidDetected(this, rfidReader);

            _uut._display.Received(1).LadeskabOptaget();
            _uut._door.Received(1).LockDoor();
            Assert.That(_uut._state, Is.EqualTo(Locked));
        }


        [Test]

        public void TestRfidDetected_PhoneConnected_Locked()
        {
            _uut._charger.IsConnected = true;
            _uut._state = Locked;
            _uut.RfidDetected(this, new RfidReader() { Id=12345});
            _uut.DoorOpen(this, new DoorEventArgs());
            Assert.That(_uut._state,Is.EqualTo(Locked));
        }

        [Test]

        public void TestDoorClosedStateAvaialble()
        {
            _uut._state = Available;
            _uut._charger.IsConnected = false;
            _uut.RfidDetected(this, new RfidReader() { Id = 12345 });
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

        public void TestDoorOpenStateDoorOpen()
        {
            _uut._state = DoorOpen;
            _uut._charger.IsConnected = true;
            _uut.DoorOpen(this,new DoorEventArgs() { DoorIsOpen = true});
            
            Assert.That(_uut._state,Is.EqualTo(Locked));
        }

        [Test]

        public void TestDoorOpenStateChargerDisconnected()
        {
            _uut._state = DoorOpen;
            _uut._charger.IsConnected = false;
            _uut.DoorOpen(this, new DoorEventArgs() { DoorIsOpen = true });

            Assert.That(_uut._state, Is.EqualTo(Available));
        }



    }
}
