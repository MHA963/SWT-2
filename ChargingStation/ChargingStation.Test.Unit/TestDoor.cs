using ChargingStation.lib.Simulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStation.Test.Unit
{
    public class TestDoor
    {
        private Door door;

        [SetUp]
        public void Setup()
        {
            door = new Door();
        }

        [TestCase(false, false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        public void TestDoorLock(bool DoorOpen, bool DoorLocked)
        {
            door.IsDoorLocked = DoorLocked;
            door.IsDoorOpen = DoorOpen;
            door.LockDoor();

            Assert.That(door.IsDoorLocked, !door.IsDoorOpen ? Is.EqualTo(true) : Is.EqualTo(false));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void TestDoorUnlock(bool DoorOpen, bool DoorLocked)
        {
            door.IsDoorLocked = DoorLocked;
            door.IsDoorOpen = DoorOpen;
            door.UnlockDoor();
            Assert.That(door.IsDoorLocked, Is.EqualTo(false));
        }

        [Test]
        public void TestDoorOpen()
        {
            // Arrange
            var message = false;
            door.DoorOpened();

            // Act
            door.DoorEvent += (sender, args) => { message = true; };
            door.DoorClosed();

            // Assert
            Assert.That(message, Is.True);
        }

        [Test]
        public void TestDoorClose()
        {
            // Arrange
            var message = false;
            door.DoorClosed();
            // Act
            door.DoorEvent += (sender, args) => { message = true; };
            door.DoorOpened();
            // Assert
            Assert.That(message, Is.True);
        }
    }
}