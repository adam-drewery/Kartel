using System;
using static System.Console;

namespace Kartel.Console.Controls
{
    class ActionControl : Control
    {
        private int? _x;
        private int? _y;
        private int _height;
        private int _width;
        private string _text;
        private string _description;
        private readonly Border _border = new ThickBorder();
        
        public Func<string> Text { get; set; }

        public Func<string> Description { get; set; }

        public Action Action { get; set; }
        
        public bool Selected { get; private set; }

        public override int? X => _x;

        public override int? Y => _y;
        
        public void Select() => Selected = true;
        
        public void Deselect() => Selected = false;

        public ActionControl(Func<string> text, Func<string> description, Action action, char key) : this(text, description, action)
        {
            ShortcutKey = key;
        }

        public ActionControl(Func<string> text, Func<string> description, Func<Form> form) : this(text, description, () => form().Render()) { }

        public ActionControl(Func<string> text, Func<string> description, Func<Form> form, char key) : this(text, description, () => form().Render(), key) { }

        public ActionControl(Func<string> text, Func<string> description, Action action)
        {
            Text = text;
            Description = description;
            Action = action;
        }

        public override void Update()
        {
            if (_text == Text() && _description == Description())
                return;
            
            Render(Width);
        }

        public override int Height => _height;

        public override int Width => _width;

        public override void Render(int width)
        {
            _text = Text();
            _description = Description();
            
            if (!_x.HasValue || !_y.HasValue)
            {
                _x = CursorLeft;
                _y = CursorTop;
            }
            else SetCursorPosition(_x.Value, _y.Value);
            
            _width = width;
            
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

            var lines = new[]
            {
                _border.TopLeft + new string(_border.Top, _text.Length).PadRight(width - 2, _border.Top) + _border.TopRight,
                _border.Left + _text.PadRight(width - 2) + _border.Right,
                _border.Left + new string('-', width - 2) + _border.Right,
                _border.Left + new string(' ', width - 2) + _border.Right,
                _border.Left + _description.PadRight(width - 2) + _border.Right,
                _border.BottomLeft + new string(_border.Bottom, _text.Length).PadRight(width - 2, _border.Bottom) + _border.BottomRight,
            };

            foreach(var line in lines)
                Out.WriteLine(line);
            
            _height = lines.Length;
        }
    }
}