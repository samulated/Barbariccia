using SunshineConsole;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.ES20;
using OpenTK.Graphics.OpenGL;

namespace Barbariccia
{
    // THINGS SECTION
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

        public bool CanMove()
        {
            return !_blocking;
        }

        public string LookAt()
        { 
            return _description;
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
    class Sand : Things
    {
        public Sand(int x, int y)
        {
            _char = '.';
            _mainCol = Color4.Yellow;
            _backCol = Color4.Black;
            _posX = x;
            _posY = y;
            _description = "A patch of dry, gritty sand.";
            _blocking = false;
        }
    }
    class PondWater : Things
    {
        public PondWater(int x, int y)
        {
            _char = '~';
            _mainCol = Color4.SkyBlue;
            _backCol = Color4.Blue;
            _posX = x;
            _posY = y;
            _description = "A shallow patch of cool blue water.";
            _blocking = true;
        }
    }
    class Dirt : Things
    {
        public Dirt(int x, int y)
        {
            _char = '.';
            _mainCol = Color4.SaddleBrown;
            _backCol = Color4.Black;
            _posX = x;
            _posY = y;
            _description = "An exposed patch of loamy earth.";
            _blocking = false;
        }
    }
    class Hedge : Things
    {
        public Hedge(int x, int y)
        {
            _char = '#';
            _mainCol = Color4.ForestGreen;
            _backCol = Color4.Black;
            _posX = x;
            _posY = y;
            _description = "A dense, bushy hedge, hard to traverse.";
            _blocking = true;
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
            _blocking = false;
        }
    }
    // THINGS SECTION

    class Player
    {
        // Player Data
        string _name;

        // World Appearance Data
        int _posX;
        int _posY;
        Color4 _main;
        Color4 _back;
        char _char;

        public Player(int x, int y)
        {
            _name = "Testin Nameus";

            _posX = x;
            _posY = y;

            _main = Color4.White;
            _back = Color4.Black;
            _char = '@';
        }

        public bool MoveTo(int x, int y, Things[,] t)
        {
            if (_posX + x < 0 || _posX + x >= t.GetLength(1)
            ||  _posY + y < 0 || _posY + y >= t.GetLength(0))
                return false;

            if (t[_posY + y, _posX + x].CanMove())
            {
                _posX = _posX + x;
                _posY = _posY + y;
                return true;
            }
            return false;
        }

        public int[] GetLocation()
        {
            return new int[] { _posX, _posY };
        }


        public void Render(ConsoleWindow c, int xOffset, int yOffset)
        {
            c.Write(_posX + xOffset, _posY + yOffset, _char, _main, _back);
        }
    }

    class UI
    {
        ConsoleWindow _console;
        int _screenX;
        int _screenY;

        Color4 _frameColor;
        Color4 _textColor;
        Color4 _highlightColor;
        Color4 _backgroundColor;

        string _text;

        public UI(int x, int y, ConsoleWindow c)
        {
            _console = c;
            _screenX = x;
            _screenY = y;

            _frameColor = Color4.Gray;
            _textColor = Color4.White;
            _highlightColor = Color4.Magenta;
            _backgroundColor = Color4.Black;

            _text = string.Empty;
        }
        public void UpdateInfo(string s)
        {
            _text = s;
        }

        public void ClearInfo()
        {
            _text = string.Empty;
        }

        public void Render()
        {
            for(int i = 0; i < _screenX; i++)
            {
                
                _console.Write(0, i, '=', _frameColor);
                _console.Write(22, i, '=', _frameColor);
                for (int j = 23; j < _screenY; j++)
                {
                    _console.Write(j, i, ' ', _textColor, _backgroundColor);
                }

                _console.Write(25, 5, _text, _textColor);
            }
        }
    }

    class Program
    {
        static Things[,] DefaultWorld()
        {
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

            // Hedges
            for (int j = 3; j <= 21; j++)
            {
                world[24, j] = new Hedge(j, 24);
                world[34, j] = new Hedge(j, 34);
            }
            for (int i = 25; i < 34; i++)
            {
                world[i, 3] = new Hedge(3, i);
            }
            for (int i = 25; i < 27; i++)
            {
                world[i, 21] = new Hedge(21, i);
            }
            for (int i = 28; i <= 31; i++)
            {
                world[i, 21] = new Hedge(21, i);
            }
            world[33, 21] = new Hedge(21, 33);

            // Dirt Patch
            for (int j = 21; j < 24; j++)
            {
                world[27, j] = new Dirt(j, 27);
                world[32, j] = new Dirt(j, 32);
            }
            for (int j = 15; j < 21; j++)
            {
                world[30, j] = new Dirt(j, 30);
            }
            for (int i = 31; i < 34; i++)
            {
                world[i, 15] = new Dirt(15, i);
            }

            //      Roads
            for (int i = 20; i < 55; i++)
            {
                for (int j = 24; j < 27; j++)
                {
                    world[i, j] = new Dirt(j, i);
                }
            }

            for (int i = 15; i < 21; i++)
            {
                for (int j = 0; j < world.GetLength(1); j++)
                {
                    world[i, j] = new Dirt(j, i);
                }
            }

            // BEACH
            // Sand
            for (int i = 64; i < world.GetLength(0); i++)
            {
                for (int j = 0; j < world.GetLength(1); j++)
                {
                    world[i, j] = new Sand(j, i);
                }
            }

            for (int i = 62; i < 64; i++)
            {
                for (int j = 1; j < world.GetLength(1); j++)
                {
                    world[i, j] = new Sand(j, i);
                }
            }

            for (int i = 61; i < 62; i++)
            {
                for (int j = 2; j < 19; j++)
                {
                    world[i, j] = new Sand(j, i);
                }
            }
            for (int i = 60; i < 61; i++)
            {
                for (int j = 2; j < 18; j++)
                {
                    world[i, j] = new Sand(j, i);
                }
            }

            for (int i = 59; i < 60; i++)
            {
                for (int j = 3; j < 17; j++)
                {
                    world[i, j] = new Sand(j, i);
                }
            }

            // Water
            for (int i = 70; i < world.GetLength(0); i++)
            {
                for (int j = 0; j < world.GetLength(1); j++)
                {
                    world[i, j] = new PondWater(j, i);
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

            // Silo hack
            world[3, 4] = new Hedge(4, 3);
            world[4, 4] = new Hedge(4, 4);
            world[5, 4] = new Hedge(4, 5);

            return world;
        }

        static void Main(string[] args)
        {
            int screenX = 80;
            int screenY = 32;

            ConsoleWindow console = new ConsoleWindow(screenY, screenX, "BarbaricciaRL");

            UI ui = new UI(screenX, screenY, console);

            Things[,] world = DefaultWorld();


            int worldOffsetX = 2;
            int worldOffsetY = 3;

            Player player = new Player(10, 10);

            bool running = true;

            while (!console.KeyPressed && console.WindowUpdate() && running)
            {
                // Get Input
                if (console.KeyPressed)
                {
                    Key key = console.GetKey();

                    if (key == Key.Escape)
                        running = false;

                    if (key == Key.L)
                    {
                        // Look around
                        int[] loc = player.GetLocation();
                        ui.UpdateInfo(world[loc[1], loc[0]].LookAt());
                    }

                    // Movement
                    if (key == Key.W)
                        player.MoveTo(-1, 0, world);
                    if (key == Key.A)
                        player.MoveTo(0, -1, world);
                    if (key == Key.S)
                        player.MoveTo(1, 0, world);
                    if (key == Key.D)
                        player.MoveTo(0, 1, world);
                }

                //  Render Stuff
                //      World
                foreach(Things t in world)
                {
                    t.Render(console, worldOffsetX, worldOffsetY);
                }
                //      Player
                player.Render(console, worldOffsetX, worldOffsetY);

                // hacky 'walk under' layer (Silo)
                console.Write(4, 6, "===", Color4.LightSteelBlue);
                console.Write(5, 6, "\\ /", Color4.LightSteelBlue);
                console.Write(6, 6, "|^|", Color4.LightSteelBlue);

                // UI
                ui.Render();
            }
        }
    }
}
