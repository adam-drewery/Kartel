using Newtonsoft.Json;

namespace Zoopla.ApiClient;

public enum ListingType
{
    [JsonProperty("Terraced")] Terraced,
    [JsonProperty("End of terrace")] EndOfTerrace,
    [JsonProperty("semi-detached")] SemiDetached,
    [JsonProperty("Detached house")] Detached,
    [JsonProperty("Mews house")] MewsHouse,
    [JsonProperty("Flat")] Flat,
    [JsonProperty("Maisonette")] Maisonette,
    [JsonProperty("Bungalow")] Bungalow,
    [JsonProperty("Town house")] Townhouse,
    [JsonProperty("Cottage")] Cottage,
    [JsonProperty("Farm/Barn")] Farm,
    [JsonProperty("Mobile/Static")] Mobile,
    [JsonProperty("Land")] Land,
    [JsonProperty("Studio")] Studio,
    [JsonProperty("Block of flats")] BlockOfFlats,
    [JsonProperty("Country house")] CountryHouse,
    [JsonProperty("Office")] Office
}