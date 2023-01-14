using Kartel.Console.Controls;

namespace Kartel.Console.Menus
{
    internal partial class MainMenu : KartelMenuForm
    {
        public MainMenu(MenuForm parent) : base(parent)
        {
            Action(() => "Personal", () => "View your personal details", () => new PersonalMenu(this), 'p');
            Action(() => "Contacts", () => "View and Manage your contacts", () => new ContactsMenu(this), 'c');
            Action(() => "Properties", () => "View and Manage properties you own", () => new EstateMenu(this), 'b');
        }

        //public override string Title => "Kartel";

        public override string Text => "Welcome to Kartel";
    }
}
    