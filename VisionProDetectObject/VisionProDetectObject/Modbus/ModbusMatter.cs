using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using NModbus;
//using Modbus.Device;
namespace VisionProDetectObject.Modbus
{
    public sealed class ModbusMatter
    {
        private static readonly Lazy<ModbusMatter> instance = new Lazy<ModbusMatter>(() => new ModbusMatter());

        private TcpClient tcpClient;
        private IModbusMaster master;
        private IModbusSlave slave;
        private string ip;
        private int port;
        private readonly object lockObj = new object();

        private ModbusMatter() { }

        public static ModbusMatter Instance => instance.Value;

        public async Task<bool> InitializeAsync(string ipAddress, int portNumber)
        {
            ip = ipAddress;
            port = portNumber;

            tcpClient = new TcpClient();
            try
            {
                await tcpClient.ConnectAsync(ip, port);
                var factory = new ModbusFactory();
                master = factory.CreateMaster(tcpClient);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kết nối Modbus TCP thất bại: " + ex.Message);
                return false;
            }
        }

        // Thread-safe bằng lock
        public async Task<bool[]> ReadCoilsAsync(byte slaveId, ushort startAddress, ushort numInputs)
        {
            return await Task.Run(() =>
            {
                lock (lockObj)
                {
                    return master.ReadCoils(slaveId, startAddress, numInputs);
                }
            });
        }

        public async Task<ushort[]> ReadHoldingRegistersAsync(byte slaveId, ushort startAddress, ushort numInputs)
        {
            return await Task.Run(() =>
            {
                lock (lockObj)
                {
                    return master.ReadHoldingRegisters(slaveId, startAddress, numInputs);
                }
            });
        }

        // ... Thêm các hàm ghi nếu cần, cũng lock(lockObj)

        public void Close()
        {
            tcpClient?.Close();
            tcpClient = null;
            master = null;
        }
    }
}
