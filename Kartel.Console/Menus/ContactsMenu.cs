using System.Linq;
using Kartel.Console.Controls;

namespace Kartel.Console.Menus
{
    internal class ContactsMenu : KartelMenuForm
    {
        //public override string Title => "Contacts";

        public override string Text => "Manage your contacts";

        public ContactsMenu(MenuForm parent) : base(parent)
        {
            Actions.AddRange(Program.Player.Relationships.Select(m =>
            {
                string Status() => m.Key.CurrentActivity == null
                    ? "Hanging out at " + m.Key.Location
                    : m.Key.CurrentActivity.Name.PresentTense 
                      + " at " + m.Key.CurrentActivity.Location;

                return new ActionControl(() => m.Key.Name, Status, () => { });
            }));
        }
    }
}