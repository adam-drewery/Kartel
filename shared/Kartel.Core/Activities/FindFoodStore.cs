using System;
using System.Threading.Tasks;
using Kartel.Attributes;
using Kartel.Entities;
using Kartel.Environment;

namespace Kartel.Activities;

[Verb("Find something to eat", "Finding something to eat", "Found something to eat")]
public class FindFoodStore : Activity
{
    private Task<Shop> _findShopTask;

    public Shop Result { get; private set; }

    public FindFoodStore(Person actor) : base(actor) { }

    protected override void Update(TimeSpan sinceLastUpdate)
    {
        if (_findShopTask == null)
        {
            _findShopTask = Game.Services.Locale
                .FindStoreAsync((Actor.Location, StockType.Food));
        }
        else
        {
            if (_findShopTask.IsCompleted)
            {
                Result = _findShopTask.Result;
                Complete();
            }
        }
    }
}