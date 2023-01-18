using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Zoopla.ApiClient;

public class PropertyListing
{
	[JsonProperty("listing_id")] public string Id { get; set; }

	[JsonProperty("outcode")] public string Outcode { get; set; }

	[JsonProperty("post_town")] public string PostTown { get; set; }

	[JsonProperty("displayable_address")] public string DisplayableAddress { get; set; }

	[JsonProperty("county")] public string County { get; set; }

	[JsonProperty("country")] public string Country { get; set; }

	[JsonProperty("num_bathrooms")] public short? Bathrooms { get; set; }

	[JsonProperty("num_bedrooms")] public short? Bedrooms { get; set; }

	[JsonProperty("num_floors")] public short? Floors { get; set; }

	[JsonProperty("num_recepts")] public short Receptions { get; set; }

	//[JsonProperty("listing_status")]
	//public ListingStatus ListingStatus { get; set; }

	//[JsonProperty("status")]
	//public Status Status { get; set; }

	[JsonProperty("price_change")] public IReadOnlyList<PriceChange> PriceChanges { get; set; }

	//[JsonProperty("property_type")]
	//public ListingType? PropertyType { get; set; }

	[JsonProperty("street_name")] public string StreetName { get; set; }

	[JsonProperty("thumbnail_url")] public Uri ThumbnailUrl { get; set; }

	[JsonProperty("image_url")] public Uri ImageUrl { get; set; }

	[JsonProperty("image_caption")] public string ImageCaption { get; set; }

	[JsonProperty("description")] public string Description { get; set; }

	[JsonProperty("short_description")] public string ShortDescription { get; set; }

	[JsonProperty("details_url")] public Uri DetailsUrl { get; set; }

	[JsonProperty("new_home")] public bool NewHome { get; set; }

	[JsonProperty("shared_occupancy")] public bool SharedOccupancy { get; set; }

	[JsonProperty("first_published_date")] public DateTime FirstPublishedDate { get; set; }

	[JsonProperty("last_published_date")] public DateTime LastPublishedDate { get; set; }

	[JsonProperty("latitude")] public double Latitude { get; set; }

	[JsonProperty("longitude")] public double Longitude { get; set; }

	public override string ToString()
	{
		var parts = new[] {StreetName, PostTown, County, Country}.Where(s => !string.IsNullOrWhiteSpace(s));
		return string.Join(", ", parts);
	}
}