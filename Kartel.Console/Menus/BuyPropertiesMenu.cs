using System.Threading.Tasks;
using Kartel.Console.Controls;

namespace Kartel.Console.Menus
{
    internal class BuyPropertiesMenu : KartelMenuForm
    {
        public BuyPropertiesMenu(Form parent) : base(parent)
        {
            var random = new System.Random();
            Task.Run(() =>
            {
                //TODO: Re-implement this.
                ////foreach (var _ in Enumerable.Range(0, 10))
                ////{
                ////    var property = await random.Property();
                ////    Add(property.Price.ToString("C"), property.Address.Lines);
                ////}
            }).Wait();
        }

        public override string Text => "Browse the market for new properties";
    }
}