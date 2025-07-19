using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace VisionProDetectObject.Modbus
{
    internal class ModbusPolling
    {
        public EventHandler<ModbusDataEventArgs> DataRefresh;
        private CancellationTokenSource _cts;

        public ModbusPolling()
        {
 
        }
        public void Start()
        {
            _cts = new CancellationTokenSource();
            Task.Run(async () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    ushort[] Data = await ModbusTCPClient.Instance.Master.ReadHoldingRegistersAsync(1, 0, 10);
                    DataRefresh?.Invoke(this, new ModbusDataEventArgs(Data));
                    await Task.Delay(200);
                }
            }, _cts.Token);
        }

        public void Stop() => _cts?.Cancel();
    }
}
