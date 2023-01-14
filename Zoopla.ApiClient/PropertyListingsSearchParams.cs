using System.Collections.Generic;

namespace Zoopla.ApiClient;

public class PropertyListingsSearchParams : SearchParams
{
    [Parameter("radius")]
    public decimal Radius { get; set; } = 0.1m;

    [Parameter("order_by")]
    public OrderBy? OrderBy { get; set; }

    [Parameter("ordering")]
    public Ordering? Ordering { get; set; }

    [Parameter("listing_status")]
    public ListingStatus? ListingStatus { get; set; }

    [Parameter("include_sold")]
    public bool? IncludeSold { get; set; }

    [Parameter("include_rented")]
    public bool? IncludeRented { get; set; }

    [Parameter("minimum_price")]
    public decimal? MinumumPrice { get; set; }

    [Parameter("maximum_price")]
    public decimal? MaximumPrice { get; set; }

    [Parameter("minimum_beds")]
    public int? MinimumBeds { get; set; }

    [Parameter("maximum_beds")]
    public int? MaximumBeds { get; set; }

    [Parameter("furnished")]
    public Furnishing? Furnished { get; set; }

    [Parameter("property_type")]
    public PropertyType? PropertyType { get; set; }

    [Parameter("new_homes")]
    public bool? NewHomes { get; set; }

    [Parameter("chain_free")]
    public bool? ChainFree { get; set; }

    [Parameter("keywords")]
    public List<string> Keywords { get; } = new();

    [Parameter("page_number")]
    public int? PageNumber { get; set; }

    [Parameter("radius")]
    public int? PageSize { get; set; }

    [Parameter("summarised")]
    public bool? Summarised { get; set; }
}