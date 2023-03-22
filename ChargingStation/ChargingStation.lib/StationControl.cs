using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using ChargingStation.lib.Interfaces;
using ChargingStation.lib.Simulators;


namespace ChargingStation.lib
{
    public class StationControl
    {
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        private enum LadeskabState
        {
            Available,
            Locked,
            DoorOpen
        };

        // Her mangler flere member variable
        private LadeskabState _state;
        private IChargeControl _charger;
        private int _oldId;
        private IDoor _door;
        private IDisplay _display;
        private ILog _log;
        private IRfidReader _reader;
        public StationControl(IRfidReader reader, IChargeControl charger, IDoor door, IDisplay display, ILog logFile)
        {
            _state = LadeskabState.Available;
            _reader = reader;
            _charger = charger;
            _door = door;
            _display = display;
            _log = logFile;

            door.DoorEvent += DoorOpen;
            door.DoorEvent += DoorLocked;

            reader.RfidEvent += RfidDetected;

        }

        public bool IsConnected { get; set; }

        private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        // Her mangler constructor

        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        private void RfidDetected(object source, RfidReader eventArgs)
        {
            switch (_state)
            {
                case LadeskabState.Available:
                    // Check for ladeforbindelse
                    if (_charger.IsConnected)
                    {
                        _door.LockDoor();
                        _charger.StartCharge();
                        _oldId = eventArgs.Id;

                        using (var writer = File.AppendText(logFile))
                        {
                            writer.WriteLine(DateTime.Now + ": Skab låst med RFID: {0}", eventArgs.Id);
                        }

                        _display.LadeskabOptaget();
                        _state = LadeskabState.Locked;
                    }
                    else
                    {
                        Console.WriteLine("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                    }

                    break;

                case LadeskabState.DoorOpen:
                    // Ignore
                    break;

                case LadeskabState.Locked:
                    // Check for correct ID
                    if (eventArgs.Id == _oldId)
                    {
                        _charger.StopCharge();
                        _door.UnlockDoor();
                        using (var writer = File.AppendText(logFile))
                        {
                            writer.WriteLine(DateTime.Now + ": Skab låst op med RFID: {0}", eventArgs.Id);
                        }
                        _display.FjernTelefon();
                        _state = LadeskabState.Available;
                    }
                    else
                    {
                        Console.WriteLine("Forkert RFID tag");
                    }

                    break;
            }
        }

        // Her mangler de andre trigger handlere
        //trigger til Dørenlåst
        public void DoorLocked(object source, DoorEventArgs eventArgs)
        {
            if (eventArgs.DoorIsOpen == true) return;
            
            switch (_state)
            {
                case LadeskabState.Available:
                    // Ignore
                    break;

                case LadeskabState.DoorOpen:

                    if (_charger.IsConnected)
                    {
                        _charger.StartCharge();
                        using (var writer = File.AppendText(logFile))
                        {
                            writer.WriteLine(DateTime.Now + ": Skab låst med RFID: {0}", _oldId);
                        }

                        Console.WriteLine("Din telefon oplader nu og er låst i skabet. Brug dit RFID tag til at låse op.");
                        _state = LadeskabState.Locked;
                        _display.RFIDLåst();
                    }
                    
                    break;

                case LadeskabState.Locked:
                    // ignore

                    break;
            }
            Console.WriteLine("CURRENT STATE: " + _state);
        }

        //trigger til Dørenåbnet
        public void DoorOpen(object source, DoorEventArgs eventArgs)
        {
            if (eventArgs.DoorIsOpen == false) return;
            switch (_state)
            {
                case LadeskabState.Available:
                    // Ignore
                    _display.TilslutTelefon();
                    _state = LadeskabState.DoorOpen;
                    using (var writer = File.AppendText(logFile))
                    {
                        writer.WriteLine(DateTime.Now + "Skab er åben" );
                    }
                    break;
                case LadeskabState.DoorOpen:
                    // Ignore
                    break;
                case LadeskabState.Locked:
                   
                    //Ignore


                    break;
            }

        }

    }
}
