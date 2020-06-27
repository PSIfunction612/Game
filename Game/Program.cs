using System;
using OpenTK;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(600, 600, "FirstWindow")) 
            {
                game.Run(60.0);
            }
            
        }
    }
}
