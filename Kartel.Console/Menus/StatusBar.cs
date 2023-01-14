using System;
using Kartel.Console.Controls;
using static System.Console;

namespace Kartel.Console.Menus
{
    public class StatusBar : Control
    {
        private int? _x;
        private int? _y;

        public override void Update()
        {
            Render(Width);
        }

        public override int Height { get; } = 1;
        
        public override int Width => WindowWidth;

        public override int? X => _x;

        public override int? Y => _y;
        
        public override void Render(int width)
        {
            if (!_x.HasValue || !_y.HasValue)
            {
                _x = CursorLeft;
                _y = CursorTop;
            }
            else SetCursorPosition(_x.Value, _y.Value);
            
            ForegroundColor = ConsoleColor.Black;
            BackgroundColor = ConsoleColor.White;
                    
            SetCursorPosition(0, WindowHeight - 2);
            WriteLine("   " + Program.Game.Clock.Time + $" (x{Program.Game.Clock.SpeedFactor})");
                    
            SetCursorPosition(WindowWidth - 14, WindowHeight - 2);
            WriteLine($"{(1000/Program.Game.Clock.TickSpeed):F}tps".PadRight(14));
                    
            ForegroundColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Black;
        }
    }
}