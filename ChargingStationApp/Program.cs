using ChargingStation.lib;
using ChargingStation.lib.Interfaces;
using ChargingStation.lib.Simulators;

class Program
{

    static void Main(string[] args)
        {
        // Assemble your system here from all the classes
        Door door = new Door();
        RFidReader rfidReader = new RFidReader();
        Display display = new Display();
        ChargeControl chargeControl = new ChargeControl(display, new UsbChargerSimulator());
        LogFile log = new LogFile("ProgramLog.txt");

        var stationControl = new StationControl(rfidReader, chargeControl, door, display, log);

        bool finish = false;
            do
            {
                string input;
                System.Console.WriteLine("Indtast E, O, C, R: ");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;

                switch (input[0])
                {
                    case 'E':
                        finish = true;
                        break;

                    case 'O':
                        door.DoorOpened();
                        break;

                    case 'C':
                        door.DoorClosed();
                        break;

                    case 'R':
                        System.Console.WriteLine("Indtast RFID id: ");
                        string idString = System.Console.ReadLine();

                        int id = Convert.ToInt32(idString);
                        rfidReader.RfidDetected(id);
                        break;

                    default:
                        break;
                }

            } while (!finish);
        }
}
