using ChargingStation.lib.Simulators;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStation.Test.Unit
{
    public class TestDisplay
    {
        Display display;
        private StringWriter writer;

        [SetUp]

        public void Setup()
        {
            writer = new StringWriter();
            display = new Display();
            Console.SetOut(writer);
        }


        [Test]
        public void DisplayTestTilslutTelefon()
        {
            //Arrange
            var expectedresult = "Tilslut telefon";
            //Act
            display.TilslutTelefon();
            //Assert
            Assert.That(writer.ToString(), Contains.Substring(expectedresult));
        }

        [Test]
        public void DisplayTestIndlaesRFID()
        {
            //Arrange
            var expectedresult = "Skabet er låst, telefonen lades nu op. Brug RFID til at låse op";
            //Act
            display.IndlaesRFID();
            //Assert
            Assert.That(writer.ToString(), Contains.Substring(expectedresult));
        }

        [Test]
        public void DisplayTestTilslutningsFejl()
        {
            //Arrange
            var expectedresult = "Tilslutnings Fejl";
            //Act
            display.TilslutningsFejl();
            //Assert
            Assert.That(writer.ToString(), Contains.Substring(expectedresult));
        }

        [Test]
        public void DisplayTestLadeskabOptaget()
        {
            //Arrange
            var expectedresult = "Ladeskab optaget";
            //Act
            display.LadeskabOptaget();
            //Assert
            Assert.That(writer.ToString(), Contains.Substring(expectedresult));
        }

        [Test]
        public void DisplayTestRFIDFejl()
        {
            //Arrange
            var expectedresult = "RFID fejl";
            //Act
            display.RFIDFejl();
            //Assert
            Assert.That(writer.ToString(), Contains.Substring(expectedresult));
        }

        [Test]
        public void DisplayTestFjernTelefon()
        {
            //Arrange
            var expectedresult = "Fjern telefon";
            //Act
            display.FjernTelefon();
            //Assert
            Assert.That(writer.ToString(), Contains.Substring(expectedresult));
        }

        [Test]
        public void DisplayTestTelefonTilsluttet()
        {
            //Arrange
            var expectedresult = "Telefonen er tilsluttet";
            //Act
            display.TelefonTilsluttet();
            //Assert
            Assert.That(writer.ToString(), Contains.Substring(expectedresult));
        }

        [Test]
        public void DisplayTestRFIDLåst()
        {
            //Arrange
            var expectedresult = "Scan dit RFID tag, for at låse skabet";
            //Act
            display.RFIDLåst();
            //Assert
            Assert.That(writer.ToString(), Contains.Substring(expectedresult));
        }

        [Test]
        public void DisplayTestOpladerFejl()
        {
            //Arrange
            var expectedresult = "Der er fejl med opladeren";
            //Act
            display.OpladerFejl();
            //Assert
            Assert.That(writer.ToString(), Contains.Substring(expectedresult));
        }

        [Test]
        public void DisplayTestDørLaest()
        {
            //Arrange
            var expectedresult = "Døren er låst og din telefon lades. Brug dit RFID tag til at låse op";
            //Act
            display.DørLaest();
            //Assert
            Assert.That(writer.ToString(), Contains.Substring(expectedresult));
        }

    }
}
