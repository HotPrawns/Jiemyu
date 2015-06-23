using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessServer.Server;

namespace ChessServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var srv = new TcpServer(ChessServer.Properties.Settings.Default.Port);
            srv.Start();

            // Loop until server is closed
            while (srv.Running) { }
        }
    }
}
