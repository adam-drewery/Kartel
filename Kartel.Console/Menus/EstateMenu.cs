using System.Linq;
using Kartel.Console.Controls;

namespace Kartel.Console.Menus
{
    internal class EstateMenu : KartelMenuForm
    {
        //public override string Title => "Properties";

        public override string Text => "View and manage properties.";

        public EstateMenu(MenuForm parent) : base(parent)
        {
            Actions.AddRange(Program.Player.Estate.Select(p => new ActionControl(() => p.Address.ToString(), () => "", () => new PropertyDetails(this, p))));
            Shortcuts.Add(new ActionControl(() => "View properties for sale", () => null, () => new BuyPropertiesMenu(this), 'b'));
        }
    }
}