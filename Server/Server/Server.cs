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
        bool resfound = false;

        public Server()
        {
            Thread.CurrentThread.Name = "main";
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
            clientSock = sock.Accept();
            byte[] mabytes = encoding.GetBytes("Welcome to Josh's epic server.");
            clientSock.Send(mabytes);
            connected = true;
            dataGridView1.Rows.Add(Convert.ToString(clientSock.Handle), 1);
            comboBox1.Items.Add(Convert.ToString(clientSock.Handle));
            
        }

        private void listener()
        {
            if (clientSock.Connected)
            {
                // Listener function, this will listen for incoming messages and write them to our console
                byte[] buffer = new byte[32];
                int len = clientSock.Receive(buffer);
                if (len >= 1)
                {
                    resfound = messageConversion.convertMessage(encoding.GetString(buffer, 0, len));

                    if (!resfound)
                    {
                        // Invoke a new action to add items to listbox, must do because we are editing an object of another thread.
                        BeginInvoke(new Action(delegate()
                        {
                            listBox1.Items.Add("Incoming Message from " + Convert.ToString(clientSock.Handle) + ": " + encoding.GetString(buffer, 0, len));
                        }));
                    }
                    else
                    {
                        BeginInvoke(new Action(delegate()
                            {
                                listBox1.Items.Add("A solution was found to the suggested argument");
                            }));

                    }
                }
            }
            else
            {
                connected = false;
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
                Thread listen = new Thread(listener);
                listen.Name = "Listener";
                listen.Start();
            }
        }
    }
}
