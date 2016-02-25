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
    enum LifxMessageID
    {
        GET_SERVICE = 2,
        STATE_SERVICE = 3,
        GET_HOST_INFO = 12,
        STATE_HOST_INFO = 13,
        GET_HOST_FIRMWARE = 14,
        STATE_HOST_FIRMWARE = 15,
        GET_WIFI_INFO = 16,
        STATE_WIFI_INFO = 17,
        GET_WIFI_FIRMWARE = 18,
        STATE_WIFI_FIRMWARE = 19,
        GET_POWER = 20,
        SET_POWER = 21,
        STATE_POWER = 22,
        GET_LABEL = 23,
        SET_LABEL = 24,
        STATE_LABEL = 25,
        GET_VERSION = 32,
        STATE_VERSION = 33,
        GET_INFO = 34,
        STATE_INFO = 35,
        ACK = 45,
        GET_LOCATION = 48,
        STATE_LOCATION = 50,
        GET_GROUP = 51,
        STATE_GROUP = 53,
        ECHO_REQUEST = 58,
        ECHO_RESPONSE = 59,
        LIGHT_GET = 101,
        LIGHT_SET_COLOR = 102,
        LIGHT_STATE = 107,
        LIGHT_GET_POWER = 116,
        LIGHT_SET_POWER = 117,
        LIGHT_STATE_POWER = 118
    }

    enum LifxMessageSize
    {
        HEADER_SIZE = 36,
        GET_SERVICE = HEADER_SIZE,
        STATE_SERVICE = HEADER_SIZE + 5,
        GET_HOST_INFO = HEADER_SIZE,
        STATE_HOST_INFO = HEADER_SIZE + 14,
        GET_HOST_FIRMWARE = HEADER_SIZE,
        STATE_HOST_FIRMWARE = HEADER_SIZE + 20,
        GET_WIFI_INFO = HEADER_SIZE,
        STATE_WIFI_INFO = HEADER_SIZE + 14,
        GET_WIFI_FIRMWARE = HEADER_SIZE,
        STATE_WIFI_FIRMWARE = HEADER_SIZE + 20,
        GET_POWER = HEADER_SIZE,
        SET_POWER = HEADER_SIZE + 2,
        STATE_POWER = HEADER_SIZE + 2,
        GET_LABEL = HEADER_SIZE,
        SET_LABEL = HEADER_SIZE + 32,
        STATE_LABEL = HEADER_SIZE + 32,
        GET_VERSION = HEADER_SIZE,
        STATE_VERSION = HEADER_SIZE + 12,
        GET_INFO = HEADER_SIZE,
        STATE_INFO = HEADER_SIZE + 24,
        ACK = HEADER_SIZE,
        GET_LOCATION = HEADER_SIZE,
        STATE_LOCATION = HEADER_SIZE + 56,
        GET_GROUP = HEADER_SIZE,
        STATE_GROUP = HEADER_SIZE + 56,
        ECHO_REQUEST = HEADER_SIZE + 64,
        ECHO_RESPONSE = HEADER_SIZE + 64,
        LIGHT_GET = HEADER_SIZE,
        LIGHT_SET_COLOR = HEADER_SIZE + 13,
        LIGHT_STATE = HEADER_SIZE + 52,
        LIGHT_GET_POWER = HEADER_SIZE,
        LIGHT_SET_POWER = HEADER_SIZE + 6,
        LIGHT_STATE_POWER = HEADER_SIZE + 2
    }
}