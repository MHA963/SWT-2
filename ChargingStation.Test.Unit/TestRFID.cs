using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargingStation.lib.Simulators;
using ChargingStation.lib.Interfaces;

namespace ChargingStation.Test.Unit
{
    internal class TestRFID
    {
        private RFidReader rfid;
        private int Id;

        [SetUp]
        public void Setup()
        {
            rfid = new RFidReader();
            rfid.RfidEvent += RfidDetected;
        }

        private void RfidDetected(object source, RfidReader e)
        {
            Id = e.Id;
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(-100)]
        public void TestRfidDetected(int id)
        {
            rfid.RfidDetected(id);
            Assert.That(Id, Is.EqualTo(id));
        }
    }
}
