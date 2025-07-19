using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Commands;
using MobusTCP;
using VisionProDetectObject.Modbus;


namespace VisionProDetectObject.ViewModels
{
    public class ModbusViewModel:BindableBase
    {

        private string _ipaddress;
        public string IpAddresss
        {
            get { return _ipaddress; }
            set { SetProperty(ref _ipaddress, value); }
        }

        private string _port;
        public string Port
        {
            get { return _port; }
            set { SetProperty(ref _port, value); }
        }
        public ICommand connectPLCCommand { get;private set; }
        public ICommand disconectPLCCommand { get;private set; }
        private readonly ModbusClientService _modbusClientService;

        public ModbusViewModel(ModbusClientService modbusClientService)
        {
            IpAddresss = "192.168.10.1";
            Port = "502";
            connectPLCCommand = new DelegateCommand(async() => { await connectPLCExecute(); });
            disconectPLCCommand = new DelegateCommand(disconectPLCExecute);
            _modbusClientService = modbusClientService;
        }

        private void disconectPLCExecute()
        {
            _modbusClientService.Disconnect();
            _modbusClientService.StopPolling();
        }

        private async Task connectPLCExecute()
        {
            await _modbusClientService.ConnectAsync(IpAddresss,Convert.ToInt32(Port));
            if (_modbusClientService.IsConnected)
                _modbusClientService.StartPolling();
        }
    }
}
