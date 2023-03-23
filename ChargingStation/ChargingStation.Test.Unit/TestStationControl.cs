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
        private StationControl _uut;
        private Door _door;
        private RFidReader _rfidReader;
        private Display _display;
        private ChargeControl _chargeControl;
        private LogFile _log;
        private IUsbCharger _usbCharger;


        [SetUp]
        public void Setup()
        {
            _display = Substitute.For<Display>();
            _door = Substitute.For<Door>();
            _rfidReader = Substitute.For<RFidReader>();
            _log = Substitute.For<LogFile>("Test.txt");
            _usbCharger = Substitute.For<UsbChargerSimulator>();
            _chargeControl = Substitute.For<ChargeControl>(_display, _usbCharger);
            _uut = new StationControl(_rfidReader, _chargeControl, _door, _display, _log);
        }

        [Test]
        public void TestRfidDetected_lock()
        {
            //Assert
            _uut._state = StationControl.LadeskabState.Locked;
            _uut._oldId = 12345;

            _uut.RfidDetected(this, new RfidReader(){Id = 12345});

            _uut._charger.StopCharge();
            _uut._door.UnlockDoor();
            _uut._log.WriteLogEntry("Skab er låst op med rfid, 12345");

            _uut._display.FjernTelefon();
            Assert.That(_uut._state,Is.EqualTo(Available));

        }


    }
}
