using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace Kartel.Console.Controls
{
    abstract class MenuForm : Form
    {
        private MenuForm()
        {
            Actions = new List<ActionControl>();
            Details = new List<DetailsControl>();
            Shortcuts = new List<ActionControl>();
        }

        public Form Parent { get; }

        public abstract IEnumerable<string> Title { get; }

        public abstract string Text { get; }

        public int SelectedIndex
        {
            get
            {
                var selected = Actions.SingleOrDefault(i => i.Selected);
                return selected == null ? -1 : Actions.IndexOf(selected);
            }
            set
            {
                Actions.SingleOrDefault(i => i.Selected)?.Deselect();
                Actions[value].Select();
            }
        }

        public List<DetailsControl> Details { get; }
        
        public List<ActionControl> Actions { get; }

        public List<ActionControl> Shortcuts { get; }

        public void Detail(Func<string> text, Func<object> value) =>
            Details.Add(new DetailsControl(text, value));

        public void Action(Func<string> text, Func<string> description, Func<Form> form) =>
            Actions.Add(new ActionControl(text, description, form));

        public void Action(Func<string> text, Func<string> description, Func<Form> form, char key) =>
            Actions.Add(new ActionControl(text, description, form, key));

        public override void Render()
        {
            Program.UpdateSemaphore.Wait();
            
            CursorVisible = false;
            ForegroundColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Black;

            Clear();

            SetCursorPosition(0, 0);

            foreach (var line in Title) WriteLine(" " + line.ToUpper());

            WriteLine();
            WriteLine(" " + Text);
            WriteLine(" " + new string('-', WindowWidth / 2));
            WriteLine();

            foreach (var detail in Details)
                detail.Render(WindowWidth / 2);

            WriteLine();
            
            foreach (var action in Actions)
                action.Render(WindowWidth / 2);

            WriteLine();
            WriteLine();

            SetCursorPosition(2, WindowHeight - 1);

            foreach (var shortcut in Shortcuts)
            {
                if (Actions.IndexOf(shortcut) == SelectedIndex)
                {
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Black;
                }

                Write($"[{shortcut.ShortcutKey}] {shortcut.Text}   ");
            }

            Window.Form = this;
            Program.UpdateSemaphore.Release();
            var key = ReadKey();

            HandleInput(key);
        }

        private void HandleInput(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.Spacebar:
                    if (Program.Game.Clock.Enabled) Program.Game.Clock.Stop();
                    else Program.Game.Clock.Start();
                    break;
                case ConsoleKey.Add:
                    Program.Game.Clock.SpeedFactor = Program.Game.Clock.SpeedFactor * 2;
                    break;
                case ConsoleKey.Subtract:
                    Program.Game.Clock.SpeedFactor = Program.Game.Clock.SpeedFactor / 2;
                    break;
                case ConsoleKey.UpArrow:
                    if (SelectedIndex > 0)
                        SelectedIndex--;
                    break;
                case ConsoleKey.DownArrow:
                    if (SelectedIndex < Actions.Count - 1)
                        SelectedIndex++;
                    break;
                case ConsoleKey.Enter:
                    Actions[SelectedIndex].Action();
                    break;
                case ConsoleKey.Backspace:
                case ConsoleKey.Escape:
                    Clear();
                    ResetColor();
                    return;
                default:
                    var shortcut = Shortcuts.SingleOrDefault(s => s.ShortcutKey == key.KeyChar);
                    shortcut?.Action();
                    break;
            }

            Render();
        }

        public MenuForm(Form parent) : this()
        {
            Parent = parent;
        }
    }
}