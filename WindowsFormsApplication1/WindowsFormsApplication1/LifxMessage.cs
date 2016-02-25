using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using System.IO;

namespace LIFXController
{
    [Serializable]
    public class PacketHdr
    {

        //Frame
        public UInt16 _size;
        public UInt16 _protocol;
        public UInt32 _source;
        //Frame Address
        public Byte[] _target;
        public Byte[] _reserved;
        public Byte _acks;
        public Byte _sequence;
        //Protocol
        public UInt64 _preserved;
        public UInt16 _type;
        public UInt16 _preserved2;

        public PacketHdr(UInt16 size, UInt32 source, Byte[] target, UInt16 type)
        {
            this._size = size;
            this._source = source;
            this._type = type;
            this._target = target;

            //Set by helper functions
            this._protocol = 0;
            this._acks = 0;

            //Always 0
            this._sequence = 0;
            this._preserved = 0;
            this._preserved2 = 0;
            this._reserved = new Byte[6];
        }

        public PacketHdr(PacketHdr copy)
        {
            this._size = copy._size;
            this._protocol = copy._protocol;
            this._source = copy._source;
            this._acks = copy._acks;
            this._sequence = copy._sequence;
            this._type = copy._type;
            this._target = copy._target;
            this._reserved = copy._reserved;
            this._preserved = copy._preserved;
            this._preserved2 = copy._preserved2;
        }

        public PacketHdr(byte[] buf)
        {
            this.PacketDeserialize(buf);
        }

        public void UpdateValues(UInt16 size, UInt32 source, Byte[] target, UInt16 type)
        {
            this._size = size;           
            this._source = source;          
            this._type = type;
            this._target = target;           

            //Set by helper functions
            this._protocol = 0;
            this._acks = 0;

            //Always 0
            this._sequence = 0;
            this._preserved = 0;
            this._preserved2 = 0;
            this._reserved = new Byte[6];
        }

        public void SetProtocol(byte tagged)
        {
            this._protocol = (UInt16)((tagged << 13) | (1 << 12) | 1024);
        }

        public void SetAcks(byte ack, byte resp)
        {
            this._acks = (byte)((ack << 1) | resp);
        }

        private static void SerializeTargetAddr(BinaryWriter writer, byte[] target)
        {
            int i;
            for (i = 0; i < target.Length; ++i)
            {
                writer.Write(target[i]);
            }
        }

        public byte[] PacketSerialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);

            writer.Write(this._size);
            writer.Write(this._protocol);
            writer.Write(this._source);
            SerializeTargetAddr(writer, this._target);
            writer.Write(this._reserved);
            writer.Write(this._acks);
            writer.Write(this._sequence);
            writer.Write(this._preserved);
            writer.Write(this._type);
            writer.Write(this._preserved2);
            return ms.ToArray();
        }

        private static byte[] DeserializeTargetAddr(BinaryReader reader)
        {
            int i;
            byte[] target = new byte[8];
            for (i = 0; i < 8; ++i)
            {
                target[i] = reader.ReadByte();
            }
            return target;
        }
        public void PacketDeserialize(byte[] packet)
        {
            MemoryStream ms = new MemoryStream(packet);
            BinaryReader reader = new BinaryReader(ms);
            this._size = reader.ReadUInt16();
            this._protocol = reader.ReadUInt16();
            this._source = reader.ReadUInt32();
            this._target = DeserializeTargetAddr(reader);
            this._reserved = reader.ReadBytes(6);
            this._acks = reader.ReadByte();
            this._sequence = reader.ReadByte();
            this._preserved = reader.ReadUInt64();
            this._type = reader.ReadUInt16();
            this._preserved2 = reader.ReadUInt16();
        }
    }

    abstract class LifxMessage
    {
        private static UInt16 sequence = 1;
        protected const int HEADER_SIZE = 36;

        public PacketHdr header;

        public LifxMessage (byte[] target, bool broadcast, UInt16 len, UInt16 type, bool ack, bool resp)
        {
            //Create an outgoing message to a particular target            
            this.header = new PacketHdr(len, ++sequence, target, type);
            if (broadcast)
            {
                Array.Clear(target, 0, target.Length);
                header.SetProtocol(1);
            }
            header.SetProtocol(0);
            header.SetAcks(Convert.ToByte(ack), Convert.ToByte(resp));
            
        }

        public LifxMessage(byte[] buffer)
        {
            //Create a message based on a received message
            byte[] header = new byte[HEADER_SIZE];
            Array.Copy(buffer, 0, header, 0, HEADER_SIZE);
            this.header = new PacketHdr(header);
        }

        protected byte[] CopyBody(byte[] buffer, UInt32 len)
        {
            byte[] body = new byte[len];
            Array.Copy(buffer, HEADER_SIZE, body, 0, len);
            return body;
        }

        abstract public byte[] BodySerialize();
    }

    class LifxGetServiceMessage : LifxMessage
    {
        public LifxGetServiceMessage(byte[] target, bool broadcast, bool ack) :
            base(target, broadcast, (UInt16)LifxMessageSize.GET_SERVICE, (UInt16)LifxMessageID.GET_SERVICE, ack, true)
        {            
            //Create an outgoing message to a particular target                        
        }

        public LifxGetServiceMessage(byte[] message) :
            base(message)
        {
            //Create a message based on a received message
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxStateServiceMessage : LifxMessage
    {
        public byte service;
        public UInt32 port;

        public LifxStateServiceMessage(byte[] message) :
            base(message)
        {
            //Create a message based on a received message
            byte[] body = CopyBody(message, (UInt32)LifxMessageSize.STATE_SERVICE - HEADER_SIZE);
            MemoryStream ms = new MemoryStream(body);
            BinaryReader reader = new BinaryReader(ms);
            this.service = reader.ReadByte();
            this.port = reader.ReadUInt32();
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxGetHostInfoMessage : LifxMessage
    {
        public LifxGetHostInfoMessage(byte[] target, bool broadcast, bool ack) :
            base(target, broadcast, (UInt16)LifxMessageSize.GET_HOST_INFO, (UInt16)LifxMessageID.GET_HOST_INFO, ack, false)
        {
            //Create an outgoing message to a particular target
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxStateHostInfoMessage : LifxMessage
    {
        public lifx_host_info host_info;

        public LifxStateHostInfoMessage(byte[] message) :
            base(message)
        {
            //Create a message based on a received message
            byte[] body = CopyBody(message, (UInt32)LifxMessageSize.STATE_HOST_INFO - HEADER_SIZE);
            MemoryStream ms = new MemoryStream(body);
            BinaryReader reader = new BinaryReader(ms);
            this.host_info = new lifx_host_info();
            this.host_info.signal = (float)reader.ReadInt32();
            this.host_info.tx = reader.ReadUInt32();
            this.host_info.rx = reader.ReadUInt32();
            this.host_info.reserved = reader.ReadUInt16();
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxGetHostFirmwareMessage : LifxMessage
    {
        public LifxGetHostFirmwareMessage(byte[] target, bool broadcast, bool ack) :
            base(target, broadcast, (UInt16)LifxMessageSize.GET_HOST_FIRMWARE, (UInt16)LifxMessageID.GET_HOST_FIRMWARE, ack, false)
        {
            //Create an outgoing message to a particular target
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxStateHostFirmwareMessage : LifxMessage
    {
        public lifx_host_firmware host_firmware;

        public LifxStateHostFirmwareMessage(byte[] message) :
            base(message)
        {
            //Create a message based on a received message
            byte[] body = CopyBody(message, (UInt32)LifxMessageSize.STATE_HOST_FIRMWARE - HEADER_SIZE);
            MemoryStream ms = new MemoryStream(body);
            BinaryReader reader = new BinaryReader(ms);
            this.host_firmware = new lifx_host_firmware();
            this.host_firmware.build = reader.ReadUInt64();
            this.host_firmware.reserved = reader.ReadUInt64();
            this.host_firmware.version = reader.ReadUInt32();
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxGetWifiInfoMessage : LifxMessage
    {
        public LifxGetWifiInfoMessage(byte[] target, bool broadcast, bool ack) :
            base(target, broadcast, (UInt16)LifxMessageSize.GET_WIFI_INFO, (UInt16)LifxMessageID.GET_WIFI_INFO, ack, false)
        {
            //Create an outgoing message to a particular target
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxStateWifiInfoMessage : LifxMessage
    {
        public lifx_host_info wifi_info;

        public LifxStateWifiInfoMessage(byte[] message) :
            base(message)
        {
            //Create a message based on a received message
            byte[] body = CopyBody(message, (UInt32)LifxMessageSize.STATE_WIFI_INFO - HEADER_SIZE);
            MemoryStream ms = new MemoryStream(body);
            BinaryReader reader = new BinaryReader(ms);
            this.wifi_info = new lifx_host_info();
            this.wifi_info.signal = (float)reader.ReadInt32();
            this.wifi_info.tx = reader.ReadUInt32();
            this.wifi_info.rx = reader.ReadUInt32();
            this.wifi_info.reserved = reader.ReadUInt16();
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxGetWifiFirmwareMessage : LifxMessage
    {
        public LifxGetWifiFirmwareMessage(byte[] target, bool broadcast, bool ack) :
            base(target, broadcast, (UInt16)LifxMessageSize.GET_WIFI_FIRMWARE, (UInt16)LifxMessageID.GET_WIFI_FIRMWARE, ack, false)
        {
            //Create an outgoing message to a particular target
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxStateWifiFirmwareMessage : LifxMessage
    {
        public lifx_host_firmware wifi_firmware;

        public LifxStateWifiFirmwareMessage(byte[] message) :
            base(message)
        {
            //Create a message based on a received message
            byte[] body = CopyBody(message, (UInt32)LifxMessageSize.STATE_HOST_FIRMWARE - HEADER_SIZE);
            MemoryStream ms = new MemoryStream(body);
            BinaryReader reader = new BinaryReader(ms);
            this.wifi_firmware = new lifx_host_firmware();
            this.wifi_firmware.build = reader.ReadUInt64();
            this.wifi_firmware.reserved = reader.ReadUInt64();
            this.wifi_firmware.version = reader.ReadUInt32();
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxGetPowerMessage : LifxMessage
    {
        public LifxGetPowerMessage(byte[] target, bool broadcast, bool ack) :
            base(target, broadcast, (UInt16)LifxMessageSize.GET_POWER, (UInt16)LifxMessageID.GET_POWER, ack, false)
        {
            //Create an outgoing message to a particular target
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxSetPowerMessage : LifxMessage
    {
        public UInt16 power;

        public LifxSetPowerMessage(byte[] target, bool broadcast, bool ack, bool turn_on) :
            base(target, broadcast, (UInt16)LifxMessageSize.SET_POWER, (UInt16)LifxMessageID.SET_POWER, ack, false)
        {
            //Create an outgoing message to a particular target
            if (turn_on)
            {
                this.power = 65535;
            }
            else
            {
                this.power = 0;
            }
        }

        public override byte[] BodySerialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);

            writer.Write(this.power);
            return ms.ToArray();
        }
    }

    class LifxStatePowerMessage : LifxMessage
    {
        public UInt16 power;

        public LifxStatePowerMessage(byte[] message) :
            base(message)
        {
            //Create a message based on a received message
            byte[] body = CopyBody(message, (UInt32)LifxMessageSize.STATE_POWER - HEADER_SIZE);
            MemoryStream ms = new MemoryStream(body);
            BinaryReader reader = new BinaryReader(ms);
            this.power = reader.ReadUInt16();
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxGetLabelMessage : LifxMessage
    {
        public LifxGetLabelMessage(byte[] target, bool broadcast, bool ack) :
            base(target, broadcast, (UInt16)LifxMessageSize.GET_LABEL, (UInt16)LifxMessageID.GET_LABEL, ack, false)
        {
            //Create an outgoing message to a particular target
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxSetLabelMessage : LifxMessage
    {
        public char[] label;

        public LifxSetLabelMessage(byte[] target, bool broadcast, bool ack, char[] _label) :
            base(target, broadcast, (UInt16)LifxMessageSize.SET_LABEL, (UInt16)LifxMessageID.SET_LABEL, ack, false)
        {
            //Create an outgoing message to a particular target
            this.label = _label;
        }

        public override byte[] BodySerialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);

            writer.Write(this.label);
            return ms.ToArray();
        }
    }

    class LifxStateLabelMessage : LifxMessage
    {
        public char[] label;

        public LifxStateLabelMessage(byte[] message) :
            base(message)
        {
            //Create a message based on a received message
            byte[] body = CopyBody(message, (UInt32)LifxMessageSize.STATE_LABEL - HEADER_SIZE);
            MemoryStream ms = new MemoryStream(body);
            BinaryReader reader = new BinaryReader(ms);
            this.label = reader.ReadChars(32);
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxGetVersionMessage : LifxMessage
    {        
        public LifxGetVersionMessage(byte[] target, bool broadcast, bool ack) :
            base(target, broadcast, (UInt16)LifxMessageSize.GET_VERSION, (UInt16)LifxMessageID.GET_VERSION, ack, false)
        {
            //Create an outgoing message to a particular target
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxStateVersionMessage : LifxMessage
    {
        public lifx_version version;

        public LifxStateVersionMessage(byte[] message) :
            base(message)
        {
            //Create a message based on a received message
            byte[] body = CopyBody(message, (UInt32)LifxMessageSize.STATE_VERSION - HEADER_SIZE);
            MemoryStream ms = new MemoryStream(body);
            BinaryReader reader = new BinaryReader(ms);
            this.version.vendor = reader.ReadUInt32();
            this.version.product = reader.ReadUInt32();
            this.version.version = reader.ReadUInt32();
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxGetInfoMessage : LifxMessage
    {
        public LifxGetInfoMessage(byte[] target, bool broadcast, bool ack) :
            base(target, broadcast, (UInt16)LifxMessageSize.GET_INFO, (UInt16)LifxMessageID.GET_INFO, ack, false)
        {
            //Create an outgoing message to a particular target
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxStateInfoMessage : LifxMessage
    {
        public lifx_info info;

        public LifxStateInfoMessage(byte[] message) :
            base(message)
        {
            //Create a message based on a received message
            byte[] body = CopyBody(message, (UInt32)LifxMessageSize.STATE_INFO - HEADER_SIZE);
            MemoryStream ms = new MemoryStream(body);
            BinaryReader reader = new BinaryReader(ms);
            this.info.time = reader.ReadUInt64();
            this.info.uptime = reader.ReadUInt64();
            this.info.downtime = reader.ReadUInt64();
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxAckMessage : LifxMessage
    {
        public LifxAckMessage(byte[] message) :
            base(message)
        {
            //Create a message based on a received message
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxGetLocationMessage : LifxMessage
    {
        public LifxGetLocationMessage(byte[] target, bool broadcast, bool ack) :
            base(target, broadcast, (UInt16)LifxMessageSize.GET_LOCATION, (UInt16)LifxMessageID.GET_LOCATION, ack, false)
        {
            //Create an outgoing message to a particular target
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxStateLocationMessage : LifxMessage
    {
        public lifx_location location;

        public LifxStateLocationMessage(byte[] message) :
            base(message)
        {
            //Create a message based on a received message
            byte[] body = CopyBody(message, (UInt32)LifxMessageSize.STATE_LOCATION - HEADER_SIZE);
            MemoryStream ms = new MemoryStream(body);
            BinaryReader reader = new BinaryReader(ms);
            this.location.location = reader.ReadBytes(16);
            this.location.label = reader.ReadChars(32);
            this.location.updated_at = reader.ReadUInt64();
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxGetGroupMessage : LifxMessage
    {
        public LifxGetGroupMessage(byte[] target, bool broadcast, bool ack) :
            base(target, broadcast, (UInt16)LifxMessageSize.GET_GROUP, (UInt16)LifxMessageID.GET_GROUP, ack, false)
        {
            //Create an outgoing message to a particular target
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxStateGroupMessage : LifxMessage
    {
        public lifx_group group;

        public LifxStateGroupMessage(byte[] message) :
            base(message)
        {
            //Create a message based on a received message
            byte[] body = CopyBody(message, (UInt32)LifxMessageSize.STATE_GROUP - HEADER_SIZE);
            MemoryStream ms = new MemoryStream(body);
            BinaryReader reader = new BinaryReader(ms);
            this.group.group = reader.ReadBytes(16);
            this.group.label = reader.ReadChars(32);
            this.group.updated_at = reader.ReadUInt64();
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxEchoRequestMessage : LifxMessage
    {
        public LifxEchoRequestMessage(byte[] target, bool broadcast, bool ack) :
            base(target, broadcast, (UInt16)LifxMessageSize.ECHO_REQUEST, (UInt16)LifxMessageID.ECHO_REQUEST, ack, false)
        {
            //Create an outgoing message to a particular target
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxEchoResponseMessage : LifxMessage
    {
        public LifxEchoResponseMessage(byte[] message) :
            base(message)
        {
            //Create a message based on a received message
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxLightGetMessage : LifxMessage
    {
        public LifxLightGetMessage(byte[] target, bool broadcast, bool ack) :
            base(target, broadcast, (UInt16)LifxMessageSize.LIGHT_GET, (UInt16)LifxMessageID.LIGHT_GET, ack, false)
        {
            //Create an outgoing message to a particular target
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxLightSetColorMessage : LifxMessage
    {
        public lifx_light_color color;

        public LifxLightSetColorMessage(byte[] target, bool broadcast, bool ack, lifx_hsbk hsbk, UInt32 duration) :
            base(target, broadcast, (UInt16)LifxMessageSize.LIGHT_SET_COLOR, (UInt16)LifxMessageID.LIGHT_SET_COLOR, ack, false)
        {
            //Create an outgoing message to a particular target
            this.color.duration = duration;
            this.color.hsbk = hsbk;
            this.color.reserved = 0;
        }

        public override byte[] BodySerialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);

            writer.Write(this.color.reserved);
            writer.Write(this.color.hsbk.hue);
            writer.Write(this.color.hsbk.saturation);
            writer.Write(this.color.hsbk.brightness);
            writer.Write(this.color.hsbk.kelvin);
            writer.Write(this.color.duration);
            return ms.ToArray();
        }
    }

    class LifxLightStateMessage : LifxMessage
    {
        public lifx_light_state state;

        public LifxLightStateMessage(byte[] message) :
            base(message)
        {
            //Create a message based on a received message
            byte[] body = CopyBody(message, (UInt32)LifxMessageSize.LIGHT_STATE - HEADER_SIZE);
            MemoryStream ms = new MemoryStream(body);
            BinaryReader reader = new BinaryReader(ms);
            this.state.hsbk.hue = reader.ReadUInt16();
            this.state.hsbk.saturation = reader.ReadUInt16();
            this.state.hsbk.brightness = reader.ReadUInt16();
            this.state.hsbk.kelvin = reader.ReadUInt16();
            this.state.reserved = reader.ReadUInt16();
            this.state.power = reader.ReadUInt16();
            this.state.label = reader.ReadChars(32);
            this.state.reserved2 = reader.ReadUInt64();
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxLightGetPowerMessage : LifxMessage
    {
        public LifxLightGetPowerMessage(byte[] target, bool broadcast, bool ack) :
            base(target, broadcast, (UInt16)LifxMessageSize.LIGHT_GET_POWER, (UInt16)LifxMessageID.LIGHT_GET_POWER, ack, false)
        {
            //Create an outgoing message to a particular target
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }

    class LifxLightSetPowerMessage : LifxMessage
    {
        public lifx_light_power power;

        public LifxLightSetPowerMessage(byte[] target, bool broadcast, bool ack, UInt32 _duration, bool turn_on) :
            base(target, broadcast, (UInt16)LifxMessageSize.LIGHT_SET_POWER, (UInt16)LifxMessageID.LIGHT_SET_POWER, ack, false)
        {
            //Create an outgoing message to a particular target
            this.power.duration = _duration;
            if (turn_on)
            {
                this.power.level = 65535;
            }
            else
            {
                this.power.level = 0;
            }
        }

        public override byte[] BodySerialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);

            writer.Write(this.power.level);
            writer.Write(this.power.duration);
            return ms.ToArray();
        }
    }

    class LifxLightStatePowerMessage : LifxMessage
    {
        public UInt16 power;

        public LifxLightStatePowerMessage(byte[] message) :
            base(message)
        {
            //Create a message based on a received message
            byte[] body = CopyBody(message, (UInt32)LifxMessageSize.LIGHT_STATE_POWER - HEADER_SIZE);
            MemoryStream ms = new MemoryStream(body);
            BinaryReader reader = new BinaryReader(ms);
            this.power = reader.ReadUInt16();
        }

        public override byte[] BodySerialize()
        {
            return null;
        }
    }
}
