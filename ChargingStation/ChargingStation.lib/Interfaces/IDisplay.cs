using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChargingStation.lib.Interfaces
{
    public class IDisplay 
    {


        public void TilslutTelefon();
        public void IndlaesRFID();
        public void TilslutningsFejl();
        public void LadeskabOptaget();
        public void RFIDFejl();

        public void FjernTelefon();

        public void TelefonTilsluttet();
        public void RFIDLåst();

        public void OpladerFejl();
        public void DørLaest();



    }
}
