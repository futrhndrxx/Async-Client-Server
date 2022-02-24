using SocketAsync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace AsyncSocketServer
{
    class Program
    {
       
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("exit");

        }
        static async Task Main(string[] args)
        {

          

           
           // string width = "";
           // string height = "";
           // width += Screen.PrimaryScreen.WorkingArea.Width;
           // height += Screen.PrimaryScreen.WorkingArea.Height;
        
            string screenWidth = Screen.PrimaryScreen.Bounds.Width.ToString();
            string screenHeight = Screen.PrimaryScreen.Bounds.Height.ToString();
            string resolution = string.Format("Screen Resolution: Width: {0} x Height: {1}", screenWidth, screenHeight);


            SocketServer server = new SocketServer();
            await server.StartServer(resolution);
            string input = "";
            while (input != "exit") { 
                input = Console.ReadLine();
                if (input != "exit") 
                { 
                    server.KeepRunning = false;
                }
            }
        }

        
    }
}
