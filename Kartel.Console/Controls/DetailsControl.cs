using System;
using static System.Console;

namespace Kartel.Console.Controls
{
    class DetailsControl : Control
    {
        private string _label;
        private string _value;
        
        private int _width;
        private int _height;
        private int? _x;
        private int? _y;

        public DetailsControl(Func<string> label, Func<object> value)
        {
            Label = label;
            Value = value;
        }

        public Func<string> Label { get; set; }

        public Func<object> Value { get; set; }
        
        public bool Selected { get; private set; }

        public void Select()
        {
            Selected = true;
            Render(_width);
        }

        public void Deselect()
        {
            Selected = false;
            Render(_width);
        }

        public override void Update()
        {
            if (_label == Label() && _value == Value().ToString())
                return;
            
            Render(Width);
        }

        public override int Height => _height;

        public override int Width => _width;

        public override int? X => _x;

        public override int? Y => _y;

        public override void Render(int width)
        {
            _label = Label();
            _value = Value().ToString();
            
            if (!_x.HasValue || !_y.HasValue)
            {
                _x = CursorLeft;
                _y = CursorTop;
            }
            else SetCursorPosition(_x.Value, _y.Value);
            
            _width = width;
            _height = 1;
            if (Selected)
            {
                ForegroundColor = ConsoleColor.Black;
                BackgroundColor = ConsoleColor.White;
            }
            else
            {
                ForegroundColor = ConsoleColor.White;
                BackgroundColor = ConsoleColor.Black;
            }

            var padding = width / 2 - Label().Length;
            WriteLine($" {new string(' ', padding)}[{_label}] {_value}");
        }
    }
}