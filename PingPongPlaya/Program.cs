using System;

namespace PingPongPlaya
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new PingPongPlayaGame())
                game.Run();
        }
    }
}
