using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingStation.lib.Interfaces
{
    public interface ILog
    {

        string fileName { get; set; }
        void WriteLogEntry(string message, int id);
        void WriteLogEntry(string message);

    }
}
