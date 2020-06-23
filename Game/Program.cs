using System;
using OpenTK;

namespace Game
{
    class Program
    {
        float[] verticles =
        {
            -0.5f, -0.5f, 0.0f,
            0.5f, -0.5f, 0.0f,
            0.0f, 0.5f, 0.0f
        };

        static void Main(string[] args)
        {
            using (Game game = new Game(600, 600, "FirstWindow")) 
            {


                game.Run(60.0);
            }
            
        }
    }
}
