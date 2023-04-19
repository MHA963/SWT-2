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
        public enum LadeskabState
        {
            Available,
            Locked,
            DoorOpen
        };

        // Her mangler flere member variable
        public LadeskabState _state;
        public IChargeControl _charger;
        public int _oldId;
        public IDoor _door;
        public IDisplay _display;
        public ILog _log;
        public IRfidReader _reader;
        public StationControl(IRfidReader reader, IChargeControl charger, IDoor door, IDisplay display, ILog logFile)
        {
            _state = LadeskabState.Available;
            _reader = reader;
            _charger = charger;
            _door = door;
            _display = display;
            _log = logFile;

            door.DoorEvent += DoorOpen;
            // Der er ingen events, når man låser døren
            //door.DoorEvent += DoorLocked;

            reader.RfidEvent += RfidDetected;

        }


        private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        

        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        public void RfidDetected(object source, RfidReader eventArgs)
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

                        // Hvorfor bruger I ikke ILog?
                        _log.WriteLogEntry("Skab er låst med rfid, " + eventArgs.Id);

                        _display.LadeskabOptaget();
                        _state = LadeskabState.Locked;
                    }
                    else
                    {
                        _display.OpladerFejl();
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

                        // Hvorfor bruger I ikke ILog
                        _log.WriteLogEntry("Skab låst op med RFID: " + eventArgs.Id);

                        _display.FjernTelefon();
                        _state = LadeskabState.Available;
                    }
                    else
                    {
                        _display.RFIDFejl();
                    }

                    break;
            }
        }

     


        //trigger til Dørenlåst

        // Flet denne metode sammen med DoorOpen, der er kun events på at
        // døren åbnes og lukkes
        //public void DoorLocked(object source, DoorEventArgs eventArgs)
        //{
        //    if (eventArgs.DoorIsOpen == true) return;

        //    switch (_state)
        //    {
        //        case LadeskabState.Available:
        //            // Ignore
        //            break;

        //        case LadeskabState.DoorOpen:

        //            if (_charger.IsConnected)
        //            {
        //                _charger.StartCharge();

        //                _log.WriteLogEntry("Skab låst med RFID: " + _oldId);

        //                Console.WriteLine("Din telefon oplader nu og er låst i skabet. Brug dit RFID tag til at låse op.");
        //                _state = LadeskabState.Locked;
        //                _display.RFIDLåst();
        //            }

        //            break;

        //        case LadeskabState.Locked:
        //            // ignore

        //            break;
        //    }

        //    Console.WriteLine("CURRENT STATE: " + _state);
        //}

        //trigger til Dørenåbnet
        public void DoorOpen(object source, DoorEventArgs eventArgs)
        {
            if (eventArgs.DoorIsOpen)
            {
                switch (_state)
                {
                    case LadeskabState.Available:
                        // Ignore
                        _display.TilslutTelefon();
                        _state = LadeskabState.DoorOpen;
                        _log.WriteLogEntry("Skab er åben");
                        break;
                    case LadeskabState.DoorOpen:

                        if (_charger.IsConnected)
                        {
                            _charger.StartCharge();
                            _state = LadeskabState.Locked;
                            _display.DørLaest();
                        }
                        else
                        {
                            _state = LadeskabState.Available;
                            _display.FjernTelefon();
                            _log.WriteLogEntry("Der er ikke tilsluttet en telefon til opladning. Skabet er åbent og låst op.");
                        }

                        break;
                    case LadeskabState.Locked:

                        //Ignore


                        break;


                }
            }

            Console.WriteLine("CURRENT STATE: " + _state);
        }

    }
}
