using System.Linq;
using Kartel.Console.Controls;

namespace Kartel.Console
{
    internal static class Window
    {
        static Window()
        {
            Program.Game.Clock.Tick += (sender, args) =>
            {
                Program.UpdateSemaphore.Wait();

                foreach (var control in Form.Actions.Concat<Control>(Form.Details))
                    control.Update();

                Program.UpdateSemaphore.Release();
            };
        }

        public static MenuForm Form { get; set; }
    }
}