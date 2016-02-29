using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;

namespace LIFXController
{
    public partial class MainForm : Form
    {
        private int BROADCAST_PORT = 56700;
        private int LOCAL_PORT = 56701;
        private IPAddress BROADCAST_ADDR = IPAddress.Parse("255.255.255.255");
        private IPAddress LOCAL_ADDR = IPAddress.Parse("0.0.0.0");
        private List<LifxLightbulb> lightbulbs = new List<LifxLightbulb>();
        private Socket sock;
        private IPEndPoint bcastep;

        struct rgb_color
        {
            public UInt16 r;
            public UInt16 g;
            public UInt16 b;
        }

        struct hsv_color
        {
            public UInt16 h;
            public UInt16 s;
            public UInt16 v;
        }

        private hsv_color RbgToHsv(rgb_color rgb)
        {
            hsv_color hsv = new hsv_color();
            UInt16 rgbMin, rgbMax;

            rgbMin = rgb.r < rgb.g ? (rgb.r < rgb.b ? rgb.r : rgb.b) : (rgb.g < rgb.b ? rgb.g : rgb.b);
            rgbMax = rgb.r > rgb.g ? (rgb.r > rgb.b ? rgb.r : rgb.b) : (rgb.g > rgb.b ? rgb.g : rgb.b);

            hsv.v = rgbMax;
            if (hsv.v == 0)
            {
                hsv.h = 0;
                hsv.s = 0;
                return hsv;
            }

            hsv.s = (UInt16)(255 * (rgbMax - rgbMin) / hsv.v);
            if (hsv.s == 0)
            {
                hsv.h = 0;
                return hsv;
            }

            if (rgbMax == rgb.r)
                hsv.h = (UInt16)(0 + 43 * (rgb.g - rgb.b) / (rgbMax - rgbMin));
            else if (rgbMax == rgb.g)
                hsv.h = (UInt16)(85 + 43 * (rgb.b - rgb.r) / (rgbMax - rgbMin));
            else
                hsv.h = (UInt16)(171 + 43 * (rgb.r - rgb.g) / (rgbMax - rgbMin));

            return hsv;
        }

        public MainForm()
        {
            InitializeComponent();
            sock = null;
            IPEndPoint ep = new IPEndPoint(LOCAL_ADDR, LOCAL_PORT);
            bcastep = new IPEndPoint(BROADCAST_ADDR, BROADCAST_PORT);
            byte[] buf;
            sock = new Socket(SocketType.Dgram, ProtocolType.IP);
            sock.EnableBroadcast = true;
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            try
            {
                sock.Bind(ep);
            }
            catch (Exception e)
            {
                Console.WriteLine("Bind Winsock Error: " + e.ToString());
                return;
            }
            buf = new byte[1024];
            Byte[] tmp = new Byte[8]; 
            //{ 0x24, 0, 0, 0x34, 0x1c, 0xba, 0x62, 0x70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x65, 0, 0, 0 };
            int bytes = 0;
            LifxMessage discover = new LifxGetServiceMessage(tmp, true, false);
            Byte[] hdr = discover.header.PacketSerialize();
            try
            {
                sock.SendTo(hdr, bcastep);
            }
            catch(Exception e)
            {
                Console.WriteLine("Send Winsock Error: " + e.ToString());
            }
            do
            {
                try
                {
                    //bytes = sock.Receive(buf, buf.Length, 0);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Recv Winsock Error: " + e.ToString());
                    return;
                }
                discover = new LifxStateServiceMessage(buf);
                LifxLightbulb bulb = new LifxLightbulb(discover.header._target);
                if (!lightbulbs.Contains(bulb))
                {
                    GetLightbulbInfo(bulb);
                    lightbulbs.Add(bulb);
                    return;
                }
            } while (bytes > 0);
        }

        private void GetLightbulbInfo(LifxLightbulb bulb)
        {
            GetBulbHostInfo(bulb);
            GetBulbHostFirmware(bulb);
            GetBulbWifiInfo(bulb);
            GetBulbWifiFirmware(bulb);
            GetBulbPower(bulb);
            GetBulbLabel(bulb);
            GetBulbVersion(bulb);
            GetBulbInfo(bulb);
            GetBulbLocation(bulb);
            GetBulbGroup(bulb);
            GetBulbLight(bulb);
        }

        private void GetBulbHostInfo(LifxLightbulb bulb)
        {

        }

        private void GetBulbHostFirmware(LifxLightbulb bulb)
        {

        }

        private void GetBulbWifiInfo(LifxLightbulb bulb)
        {

        }

        private void GetBulbWifiFirmware(LifxLightbulb bulb)
        {

        }

        private void GetBulbPower(LifxLightbulb bulb)
        {

        }

        private void GetBulbLabel(LifxLightbulb bulb)
        {

        }

        private void GetBulbVersion(LifxLightbulb bulb)
        {

        }

        private void GetBulbInfo(LifxLightbulb bulb)
        {

        }

        private void GetBulbLocation(LifxLightbulb bulb)
        {

        }

        private void GetBulbGroup(LifxLightbulb bulb)
        {

        }

        private void GetBulbLight(LifxLightbulb bulb)
        {

        }

        private void OffButton_Click(object sender, EventArgs e)
        {
            LifxMessage power = new LifxLightSetPowerMessage(lightbulbs[0].address, false, false, (UInt32)Convert.ToInt32("0"), false);
            byte[] hdr = power.header.PacketSerialize();
            byte[] body = power.BodySerialize();
            byte[] message = new byte[hdr.Length + body.Length];
            Array.Copy(hdr, message, hdr.Length);
            Array.Copy(body, 0, message, hdr.Length, body.Length);
            sock.SendTo(message, bcastep);
        }

        private void OnButton_Click(object sender, EventArgs e)
        {
            LifxMessage power = new LifxLightSetPowerMessage(lightbulbs[0].address, false, false, (UInt32)Convert.ToInt32("0"), true);
            byte[] hdr = power.header.PacketSerialize();
            byte[] body = power.BodySerialize();
            byte[] message = new byte[hdr.Length + body.Length];
            Array.Copy(hdr, message, hdr.Length);
            Array.Copy(body, 0, message, hdr.Length, body.Length);
            sock.SendTo(message, bcastep);
        }

        private void COLOR_Click(object sender, EventArgs e)
        {
            this.colorDialog1.ShowDialog();
            Color test = this.colorDialog1.Color;
            rgb_color rgb = new rgb_color();
            hsv_color hsv;
            lifx_hsbk hsbk = new lifx_hsbk();
            rgb.r = test.R;
            rgb.g = test.G;
            rgb.b = test.B;
            hsv = RbgToHsv(rgb);
            hsbk.hue = hsv.h;
            hsbk.saturation = hsv.s;
            hsbk.brightness = hsv.v;
            hsbk.kelvin = 10000;
            LifxMessage color = new LifxLightSetColorMessage(lightbulbs[0].address, false, false, hsbk, (UInt32)Convert.ToInt32("0"));
            byte[] hdr = color.header.PacketSerialize();
            byte[] body = color.BodySerialize();
            byte[] message = new byte[hdr.Length + body.Length];
            Array.Copy(hdr, message, hdr.Length);
            Array.Copy(body, 0, message, hdr.Length, body.Length);
            sock.SendTo(message, bcastep);
        }

        private void SendMessage(LifxMessage message, bool ack)
        {
            byte[] hdr = message.header.PacketSerialize();
            byte[] body = message.BodySerialize();
            byte[] buffer = new byte[hdr.Length + body.Length];
            Array.Copy(hdr, buffer, hdr.Length);
            Array.Copy(body, 0, buffer, hdr.Length, body.Length);
            sock.SendTo(buffer, bcastep);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var confirmation_box_result = MessageBox.Show("This will clear all entries.", 
                "Clear Entries", 
                MessageBoxButtons.OKCancel);

            if (confirmation_box_result == DialogResult.OK)
            {
                while (actionsList.Items.Count > 0)
                {
                    actionsList.Items.Remove(actionsList.Items[0]);
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var open_result = openFileDialog.ShowDialog();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var save_results = saveFileDialog.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var save_box_result = MessageBox.Show("Do you want to save your changes?",
                "Save?",
                MessageBoxButtons.YesNoCancel);

            if (save_box_result == DialogResult.Yes)
            {

            }
            else if (save_box_result == DialogResult.No)
            {

            }
        }
    }
}
