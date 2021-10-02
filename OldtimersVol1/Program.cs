using System;

namespace OldtimersVol1
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Demo())
                game.Run();
        }
    }
}
