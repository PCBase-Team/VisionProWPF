using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobusTCP
{
    public class ModbusDataEventArgs : EventArgs
    {
        public ushort[] Data { get; }
        public ModbusDataEventArgs(ushort[] data)
        {
            Data = data;
        }
    }
}
