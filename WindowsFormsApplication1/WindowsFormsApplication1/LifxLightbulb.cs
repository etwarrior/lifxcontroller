using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIFXController
{
    struct lifx_host_info
    {
        public float signal;
        public UInt32 tx;
        public UInt32 rx;
        public UInt16 reserved;
    }

    struct lifx_host_firmware
    {
        public UInt64 build;
        public UInt64 reserved;
        public UInt32 version;
    }

    struct lifx_version
    {
        public UInt32 vendor;
        public UInt32 product;
        public UInt32 version;
    }

    struct lifx_info
    {
        public UInt64 time;
        public UInt64 uptime;
        public UInt64 downtime;
    }

    struct lifx_location
    {
        public byte[] location;
        public char[] label;
        public UInt64 updated_at;
    }

    struct lifx_group
    {
        public byte[] group;
        public char[] label;
        public UInt64 updated_at;
    }

    struct lifx_hsbk
    {
        public UInt16 hue;
        public UInt16 saturation;
        public UInt16 brightness;
        public UInt16 kelvin;
    }

    struct lifx_light_color
    {
        public byte reserved;
        public lifx_hsbk hsbk;
        public UInt32 duration;
    }

    struct lifx_light_state
    {
        public lifx_hsbk hsbk;
        public UInt16 reserved;
        public UInt16 power;
        public char[] label;
        public UInt64 reserved2;
    }

    struct lifx_light_power
    {
        public UInt16 level;
        public UInt32 duration;
    }

    class LifxLightbulb : IEquatable<LifxLightbulb>
    {
        public byte[] address;
        public lifx_host_info host_info;
        public lifx_host_firmware host_firmware;
        public lifx_host_info wifi_info;
        public lifx_host_firmware wifi_firmware;
        public UInt16 power;
        public char[] label;
        public lifx_version version;
        public lifx_info info;
        public lifx_location location;
        public lifx_group group;
        public lifx_light_state light_state;     

        public LifxLightbulb(byte[] _address)
        {
            this.address = _address;
        }

        public bool Equals(LifxLightbulb other)
        {
            int i;
            if (other == null) return false;
            for(i=0; i<8; ++i)
            {
                if (this.address[i] != other.address[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
