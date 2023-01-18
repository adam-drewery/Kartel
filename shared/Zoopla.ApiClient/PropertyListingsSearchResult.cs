using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zoopla.ApiClient;

public class PropertyListingsSearchResult : SearchResult
{
    [JsonProperty("result_count")]
    public int ResultCount { get; set; }

    [JsonProperty("listing")]
    public IReadOnlyList<PropertyListing> Listings { get; set; }
}