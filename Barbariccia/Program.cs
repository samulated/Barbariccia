using SunshineConsole;
using OpenTK.Graphics;
using OpenTK.Input;

namespace Barbariccia
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleWindow console = new ConsoleWindow(32, 80, "Sunshine Console Hello World");

            // console.Write(12, 28, "Hello World!", Color4.Lime);

            // Lawn
            for (int i = 2; i < 30; i++)
            {
                for (int j = 2; j < 78; j++)
                {
                    console.Write(i, j, "#", Color4.LawnGreen);
                }
            }

            // House
            //  Walls
            console.Write(18, 5, "  ", Color4.SandyBrown, Color4.SandyBrown);
            console.Write(18, 8, "      ", Color4.SandyBrown, Color4.SandyBrown);
            console.Write(18, 15, "   ", Color4.SandyBrown, Color4.SandyBrown);

            // Sidewalk



            // Street




            while (!console.KeyPressed && console.WindowUpdate())
            { 
                
            }
        }
    }
}
