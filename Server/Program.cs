using System;
using System.Threading.Tasks;

namespace SurvivalGame
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
                Server.GameServer server = new Server.GameServer();
                server.Start();
        }
    }
}
