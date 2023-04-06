using SunshineConsole;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.ES20;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System;

namespace Barbariccia
{
    class Observer
    {
        void OnNotify(Subject subject) { }

    }
    class Subject
    {
        void Attach(Observer observer) { }
        void Notify() {}
    }


    class GameTime // Governs in-game timer
    {
        private int _minutes;
        private int _days;
        private int _daysTotal;
        private int _months;
        private int _years;
        private TimeSpan _oldTickTspan;
        private int _tickTime; // for private timers (eg, plants tracking their own growth)
        private int _runningTime;
        private int _totalTime;

        public GameTime()
        {
            _oldTickTspan = TimeSpan.Zero;
        }

        public void Tick(TimeSpan t)
        {
            // Process tick time
            int speed = 1;
            if (_oldTickTspan != TimeSpan.Zero)  // if it's actually been set
            {
                _tickTime = (t - _oldTickTspan).Milliseconds;
            }
            else
            {
                _tickTime = t.Milliseconds;
            }
            _runningTime = _runningTime + _tickTime * speed;
            _totalTime = _totalTime + _tickTime * speed;
            

            // Convert to Minutes
            _minutes = _runningTime / 1000;

            // NEXT DAY CHECK
            if (_minutes >= 1140)
            {
                _days = _days + 1;
                _daysTotal = _daysTotal + 1;

                _minutes = _minutes - 1140;
                _runningTime = _runningTime - 1140000;

                if (_days >= 28)
                {
                    _months = _months + 1;
                    _days = _days - 28;
                    if (_months >= 12)
                    {
                        _years = _years + 1;
                        _months = _months - 12;
                    }
                }

                // Check for Special Events probably
                // Anything else that needs to happen Once Per Day
            }
            _oldTickTspan = t;
        }

        public void ApplyOffset (int minutes, int days, int months, int years)
        {
            _minutes = minutes;
            _runningTime = minutes * 1000;
            _days = days;
            _months = months;
            _years = years;
        }

        // Get Strings
        public string GetClock()
        {
            string timeDisplay = string.Empty;
            timeDisplay = (_minutes / 60).ToString(@"00") + ":" + (_minutes % 60).ToString(@"00");
            if (_minutes > 720)
                timeDisplay += " PM";
            else
                timeDisplay += " AM";

            return timeDisplay;
        }
        public string GetDayOfTheWeek()
        {
            switch (_days % 7)
            {
                case 0:
                    return "Sunday";
                    break;
                case 1:
                    return "Monday";
                    break;
                case 2:
                    return "Tuesday";
                    break;
                case 3:
                    return "Wednesday";
                    break;
                case 4:
                    return "Thursday";
                    break;
                case 5:
                    return "Friday";
                    break;
                case 6:
                    return "Saturday";
                    break;
                default:
                    break;
            }
            return "Error";
        }
        public string GetDate()
        {
            return GetDays() + "/" + GetMonths() + "/" + GetYears();
        }
        public string GetDays()
        {
            return (_days + 1).ToString(@"00");
        }
        public string GetMonths()
        {
            return (_months + 1).ToString(@"00");
        }
        public string GetYears()
        {
            return _years.ToString(@"00");
        }
        public string GetDebugTickTimeCounter()
        {
            return _tickTime.ToString();
        }
        public string GetDebugRunningTimeCounter()
        {
            return _runningTime.ToString();
        }
        public string GetDebugTotalTimeCounter()
        {
            return _totalTime.ToString();
        }
        // Get Numerical
        public int GetNumericalMinutes()
        {
            return _minutes;
        }
        public int GetNumericalHours()
        {
            return _minutes / 60;
        }
        public int GetNumericalDays()
        {
            return _days;
        }
        public int GetNumericalMonths()
        {
            return _months;
        }
        public int GetNumericalYears()
        {
            return _years;
        }
    }

    // THINGS SECTION
    enum Direction
    { Left, Right, Up, Down };

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
            c.Write(_posY + xOffset, _posX + yOffset, _char, _mainCol, _backCol);
        }

        public void Update()
        {

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


    // META-THINGS (inherited for functionality)

    class Tillable : Things
    {
        public void Till(Things[,] t)
        {
            // TODO: add more like animation and delay when tilling by hand (that's probably going to need an event system)

            t[_posX, _posY] = new TilledDirt(_posX, _posY);
        }
    }

    class Lawn : Tillable
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
    class DirtRoad : Things
    {
        public DirtRoad(int x, int y)
        {
            _char = '.';
            _mainCol = Color4.Khaki;
            _backCol = Color4.Black;
            _posX = x;
            _posY = y;
            _description = "The earth has been packed down from\nyears of use.";
            _blocking = false;
        }
    }
    class TilledDirt : Things
    {
        public TilledDirt(int x, int y)
        {
            _char = '.';
            _mainCol = Color4.SaddleBrown;
            _backCol = Color4.Black;
            _posX = x;
            _posY = y;
            _description = "A patch of ground that has recently been tilled.";
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

    class AncientWall : Things
    {
        public AncientWall(int x, int y)
        {
            _char = '#';
            _mainCol = Color4.Black;
            _backCol = Color4.DarkGray;
            _posX = x;
            _posY = y;
            _description = "Something tells you that there isn't much to see\nbeyond this ancient wall...";
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
        Direction _direction;

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

            _direction = Direction.Down;
        }

        public bool MoveTo(int x, int y, Things[,] t)
        {
            if (x < 0)
                _direction = Direction.Up;
            else if (x > 0)
                _direction = Direction.Down;
            else if (y < 0)
                _direction = Direction.Left;
            else if (y > 0)
                _direction = Direction.Right;

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

        public Direction GetDirection()
        {
            return _direction;
        }

        public Things GetTarget(Things[,] t)
        {
            int x = 0;
            int y = 0;

            switch (_direction)
            {
                case Direction.Up:
                    x = -1;
                    break;
                case Direction.Down:
                    x = 1;
                    break;
                case Direction.Left:
                    y = -1;
                    break;
                case Direction.Right:
                    y = 1;
                    break;
                default:
                    break;
            }

            if (_posX + x < 0 || _posX + x >= t.GetLength(1)
            || _posY + y < 0 || _posY + y >= t.GetLength(0))
            {
                return null;
            }
            return t[_posY + y, _posX + x];
        }

        public string LookAt(Things[,] t)
        {
            if(GetTarget(t) == null)
            {
                return "That doesn't look like much of anything...";
            }
            return GetTarget(t).LookAt();
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

        public void Render(Player p)
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

                _console.Write(28, 2, "O", p.GetDirection() == Direction.Up     ? Color4.Cyan  : Color4.White);
                _console.Write(29, 1, "O", p.GetDirection() == Direction.Left   ? Color4.Cyan  : Color4.White);
                _console.Write(29, 3, "O", p.GetDirection() == Direction.Right  ? Color4.Cyan  : Color4.White);
                _console.Write(30, 2, "O", p.GetDirection() == Direction.Down   ? Color4.Cyan  : Color4.White);
            }
        }
    }

    class Program
    {
        static Things[,] DefaultWorld()
        {
            int worldX = 128;
            int worldY = 128;
            int worldZ = 128; // currently unused

            Things[,] world = new Things[worldX, worldY];
            // console.Write(12, 28, "Hello World!", Color4.Lime);

            // Lawn
            for (int i = 0; i < worldX; i++)
            {
                for (int j = 0; j < worldY; j++)
                {
                    world[i, j] = new Lawn(i, j);
                }
            }

            // Hedges
            for (int j = 3; j <= 21; j++)
            {
                world[24, j] = new Hedge(24, j);
                world[34, j] = new Hedge(34, j);
            }
            for (int i = 25; i < 34; i++)
            {
                world[i, 3] = new Hedge(i, 3);
            }
            for (int i = 25; i < 27; i++)
            {
                world[i, 21] = new Hedge(i, 21);
            }
            for (int i = 28; i <= 31; i++)
            {
                world[i, 21] = new Hedge(i, 21);
            }
            world[33, 21] = new Hedge(33, 21);

            // Dirt Patch
            for (int j = 21; j < 24; j++)
            {
                world[27, j] = new DirtRoad(27, j);
                world[32, j] = new DirtRoad(32, j);
            }
            for (int j = 15; j < 21; j++)
            {
                world[30, j] = new DirtRoad(30, j);
            }
            for (int i = 31; i < 34; i++)
            {
                world[i, 15] = new DirtRoad(i, 15);
            }

            //      Roads
            for (int i = 20; i < 55; i++)
            {
                for (int j = 24; j < 27; j++)
                {
                    world[i, j] = new DirtRoad(i, j);
                }
            }

            for (int i = 15; i < 21; i++)
            {
                for (int j = 0; j < world.GetLength(1); j++)
                {
                    world[i, j] = new DirtRoad(i, j);
                }
            }

            // BEACH
            // Sand
            for (int i = 114; i < world.GetLength(0); i++)
            {
                for (int j = 0; j < world.GetLength(1); j++)
                {
                    world[i, j] = new Sand(i, j);
                }
            }

            for (int i = 112; i < 114; i++)
            {
                for (int j = 1; j < world.GetLength(1); j++)
                {
                    world[i, j] = new Sand(i, j);
                }
            }

            for (int i = 111; i < 112; i++)
            {
                for (int j = 2; j < 19; j++)
                {
                    world[i, j] = new Sand(i, j);
                }
            }
            for (int i = 110; i < 111; i++)
            {
                for (int j = 2; j < 18; j++)
                {
                    world[i, j] = new Sand(i, j);
                }
            }

            for (int i = 109; i < 110; i++)
            {
                for (int j = 3; j < 17; j++)
                {
                    world[i, j] = new Sand(i, j);
                }
            }

            // Water
            for (int i = 120; i < world.GetLength(0); i++)
            {
                for (int j = 0; j < world.GetLength(1); j++)
                {
                    world[i, j] = new PondWater(i, j);
                }
            }


            // HOUSE START
            //  Floors
            for (int i = 25; i < 34; i++)
            {
                for (int j = 10; j < 15; j++)
                {
                    world[i, j] = new Floor(i, j);
                }
            }
            for (int i = 25; i < 30; i++)
            {
                for (int j = 15; j < 20; j++)
                {
                    world[i, j] = new Floor(i, j);
                }
            }
            //  Walls
            //      Horizontals
            for (int i = 25; i < 34; i++)
            {
                world[i, 10] = new Wall(i, 10);
            }
            for (int i = 25; i < 30; i++)
            {
                world[i, 20] = new Wall(i, 20);
            }
            for (int i = 30; i < 34; i++)
            {
                world[i, 14] = new Wall(i, 14);
            }
            //      Verticals
            for (int j = 10; j < 20; j++)
            {
                world[25, j] = new Wall(25, j);
            }
            for (int j = 10; j < 15; j++)
            {
                world[30, j] = new Wall(30, j);
                world[33, j] = new Wall(33, j);
            }
            for (int j = 14; j < 20; j++)
            {
                world[29, j] = new Wall(29, j);
            }
            // Doors
            world[28, 10] = new Door(28, 10);   // North Door
            world[27, 20] = new Door(27, 20);   // South Door
            world[30, 12] = new Door(30, 12);   // Interior Door
            // HOUSE END

            // Test Tilled Earth

            world[27, 28] = new TilledDirt(27, 28);

            // Silo hack
            world[3, 4] = new Hedge(3, 4);
            world[4, 4] = new Hedge(4, 4);
            world[5, 4] = new Hedge(5, 4);

            return world;
        }

        static void Main(string[] args)
        {
            Stopwatch timer = new Stopwatch();
            GameTime gameTime = new GameTime();

            int timeOffset = 540;
            int dayOffset = 0;

            int screenX = 80;
            int screenY = 32;

            ConsoleWindow console = new ConsoleWindow(screenY, screenX, "BarbaricciaRL");

            UI ui = new UI(screenX, screenY, console);

            Things[,] world = DefaultWorld();


            int worldOffsetX = 2;
            int worldOffsetY = 3;

            Player player = new Player(30, 18);

            bool running = true;
            timer.Start();

            gameTime.ApplyOffset(timeOffset, 0, 0, 0);

            while (!console.KeyPressed && console.WindowUpdate() && running)
            {
                gameTime.Tick(timer.Elapsed);

                // Get Input
                if (console.KeyPressed)
                {
                    Key key = console.GetKey();

                    if (key == Key.Escape)
                        running = false;

                    if (key == Key.L)
                    {
                        ui.UpdateInfo(player.LookAt(world));
                    }

                    if (key == Key.T)
                    {
                        if (player.GetTarget(world) != null && typeof(Tillable).IsInstanceOfType(player.GetTarget(world)))
                        {
                            ui.UpdateInfo("Tillable");
                        }
                        else
                        {
                            ui.UpdateInfo("Not Tillable");
                        }
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

                worldOffsetX = player.GetLocation()[0] * -1 + (screenY / 2 - 5);
                worldOffsetY = player.GetLocation()[1] * -1 + (screenX / 2);

                //  Render Stuff
                // Clear
                for (int i = 0; i < screenX; i++)
                {
                    for (int j = 0; j < screenY; j++)
                    {
                        console.Write(j, i, ' ', Color4.Magenta, Color4.Black);
                    }
                }

                //      World
                foreach (Things t in world)
                {
                    t.Render(console, worldOffsetX, worldOffsetY);
                }
                //      Player
                player.Render(console, worldOffsetX, worldOffsetY);

                // hacky 'walk under' layer (Silo)
                console.Write(2 + worldOffsetX, 3 + worldOffsetY, "===", Color4.LightSteelBlue);
                console.Write(3 + worldOffsetX, 3 + worldOffsetY, "\\ /", Color4.LightSteelBlue);
                console.Write(4 + worldOffsetX, 3 + worldOffsetY, "|^|", Color4.LightSteelBlue);

                // UI
                ui.Render(player);

                console.Write(screenY - 9, 61, "D " + gameTime.GetDebugTickTimeCounter(), Color4.Yellow);
                console.Write(screenY - 8, 61, "T " + gameTime.GetDebugTotalTimeCounter(), Color4.Yellow);  
                console.Write(screenY - 7, 61, "R " + gameTime.GetDebugRunningTimeCounter(), Color4.Yellow);

                console.Write(screenY - 5, 63, gameTime.GetDayOfTheWeek(), Color4.Yellow);
                console.Write(screenY - 3, 63, gameTime.GetClock(), Color4.Yellow);
                console.Write(screenY - 1, 63, gameTime.GetDate(), Color4.Yellow);
            }
        }
    }
}
