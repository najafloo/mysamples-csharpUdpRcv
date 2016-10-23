using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace recieveUdp
{
    public partial class Form1 : Form
    {
        private UdpClient _client;
        private int Port;
        private bool allowshowdisplay = false;


        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(allowshowdisplay ? value : allowshowdisplay);
        }

        public Form1()
        {
            Port = 5005;
            //Client uses as receive udp client
            _client = new UdpClient(Port);

            try
            {
                _client.BeginReceive(new AsyncCallback(recv), null);

            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
        }

        //CallBack
        private void recv(IAsyncResult res)
        {
            string title;
            string body;

            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, Port);          

            byte[] received = _client.EndReceive(res, ref RemoteIpEndPoint);

            //Process codes
            Console.Write(Encoding.UTF8.GetString(received));
            //MessageBox.Show(Encoding.UTF8.GetString(received));
            _client.BeginReceive(new AsyncCallback(recv), null);

            title = "وضعیت دستشویی";
            body = Encoding.UTF8.GetString(received);
       
            if (title != null)
            {
                notifyIcon1.BalloonTipTitle = title;
            }

            if (body != null)
            {
                if(body == "open")
                    notifyIcon1.BalloonTipText = "اشغال است";

                else if (body == "close")
                    notifyIcon1.BalloonTipText = "آزاد است";
            }

            notifyIcon1.ShowBalloonTip(30000);
            //_client.BeginReceive(new AsyncCallback(recv), null);
        }
    }
}
