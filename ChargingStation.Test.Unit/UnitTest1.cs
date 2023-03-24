using ChargingStation.lib;
using ChargingStation.lib.Interfaces;
using ChargingStation.lib.Simulators;

namespace ChargingStation.Test.Unit
{
    public class Tests
    {

        private StationControl uut;
        [SetUp]
        public void Setup()
        {
            Door door = new Door();
            RFidReader rfidReader = new RFidReader();
            Display display = new Display();
            ChargeControl chargeControl = new ChargeControl(display, new UsbChargerSimulator());
            LogFile log = new LogFile("ProgramLog.txt");

            var stationControl = new StationControl(rfidReader, chargeControl, door, display, log);
        }


        [Test]
        public void Test1()
        {
            Assert.Pass();
        }


    }
}
