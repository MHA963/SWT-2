using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargingStation.lib.Interfaces;


namespace ChargingStation.lib.Simulators
{
    public class Display : IDisplay
    {
        public void TilslutTelefon()
        {
            Console.WriteLine("Tilslut telefon");
        }
         
        public void IndlaesRFID()
        {
            Console.WriteLine("Skabet er låst, telefonen lades nu op. Brug RFID til at låse op");
        }

        public void TilslutningsFejl()
        {
            Console.WriteLine("Tilslutnings Fejl");
        }

        public void LadeskabOptaget()
        {
            Console.WriteLine("Ladeskab optaget");
        }

        public void RFIDFejl()
        {
            Console.WriteLine("RFID fejl");
        }

        public void FjernTelefon()
        {
            Console.WriteLine("Fjern telefon");
        }

        public void TelefonTilsluttet()
        {
            Console.WriteLine("Telefonen er tilsluttet");
        }

        public void RFIDLåst()
        {
            Console.WriteLine("Scan dit RFID tag, for at låse skabet");
        }

        public void OpladerFejl()
        {
            Console.WriteLine("Der er fejl med opladeren");
        }

        public void DørLaest()
        {
            Console.WriteLine("Døren er låst og din telefon lades. Brug dit RFID tag til at låse op");
        }
    }
}
