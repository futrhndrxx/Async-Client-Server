using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketAsync
{
    public class SocketServer
    {
        IPAddress ipAddress;
        int portNumber;
        TcpListener listener;

        //Boolean keeps server running
        public bool KeepRunning { get; set; }

        // List of all clients currently connected to the server
        List<TcpClient> connectedClients;

        
        public SocketServer()
        {
            connectedClients = new List<TcpClient>();
        }
        public async Task StartServer(string screenResolution)
        {
            ipAddress = IPAddress.Any;
            portNumber = 23000;
            Console.WriteLine(string.Format("IP Address: {0} - Port: {1}", ipAddress.ToString(), portNumber));

            listener = new TcpListener(ipAddress, portNumber);

            try
            {
                // Start listening for incoming connections
                listener.Start();
                KeepRunning = true;
                Console.WriteLine("Listening");

                while (KeepRunning)
                {
                    // Accept Incoming Client Connection Requests
                    var returnedByAccept = await listener.AcceptTcpClientAsync();

                    // Add to the list of connencted Clients
                    connectedClients.Add(returnedByAccept);

                    Console.WriteLine(
                        string.Format("Client connected successfully, count: {0} - {1}",
                        connectedClients.Count, returnedByAccept.Client.RemoteEndPoint)
                        );

                    // Send Data to Client
                    SendToAll(screenResolution);
                    // Get Data from Client
                    TakeCareOfTCPClient(returnedByAccept);
                }

            }
            catch (Exception excp)
            {
                Debug.WriteLine(excp.ToString());
            }
        }

        private async void TakeCareOfTCPClient(TcpClient paramClient)
        {
            NetworkStream stream = null;
            StreamReader reader = null;

            try
            {
                // Stream of Data send by Client
                stream = paramClient.GetStream();

                reader = new StreamReader(stream);

                // Creates buff to hold client input 
                // *** will have to change to an array that can handle 2D texture Data ***
                char[] buff = new char[64];

                while (KeepRunning)
                {
                    Console.WriteLine("*** Ready to Read ***");

                    int nRet = await reader.ReadAsync(buff, 0, buff.Length);

                   Console.WriteLine("Returned: " + nRet);

                    // Client has disconnected
                    if (nRet == 0)
                    {
                        RemoveClient(paramClient);

                        Console.WriteLine("Socket disconnected");
                        break;
                    }

                    // Transform array of unicode characters to a string
                    string receivedText = new string(buff);

                    Console.WriteLine("*** RECEIVED: " + receivedText);

                    // Cleary buff array to ensure future data is not effected
                    Array.Clear(buff, 0, buff.Length);


                }

            }
            catch (Exception excp)
            {
                RemoveClient(paramClient);
                System.Diagnostics.Debug.WriteLine(excp.ToString());
            }

        }

        private void RemoveClient(TcpClient paramClient)
        {
            // If client is in the array of connected clients ** if statement to ensure error is not raised if not in array
            if (connectedClients.Contains(paramClient))
            {
                // Remove from List of Connected Clients
                connectedClients.Remove(paramClient);
               
                Console.WriteLine(String.Format("Client removed, count: {0}", connectedClients.Count));
            }
        }


        // Currently Sends text to all connected clients ... Other than Tecture Map, Do all clients get the same data?? (i.e. 3d Object, Screen Resolution) 
        public async void SendToAll(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            try
            {
                byte[] buffMessage = Encoding.ASCII.GetBytes(message);

                foreach (TcpClient c in connectedClients)
                {
                    c.GetStream().WriteAsync(buffMessage, 0, buffMessage.Length);
                }
            }
            catch (Exception excp)
            {
                Debug.WriteLine(excp.ToString());
            }

        }
    }
}







   
