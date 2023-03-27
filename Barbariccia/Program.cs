using SunshineConsole;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.ES20;

namespace Barbariccia
{
    class Things
    {
        protected char _char;
        protected Color4 _mainCol;
        protected Color4 _backCol;
        protected int _posX;
        protected int _posY;
        protected string _description;
        protected bool _blocking;

        public void Render(ConsoleWindow c, int xOffset, int yOffset)
        {
            c.Write(_posX + xOffset, _posY + yOffset, _char, _mainCol, _backCol);
        }
    }

    class Lawn : Things
    {
        public Lawn(int x, int y)
        {
            _char = '.';
            _mainCol = Color4.LawnGreen;
            _backCol = Color4.Black;
            _posX = x;
            _posY = y;
            _description = "A patch of lush, green lawn.";
            _blocking = false;
        }
    }
    class Floor : Things
    {
        public Floor(int x, int y)
        {
            _char = ' ';
            _mainCol = Color4.Black;
            _backCol = Color4.Ivory;
            _posX = x;
            _posY = y;
            _description = "A clean square of linoleum flooring.";
            _blocking = false;
        }
    }
    class Wall : Things
    {
        public Wall(int x, int y)
        {
            _char = ' ';
            _mainCol = Color4.Black;
            _backCol = Color4.SandyBrown;
            _posX = x;
            _posY = y;
            _description = "A brown brick wall.";
            _blocking = true;
        }
    }
    class Door : Things
    {
        public Door(int x, int y)
        {
            _char = '=';
            _mainCol = Color4.SaddleBrown;
            _backCol = Color4.Black;
            _posX = x;
            _posY = y;
            _description = "A brown wooden door.";
            _blocking = true;
        }
    }

    class Program
    {
        

        static void Main(string[] args)
        {
            ConsoleWindow console = new ConsoleWindow(32, 80, "Sunshine Console Hello World");
            Things[,] world = new Things[74, 28];
            // console.Write(12, 28, "Hello World!", Color4.Lime);

            // Lawn
            for (int i = 0; i < 74; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    world[i, j] = new Lawn(j, i);
                }
            }

            // HOUSE START
            //  Floors
            for (int i = 25; i < 34; i++)
            {
                for (int j = 10; j < 15; j++)
                {
                    world[i, j] = new Floor(j, i);
                }
            }
            for (int i = 25; i < 30; i++)
            {
                for (int j = 15; j < 20; j++)
                {
                    world[i, j] = new Floor(j, i);
                }
            }
            //  Walls
            //      Horizontals
            for (int i = 25; i < 34; i++)
            {
                world[i, 10] = new Wall(10, i);
            }
            for (int i = 25; i < 30; i++)
            {
                world[i, 20] = new Wall(20, i);
            }
            for (int i = 30; i < 34; i++)
            {
                world[i, 14] = new Wall(14, i);
            }
            //      Verticals
            for (int j = 10; j < 20; j++)
            {
                world[25, j] = new Wall(j, 25);
            }
            for (int j = 10; j < 15; j++)
            {
                world[30, j] = new Wall(j, 30);
                world[33, j] = new Wall(j, 33);
            }
            for (int j = 14; j < 20; j++)
            {
                world[29, j] = new Wall(j, 29);
            }
            // Doors
            world[28, 10] = new Door(10, 28);   // North Door
            world[27, 20] = new Door(20, 27);   // South Door
            world[30, 12] = new Door(12, 30);   // Interior Door
            // HOUSE END

            int worldOffsetX = 2;
            int worldOffsetY = 3;


            int playerX = 10;
            int playerY = 10;

            bool running = true;

            while (!console.KeyPressed && console.WindowUpdate())
            {
                // Get Input
                if (console.KeyPressed)
                {
                    Key key = console.GetKey();

                    if (key == Key.Escape)
                        running = false;

                    if (key == Key.W)
                        playerY--;
                    if (key == Key.A)
                        playerX--;
                    if (key == Key.S)
                        playerY++;
                    if (key == Key.D)
                        playerX++;
                }

                //  Render Stuff
                //      World
                foreach(Things t in world)
                {
                    t.Render(console, worldOffsetX, worldOffsetY);
                }
                //      Player
                console.Write(playerY, playerX, "@", Color4.White);
            }
        }
    }
}
