using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NModbus;
using VisionProDetectObject.Camera;

namespace VisionProDetectObject.Modbus
{
    internal class ModbusTCPClient : IDisposable
    {
        private static readonly Lazy<ModbusTCPClient> lazy =
        new Lazy<ModbusTCPClient>(() => new ModbusTCPClient());
        public static ModbusTCPClient Instance { get { return lazy.Value; } }
        private ModbusTCPClient()
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

        public async Task<ushort[]> ReadHoldingRegistersAsync(byte slaveId, ushort startAddress, ushort numberOfPoints)
        {
            if (_master == null)
                throw new InvalidOperationException("Not connected.");

            // NModbus không có async cho ReadHoldingRegisters; dùng Task.Run để không block
            return await Task.Run(() => _master.ReadHoldingRegisters(slaveId, startAddress, numberOfPoints));
        }

        public async Task WriteSingleRegisterAsync(byte slaveId, ushort registerAddress, ushort value)
        {
            if (_master == null)
                throw new InvalidOperationException("Not connected.");

            await Task.Run(() => _master.WriteSingleRegister(slaveId, registerAddress, value));
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
    }
}
