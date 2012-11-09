using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public partial class Client : Form
    {
        ASCIIEncoding encoding = new ASCIIEncoding();
        IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2000);
        Socket sock;

        public Client()
        {
            InitializeComponent();
        }

        private void Client_Load(object sender, EventArgs e)
        {
            conBut.Enabled = true;
            button3.Enabled = false;
        }

        private void conBut_Click(object sender, EventArgs e)
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            sock.Connect(ipEnd);
            byte[] buffer = new byte[32];
            sock.Receive(buffer);
            listBox1.Items.Add(encoding.GetString(buffer));
            conBut.Enabled = false;
            button3.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] mabyte = encoding.GetBytes(textBox1.Text);
            sock.Send(mabyte);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sock.Close();
            sock.Dispose();
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            conBut.Enabled = true;
            button3.Enabled = false;
        }
    }
}
