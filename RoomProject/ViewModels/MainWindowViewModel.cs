using RoomProject.Helpers;
using RoomProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomProject.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private double _temperature;
        public double Temperature
        {
            get => _temperature;
            set
            {
                _temperature = value;
                SetProperty<double>(ref _temperature, value);
            }
        }

        private double _humidity;
        public double Humidity
        {
            get => _humidity;
            set
            {
                _humidity = value;
                SetProperty<double>(ref _humidity, value);
            }
        }

        private double _luminosity;
        public double Luminosity
        {
            get => _luminosity;
            set
            {
                _luminosity = value;
                SetProperty<double>(ref _luminosity, value);
            }
        }

        private double _flame;
        public double Flame
        {
            get => _flame;
            set
            {
                _flame = value;
                SetProperty<double>(ref _flame, value);
            }
        }

        private string _statusMessage;

        public string StatusMessage
        {
            get => _statusMessage; 
            set 
            { 
                _statusMessage = value;
                SetProperty<string>(ref _statusMessage, value);
            }
        }

        public async Task StartMeasureAsync()
        {
            Action<string> statusMessage = (message) => StatusMessage = message;
            Action<DataPack> dataDisplay = (dataPack) =>
            {
                Temperature = dataPack.Temperature;
                Humidity = dataPack.Humidity;
                Luminosity = dataPack.Luminosity;
                Flame = dataPack.Flame;
            };
            var bluetoothModule = new BluetoothModule(statusMessage, dataDisplay);
            await bluetoothModule.Conect();
        }
    }
}
