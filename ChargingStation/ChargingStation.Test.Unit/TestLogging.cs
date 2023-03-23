using ChargingStation.lib.Interfaces;
using ChargingStation.lib.Simulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStation.Test.Unit
{
    internal class TestLogging
    {
        LogFile log;
        [SetUp]
        public void Setup()
        {
            log = new LogFile("test.txt");
        }

        [Test]
        public void LogWithID()
        {
            const string message = "test af log med ID";
            const int id = 0;
            log.WriteLogEntry(message, id);
            var last = File.ReadLines(log.fileName).Last();
            var expected = $"{DateTime.Now}: {message}: {id}";
            Assert.That(last, Is.EqualTo(expected));
        }

        [Test]
        public void LogWithoutID()
        {
            const string message = "test af log uden ID";
            log.WriteLogEntry(message);
            var last = File.ReadLines(log.fileName).Last();
            var expected = $"{DateTime.Now}: {message}";
            Assert.That(last, Is.EqualTo(expected));
        }
    }
}
