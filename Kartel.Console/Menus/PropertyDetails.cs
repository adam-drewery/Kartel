using Kartel.Console.Controls;
using Kartel.Environment;

namespace Kartel.Console.Menus
{
    internal class PropertyDetails : KartelMenuForm
    {
        private readonly Building _building;

        public PropertyDetails(Form parent, Building building) : base(parent) => _building = building;

        public override string Text => _building.ToString();
    }
}