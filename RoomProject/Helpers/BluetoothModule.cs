using System;
using System.Collections.Generic;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System.Threading.Tasks;
using RoomProject.Models;

namespace RoomProject.Helpers
{
    public class BluetoothModule
    {
        private readonly string _arduinoBoardName = "HC-05";
        private readonly int _packageStart = 36;
        private readonly int _packageEnd = 70;
        private Action<string> _statusMessage;
        private Action<DataPack> _dataDisplay;

        public BluetoothModule(Action<string> statusMessage, Action<DataPack> dataDisplay)
        {
            _statusMessage = statusMessage;
            _dataDisplay = dataDisplay;
        }

        internal async Task Conect()
        {
            BluetoothClient client = new BluetoothClient();
            _statusMessage("Searching devices...");
            IReadOnlyCollection<BluetoothDeviceInfo> devices = client.DiscoverDevices();
            _statusMessage("Found device");

            foreach (BluetoothDeviceInfo d in devices)
            {
                if (d.DeviceName.Equals(_arduinoBoardName))
                {
                    await EstablishConnection(d, client);
                    break;
                }
            }
        }

        private async Task EstablishConnection(BluetoothDeviceInfo device, BluetoothClient client)
        {
            var serviceClass = BluetoothService.SerialPort;

            if (device == null)
            {
                return;
            }

            try
            {
                if (!device.Connected)
                {
                    _statusMessage("Connecting to the device...");
                    client.Connect(device.DeviceAddress, serviceClass);
                    _statusMessage("Connected to the device.");

                    await ReadData(client);
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                _statusMessage("Connection failed!");
                client.Close();
                return;
            }
        }

        private async Task ReadData(BluetoothClient client)
        {
            while (!client.GetStream().DataAvailable)
            {
                await Task.Delay(100);
            }

            while (true)
            {
                while (client.GetStream().ReadByte() != _packageStart)
                {
                    await Task.Delay(100);
                }

                int temperature = client.GetStream().ReadByte();
                int humidity = client.GetStream().ReadByte();

                // build the number (each bype a digit)
                int digit;
                int luminosityNrDigits = client.GetStream().ReadByte();
                int luminosity = 0;

                while (luminosityNrDigits > 0)
                {
                    digit = client.GetStream().ReadByte();
                    luminosity = digit * (int)Math.Pow(10, luminosityNrDigits - 1) + luminosity;
                    luminosityNrDigits--;
                }

                int flameNrDigits = client.GetStream().ReadByte();
                int dg = flameNrDigits;
                int flame = 0;

                while (flameNrDigits > 0)
                {
                    digit = client.GetStream().ReadByte();
                    flame = digit * (int)Math.Pow(10, flameNrDigits - 1) + flame;
                    flameNrDigits--;
                }

                var dataPack = new DataPack(temperature, humidity, luminosity, flame);
                _dataDisplay(dataPack);

                //if (client.GetStream().ReadByte() != _packageEnd)
                //{
                //    Console.WriteLine("Error while reading...");
                //    Console.WriteLine($"flame digits:{dg}");
                //    Console.WriteLine($"flame:{flame}");
                //}
                await Task.Delay(2000);
            }
        }
    }
}
