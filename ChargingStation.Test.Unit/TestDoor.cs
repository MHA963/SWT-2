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

        public void TestDoorLock()
        {
            //Arrange
            door.UnlockDoor(); // unlock door

            //Act
            door.LockDoor(); // lock door

            //Assert
            Assert.That(door.IsDoorLocked == true); // confirm door is locked
        }

        public void TestDoorUnlock()
        {
            //Arrange
            door.LockDoor(); // lock door

            //Act
            door.UnlockDoor(); // unlock door

            //Assert
            Assert.That(door.IsDoorOpen == true); // confirm door is unlocked
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