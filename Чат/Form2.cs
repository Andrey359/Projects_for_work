using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Чат
{
    public partial class Form2 : Form
    {
        private static Socket socket;
        private static ListBox listBox;
        private static bool server;
        private static List<Socket> clients = new List<Socket>();
        private string name;
        public static int users = 0;
        public Form2(bool server, int port, string name)
        {
            Form2.server = server;
            this.name = name;
            InitializeComponent();
            listBox = listBox1;
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint endpoint = new IPEndPoint(ip, port);
            socket = new Socket(

                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.IP
            );

            if (server)
            {
                socket.Bind(endpoint);


                Thread serverThread = new Thread(new ThreadStart(serverListen));
                serverThread.IsBackground = true;
                serverThread.Start();
            }
            else
            {
                socket.Connect(endpoint);
                Thread listenThread = new Thread(new ThreadStart(listen));
                listenThread.IsBackground = true;
                listenThread.Start();
            }
        }

        private static void listen()
        {

            while (true)
            {
                byte[] bytes = new byte[1024];
                int l = socket.Receive(bytes);
                if (l > 0)
                {
                    string text = Encoding.UTF8.GetString(bytes);
                    text = text.TrimEnd('\0');
                    if (!text.Equals($"/clients"))
                    {
                        listBox.Items.Add(text);
                    }
                }
            }
        }

        private static void sendToEveryone(byte[] message)
        {
            foreach (Socket client in clients)
            {
                client.Send(message);
            }
        }

        private static void sendToEveryone(string message)
        {
            sendToEveryone(Encoding.UTF8.GetBytes(message));
        }

        private static void serverListen()
        {
            socket.Listen(100);
            while (true)
            {
                Socket clientSocket = socket.Accept();
                sendToEveryone(clientSocket.ToString());
                clients.Add(clientSocket);
                Thread thread = new Thread(new ThreadStart(() =>
                {
                    clientListen(clientSocket);

                }));
                thread.IsBackground = true;
                thread.Start();

            }
        }

        private static void clientListen(Socket socket)
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                int l = socket.Receive(bytes);
                if (l > 0)
                {
                    string text = Encoding.UTF8.GetString(bytes);
                    text = text.TrimEnd('\0');
                    if (text.Equals($"/clients"))
                    {
                        socket.Send(Encoding.UTF8.GetBytes(string.Format($"Число подключенных клиентов: {clients.Count}")));
                    }
                    else
                    {
                        listBox.Items.Add(text);
                    }
                }
                sendToEveryone(bytes);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_Test_For_Server(bool server, string message)
        {
            if (server)
            {
                sendToEveryone(message);
                listBox1.Items.Add(message);
            }
            else
            {
                socket.Send(Encoding.UTF8.GetBytes(message));
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            if (text.Length > 0)
            {
                if (text.Equals("/clients"))
                {
                    string message = text;
                    button1_Click_Test_For_Server(server, message);
                }
                else
                {
                    string message = name + ": " + text;
                    button1_Click_Test_For_Server(server, message);
                }
            }
        }
    }
}

