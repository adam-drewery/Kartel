using System;

namespace Zoopla.ApiClient;

public class PriceChange
{
    [Parameter("price")]
    public decimal Price { get; set; }

    [Parameter("date")]
    public DateTime Date { get; set; }
}