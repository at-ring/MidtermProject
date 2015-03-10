using System;

namespace MidtermProject
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Plinko game = new Plinko())
            {
                game.Run();
            }
        }
    }
#endif
}

