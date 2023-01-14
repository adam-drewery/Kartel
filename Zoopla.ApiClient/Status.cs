using Newtonsoft.Json;

namespace Zoopla.ApiClient;

public enum Status
{
    [JsonProperty("for_sale")] ForSale,
    [JsonProperty("sale_under_offer")] SaleUnderOffer,
    [JsonProperty("sold")] Sold,
    [JsonProperty("to_rent")] ToRent,
    [JsonProperty("rent_under_offer")] RentUnderOffer,
    [JsonProperty("rented")] Rented
}