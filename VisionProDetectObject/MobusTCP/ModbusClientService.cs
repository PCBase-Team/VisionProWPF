using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NModbus;

namespace MobusTCP
{
    public class ModbusClientService
    {
        public byte SlaveAddress { get; set; }
        public ushort   StartRegister { get; set; }     
        public ushort NumberRegister { get; set; }
        public ModbusClientService()
        {
        }
        private TcpClient _tcpClient;
        private IModbusMaster _master;
        private string _ip;
        private int _port;
        public IModbusMaster Master
        {
            get { return _master; }
            set { _master = value; }
        }


        public bool IsConnected => _tcpClient != null && _tcpClient.Connected;

        public async Task<bool> ConnectAsync(string ip, int port)
        {
            _ip = ip;
            _port = port;
            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync(ip, port);

            var factory = new ModbusFactory();
            _master = factory.CreateMaster(_tcpClient);

            return true;
        }


        public void Disconnect()
        {
            _master?.Dispose();
            _tcpClient?.Close();
            _tcpClient = null;
            _master = null;
        }
        public void Dispose()
        {
            Disconnect();
        }
        public EventHandler<ModbusDataEventArgs> DataRefresh;
        private CancellationTokenSource _cts;

        public void StartPolling()
        {
            _cts = new CancellationTokenSource();
            Task.Run(async () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    ushort[] Data = await Master.ReadHoldingRegistersAsync(SlaveAddress, StartRegister, NumberRegister);
                    DataRefresh?.Invoke(this, new ModbusDataEventArgs(Data));
                    await Task.Delay(200);
                }
            }, _cts.Token);
        }

        public void StopPolling() => _cts?.Cancel();
    }
}
