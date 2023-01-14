using System.Collections.Generic;
using Kartel.Console.Controls;

namespace Kartel.Console.Menus
{
    internal abstract class KartelMenuForm : MenuForm
    {
        public override IEnumerable<string> Title { get; } = new List<string>
        {
            @" ____  __.     _____________________    .____ ",
            @"|    |/ _|____ \______   \__    ___/___ |    |",
            @"|      < \__  \ |       _/ |    |_/ __ \|    |",
            @"|    |  \ / __ \|    |   \ |    |\  ___/|    |___",
            @"|____|__ (____  /____|_  / |____| \___  >_______ \",
            @"        \/    \/       \/             \/        \/",
            @"---------------------------------------------------"
        };

        protected KartelMenuForm(Form parent) : base(parent) { }
    }
}