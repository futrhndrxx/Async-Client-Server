using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketAsync
{
    public class SocketClient
    {
        IPAddress serverIPAddress;
        int serverPort;
        TcpClient client;
        public SocketClient()
        {
            serverIPAddress = null;
            serverPort = -1;
            client = null;
        }
        public IPAddress ServerIPAddress
        {
            get
            {
                return serverIPAddress;
            }
        }

        public int ServerPort
        {
            get
            {
                return serverPort;
            }
        }

        public bool SetServerIPAddress(string _IPAddressServer)
        {
            IPAddress ipaddr = null;

            if (!IPAddress.TryParse(_IPAddressServer, out ipaddr))
            {
                Console.WriteLine("Invalid server IP supplied.");
                return false;
            }

            serverIPAddress = ipaddr;

            return true;
        }

        public bool SetPortNumber(string _ServerPort)
        {
            int portNumber = 0;

            if (!int.TryParse(_ServerPort.Trim(), out portNumber))
            {
                Console.WriteLine("Invalid port number supplied, return.");
                return false;
            }

            serverPort = portNumber;

            return true;
        }

        public void CloseAndDisconnect()
        {
            if (client != null)
            {
                if (client.Connected)
                {
                    client.Close();
                }
            }
        }

        public async Task SendToServer(string strInputUser)
        {
            if (string.IsNullOrEmpty(strInputUser))
            {
                Console.WriteLine("Empty string supplied to send.");
                return;
            }

            if (client != null)
            {
                if (client.Connected)
                {
                    StreamWriter clientStreamWriter = new StreamWriter(client.GetStream());
                    clientStreamWriter.AutoFlush = true;

                    await clientStreamWriter.WriteAsync(strInputUser);
                    Console.WriteLine("Data sent...");
                }
            }

        }

        public async Task ConnectToServer()
        {
            if (client == null)
            {
                client = new TcpClient();
            }

            try
            {
                await client.ConnectAsync(serverIPAddress, serverPort);
                Console.WriteLine(string.Format("Connected to server IP/Port: {0} / {1}",
                    serverIPAddress, serverPort));

                ReadDataAsync(client);
            }
            catch (Exception excp)
            {
                Console.WriteLine(excp.ToString());
                throw;
            }
        }

        private async Task ReadDataAsync(TcpClient mClient)
        {
            try
            {
                StreamReader clientStreamReader = new StreamReader(mClient.GetStream());
                char[] buff = new char[64];
                int readByteCount = 0;

                while (true)
                {
                    readByteCount = await clientStreamReader.ReadAsync(buff, 0, buff.Length);

                    if (readByteCount <= 0)
                    {
                        Console.WriteLine("Disconnected from server.");
                        client.Close();
                        break;
                    }
                    Console.WriteLine(string.Format("Received bytes: {0} - Message: {1}",
                        readByteCount, new string(buff)));

                    Array.Clear(buff, 0, buff.Length);
                }
            }
            catch (Exception excp)
            {
                Console.WriteLine(excp.ToString());
                throw;
            }
        }
    }
}
