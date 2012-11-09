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
using System.Threading;

namespace Tame
{
    public partial class Server : Form
    {
        // Declare an encoder, IP, receiver socket, and sender socket //
        ASCIIEncoding encoding = new ASCIIEncoding();
        IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, 2000);
        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        Socket clientSock;
        bool connected = false;
        string response;

        public Server()
        {
            InitializeComponent();
        }

        private void Server_Load(object sender, EventArgs e)
        {
            // Initialise the socket listener, begin the accept connection thread to allow clients to connect. Automatically does this on startup //
            sock.Bind(ipEnd);
            sock.Listen(100);
            listBox1.Items.Add("Binded, waiting for connection from client");
        }

        private void acceptConnection()
        {
            //while (!connected)
            //{
            // Accept the connection from a client, give that client a welcome message, and assign it to clientSock
            clientSock = sock.Accept();
            byte[] mabytes = encoding.GetBytes("Welcome to Josh's humble server.");
            clientSock.Send(mabytes);
            connected = true;
            //}
        }

        private string listener()
        {
            // Listener function, this will listen for incoming messages and write them to our console
            byte[] buffer = new byte[32];
            int len = clientSock.Receive(buffer);
            if (len >= 1)
            {
                return (encoding.GetString(buffer, 0, len));
            }
            else
            {
                return ("");
            }
        }
        
        private void conButton_Click(object sender, EventArgs e)
        {



            timerListen.Start();
            /*
            Thread accept = new Thread(acceptConnection);
            accept.Start();
            Thread listen = new Thread(listener);
            listen.Start();
            acceptConnection();
            listener();
             */
        }

        private void timerListen_Tick(object sender, EventArgs e)
        {
            if (!connected)
            {
                acceptConnection();
            }
            else
            {
                response = listener();
                if (response != "")
                {
                    listBox1.Items.Add(response);
                }
            }
        }
    }
}
