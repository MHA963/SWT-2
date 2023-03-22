using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChargingStation.lib.Interfaces
{
    public class IDisplay 
    {


        void TilslutTelefon();
        void IndlaesRFID();
        void TilslutningsFejl();
        void LadeskabOptaget();
        void RFIDFejl();

        void FjernTelefon();

        void TelefonTilsluttet();
        void RFIDLåst();

        void OpladerFejl();
        void DørLaest();



    }
}
