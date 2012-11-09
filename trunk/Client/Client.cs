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
using System.Text;
using System.Threading;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            /* Load listener component with: Thread listener = new Thread(listen); // Start the listening thread */
        }

        static void listen()
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            Console.WriteLine("Welcome to Josh's humble client.");
            IPEndPoint ipEnd = new IPEndPoint(ip, 2000);
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            bool connected = false;

            while (!connected)
            {
                sock.Connect(ipEnd);
                if (sock.Connected)
                {
                    connected = true;
                    byte[] buffer = new byte[256];
                    sock.Receive(buffer);
                    Console.WriteLine(encoding.GetString(buffer));
                }
                else
                {
                    connected = false;
                    Console.WriteLine("Could not connect, is the server active?");
                    Thread.Sleep(1000);
                }
            }

            while (true)
            {
                Console.WriteLine("Please enter a message:\n");
                byte[] mabyte = encoding.GetBytes(Console.ReadLine());
                sock.Send(mabyte);
            }
        }
    }
}
