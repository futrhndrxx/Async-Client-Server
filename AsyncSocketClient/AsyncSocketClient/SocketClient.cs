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


        // Parse IP Addrress, if not valid, write to Client console and return false

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

        // Parse Port Number, if not valid, write to Client console and return false

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
                    // End Connection
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
                    // Network stream to write data
                    StreamWriter clientStreamWriter = new StreamWriter(client.GetStream());
                    
                    // data will be flushed from the buffer to the stream after each write operation, but the encoder state will not be flushed
                    clientStreamWriter.AutoFlush = true;
                    
                   //  Asynchronously writes a sequence of bytes to the current stream
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
               // Start an asynchronous request for a connection to the remote host
                await client.ConnectAsync(serverIPAddress, serverPort);
                // Once connection is made
                Console.WriteLine(string.Format("Connected to server IP/Port: {0} / {1}",
                    serverIPAddress, serverPort));


                // Parse Data sent from Server
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
