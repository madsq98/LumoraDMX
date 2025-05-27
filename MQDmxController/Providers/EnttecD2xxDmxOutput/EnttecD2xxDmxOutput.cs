using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTD2XX_NET;
using MQDmxController.Abstractions;
using MQDmxController.Types;

namespace MQDmxController.Providers.EnttecD2xxDmxOutput
{
    public class EnttecD2xxDmxOutput : IDmxOutput
    {
        private FTDI _device = new FTDI();

        public List<ProviderDevice> GetDevices()
        {
            var returnDevices = new List<ProviderDevice>();

            uint deviceCount = 0;
            var ftStatus = _device.GetNumberOfDevices(ref deviceCount);

            if(ftStatus != FTDI.FT_STATUS.FT_OK || deviceCount == 0)
            {
                return returnDevices;
            }

            var deviceList = new FTDI.FT_DEVICE_INFO_NODE[deviceCount];
            ftStatus = _device.GetDeviceList(deviceList);

            if (ftStatus != FTDI.FT_STATUS.FT_OK)
                return returnDevices;

            for (uint i = 0; i < deviceCount; i++)
            {
                returnDevices.Add(new ProviderDevice
                {
                    Identifier = deviceList[i].SerialNumber,
                    Name = deviceList[i].Description
                });
            }

            return returnDevices;
        }

        public void Initialize(DmxOutputOptions options)
        {
            if(options is not EnttecD2xxDmxOutputOptions opts)
            {
                throw new ArgumentException("Invalid options");
            }

            uint deviceCount = 0;
            var status = _device.GetNumberOfDevices(ref deviceCount);
            if (status != FTDI.FT_STATUS.FT_OK || deviceCount == 0)
                throw new Exception("No FTDI devices found");

            status = _device.OpenBySerialNumber(opts.SerialNumber);
            if (status != FTDI.FT_STATUS.FT_OK)
                throw new Exception("Failed to open FTDI device");

            // Standard DMX settings
            _device.SetBaudRate(255000);
            _device.SetDataCharacteristics(8, 1, 0); // 8 data bits, 1 stop bit, no parity
            _device.SetFlowControl(FTDI.FT_FLOW_CONTROL.FT_FLOW_NONE, 0x11, 0x13);
            _device.Purge(FTDI.FT_PURGE.FT_PURGE_RX | FTDI.FT_PURGE.FT_PURGE_TX);
        }

        public void SendFrame(byte[] dmxData)
        {
            if (dmxData.Length != 512)
                throw new ArgumentException("DMX data must be 512 bytes");

            byte[] packet = BuildEnttecPacket(dmxData);

            uint bytesWritten = 0;
            var status = _device.Write(packet, packet.Length, ref bytesWritten);

            if (status != FTDI.FT_STATUS.FT_OK || bytesWritten != packet.Length)
                throw new Exception("Failed to write DMX frame");
        }

        public void Shutdown()
        {
            _device.Close();
        }

        private byte[] BuildEnttecPacket(byte[] dmx)
        {
            int dataLength = 513; // Start code + 512 channels
            var packet = new byte[1 + 1 + 2 + dataLength + 1];

            packet[0] = 0x7E; // Start of message
            packet[1] = 0x06; // Label: Send DMX Packet
            packet[2] = (byte)(dataLength & 0xFF);        // LSB of length
            packet[3] = (byte)((dataLength >> 8) & 0xFF); // MSB of length
            packet[4] = 0x00; // Start code (always 0 for DMX)

            Array.Copy(dmx, 0, packet, 5, 512); // Channel data
            packet[packet.Length - 1] = 0xE7;   // End of message

            return packet;
        }
    }
}
