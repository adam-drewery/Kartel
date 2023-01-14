using System.Web;

namespace Zoopla.Scraping;

public class ListingRequest
{
    public ListingType ListingType { get; }

    public string County { get; set; }
    
    public string City { get; set; }

    public int PriceMax { get; set; }

    public int PriceMin { get; set; }

    public short Radius { get; set; }
    
    public short PageNumber { get; set; }

    public ICollection<PropertyType> PropertyTypes { get; } = new HashSet<PropertyType>();

    public ListingRequest(ListingType listingType)
    {
        ListingType = listingType;
    }

    public override string ToString()
    {
        
        var url = City == null
            ? new Uri($"https://www.zoopla.co.uk/{ListingType.Key}/{SubSection}/{County}/{City}?")
            : new Uri($"https://www.zoopla.co.uk/{ListingType.Key}/{SubSection}/{County}?");

        var query = HttpUtility.ParseQueryString(string.Empty);
        
        query.Add("search_source", "refine");
        
        if (PriceMax != default) 
            query.Add("price_max", PriceMax.ToString());
        
        if (PriceMin != default) 
            query.Add("price_min", PriceMin.ToString());
        
        foreach (var propertyType in PropertyTypes) 
            query.Add("property_sub_type", propertyType.Key);
        
        if (Radius != default) 
            query.Add("radius", Radius.ToString());
        
        if (PageNumber > 1)
            query.Add("pn", PageNumber.ToString());

        query.Add("view_type", "list");
        
        return url + query.ToString();
    }

    private string SubSection
    {
        get
        {
            if (PropertyTypes.Count == 1)
            {
                var propertyType = PropertyTypes.Single();

                if (propertyType == PropertyType.Bungalow)
                    return "bungalows";
                
                if (propertyType == PropertyType.Flats)
                    return "flats";
            }

            if (PropertyTypes.All(x => x.IsHouse))
                return "houses";
            
            return "property";
        }
    }
}