using System.Threading;
using Kartel.Service.Client;
using Kartel.Console.Menus;

namespace Kartel.Console
{
    class Program
    {
        public static Game Game { get; } = new Game(new PropertyMarketClient())
        {
            Clock = { MinimumTickSpeed = 1000 }
        };

        public static readonly SemaphoreSlim UpdateSemaphore = new SemaphoreSlim(1, 1);
        
        public static Player Player { get; private set; }

        public static StatusBar StatusBar { get; } = new StatusBar();

        static void Main(string[] args)
        {
            Player = Player.New(Game);
            Game.Characters.Add(Player);

            Game.Characters.Add(Player);
            Game.Clock.MinimumTickSpeed = 50;
            Game.Clock.SpeedFactor = 100;
            Game.Clock.Tick += (sender, eventArgs) => StatusBar.Render(0);

            Game.Clock.Start();
            new MainMenu(null).Render();
        }
    }
}