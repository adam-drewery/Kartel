using System;
using System.Linq;
using Kartel.Activities;
using Kartel.Console.Controls;

namespace Kartel.Console.Menus
{
    internal class PersonalMenu : KartelMenuForm
    {
        public override string Text { get; } = "Manage your personal details";

        public PersonalMenu(Form parent) : base(parent)
        {
            Detail(() => "Name", () =>Program.Player.Name);
            Detail(() => "Home Address", () => Program.Player.Home?.Address);
            Detail(() => "Current Location", () => Program.Player.Location);
            Detail(() => "Money", () => Program.Player.Money);

            string ActivityName() => Program.Player.CurrentActivity == null
                ? "Hanging out"
                : Program.Player.CurrentActivity.Started
                    ? Program.Player.CurrentActivity.Name.PresentTense
                    : "Moving to " + Program.Player.CurrentActivity.Name.FutureTense;

            Action(() => "Current Activity", ActivityName, () => new ActivityMenu(this));
        }
    }
    
    internal class ActivityMenu : KartelMenuForm
    {
        public ActivityMenu(Form parent) : base(parent)
        {
            Action(() =>"Attack", () => String.Empty, () => new AttackMenu(this));
        }

        public override string Text { get; } = "Activities";
    }

    internal class AttackMenu : KartelMenuForm
    {
        public override string Text { get; } = "Attack";

        public AttackMenu(Form parent) : base(parent)
        {
            Actions.AddRange(Program.Player.Relationships.Select(m =>
            {
                string Status() => m.Key.CurrentActivity == null
                    ? "Hanging out at " + m.Key.Location
                    : m.Key.CurrentActivity.Name.PresentTense + " at " + m.Key.CurrentActivity.Location;

                return new ActionControl(() => m.Key.Name, Status, () =>
                {
                    Program.Player.Activities.Enqueue(new Attack(Program.Player, m.Key));
                    Parent.Render();
                });
            }));
        }
    }
}
