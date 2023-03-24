using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargingStation.lib.Interfaces;

namespace ChargingStation.lib.Simulators
{
    public class LogFile : ILog
    {
        public string fileName { set; get; }
        public void WriteLogEntry(string message, int id)
        {
            using var writer = File.AppendText(fileName);
            writer.WriteLine(DateTime.Now + ": {0}: {1}", message, id);
        }

        public void WriteLogEntry(string message)
        {
            using var writer = File.AppendText(fileName);
            writer.WriteLine(DateTime.Now + ": {0}", message);
        }

        public LogFile(string fileName)
        {
            this.fileName = fileName;

        }

    }
}
