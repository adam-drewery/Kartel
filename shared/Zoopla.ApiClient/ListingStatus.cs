using Newtonsoft.Json;

namespace Zoopla.ApiClient;

public enum ListingStatus
{
    [Parameter("sale"), JsonProperty("sale")] Sale,
    [Parameter("rent"), JsonProperty("rent")] Rent
}