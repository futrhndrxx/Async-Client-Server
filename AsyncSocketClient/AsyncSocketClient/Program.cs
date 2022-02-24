using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketAsync;

namespace AsyncSocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a Client
            SocketClient client = new SocketClient();
            Console.WriteLine("*** Client ***");

            // Set Server IP and Port Number
            string strIPAddress = "127.0.0.1";
            string strPortInput = "23000";
            // Set Server IP and Port, if either are invalid, print an error to the Console
            if (!client.SetServerIPAddress(strIPAddress) ||
                    !client.SetPortNumber(strPortInput))
            {
                Console.WriteLine(
                    string.Format(
                        "Wrong IP Address or port number supplied - {0} - {1} - Press a key to exit",
                        strIPAddress,
                    strPortInput));
                Console.ReadKey();
                return;
            }

            // Connect to the Server
            client.ConnectToServer();

            string strInputUser = null;

            //// Temporary for Testing, will sooon replace with a Method that will send Texture instead of text
            do
            {
                // Get Input From Client ** Currently Text, will adjust to send Texture Mapping Data ***
                strInputUser = Console.ReadLine();

                if (strInputUser.Trim() != "quit")
                {
                    // send data to server
                    client.SendToServer(strInputUser);
                }
                else if (strInputUser.Equals("quit"))
                {
                    // Close client and disconnect connection
                    client.CloseAndDisconnect();
                }

            } while (strInputUser != "quit");
        }
    }
}
