using Newtonsoft.Json;

namespace Zoopla.ApiClient;

public enum PriceModifier
{
    [JsonProperty("offers_over")] OffersOver,
    [JsonProperty("poa")] Poa,
    [JsonProperty("fixed_price")] FixedPrice,
    [JsonProperty("from")] From,
    [JsonProperty("offers_in_region_of")] OffersInRegionOf,
    [JsonProperty("part_buy_part_rent")] PartBuyPartRent,
    [JsonProperty("price_on_request")] PriceOnRequest,
    [JsonProperty("shared_equity")] SharedEquity,
    [JsonProperty("shared_ownership")] SharedOwnership,
    [JsonProperty("guide_price")] GuidePrice,
    [JsonProperty("sale_by_tender")] SaleByTender
}