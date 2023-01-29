using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Zoopla.Scraping;

public class ListingScraper : ZooplaScraper
{
    public event EventHandler<ParsingFailedEventArgs> ParsingFailed;

    public ICollection<string> IgnoreIds { get; set; } = new HashSet<string>();

    public async Task<IEnumerable<PropertyListing>> Scrape(ListingRequest @params)
    {
        await using var stream = await Http.GetStreamAsync(@params.ToString());
        var document = new HtmlDocument();
        document.Load(stream);

        const string xPath = "/html/body/div[2]/div/div/main/div/div[4]/div[2]/section/div[2]/div";
        var listingNodes = document.DocumentNode.SelectNodes(xPath)?.ToList();

        if (listingNodes == null)
        {
            throw new InvalidDataException($"No listing nodes found for region \"{@params.County}\" and city \"{@params.City}\"");
        }

        var tasks = new List<Task<PropertyListing>>();

        foreach (var listingNode in listingNodes)
        {
            try
            {
                var id = listingNode.Id.Replace("listing_", string.Empty);
                if (IgnoreIds.Contains(id))
                {
                    Console.WriteLine($"Skipping listing {id}");
                    continue;
                }
                    
                var task = GetListing(listingNode, @params.ListingType);
                    
                tasks.Add(task);
            }
            catch (Exception e)
            {
                OnParsingFailed(listingNode, e);
            }
        }

        try
        {
            await Task.WhenAll(tasks);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return tasks.Where(t => !t.IsFaulted).Select(t => t.Result);
    }

    private async Task<PropertyListing> GetListing(HtmlNode listingNode, ListingType listingType)
    {
        var detailsNode = listingNode.SelectSingleNode("div/div/div/div[2]/div/a/div");

        int ParsePrice(string input, string propertyId)
        {
            var digits = input.Where(char.IsDigit).ToArray();

            if (int.TryParse(new string(digits), out var result)) return result;
            
            throw new FormatException($"Failed to parse price \"{input}\" for property {propertyId}");
        }

        var listing = new PropertyListing
        {
            Id = listingNode.Id.Replace("listing_", string.Empty),
            Price = ParsePrice(detailsNode.SelectNodes("div/div").Last()?.InnerText, listingNode.Id),
            Address = { Value = detailsNode.SelectNodes("div").Last()?.SelectSingleNode("h3").InnerText }
        };

        var roomsNode = detailsNode.SelectNodes("div/ul/li");

        if (roomsNode == null)
            throw new InvalidDataException($"Failed to find room count details for property {listing.Id}");

        foreach (var node in roomsNode)
        {
            var roomName = node.SelectSingleNode("span[1]").InnerText;
            var roomCount = node.SelectSingleNode("span[2]").InnerText;

            if (roomName == "Bathrooms")
                listing.Bathrooms = short.Parse(roomCount);

            if (roomName == "Bedrooms")
                listing.Bedrooms = short.Parse(roomCount);

            if (roomName == "Living rooms")
                listing.LivingRooms = short.Parse(roomCount);
        }
        
        Console.WriteLine($"Getting details page for listing {listing.Id}");
        var detailsPage = await Http.GetStringAsync($"https://www.zoopla.co.uk/{listingType.Key}/details/{listing.Id}");
        const string regex = @"{""__typename"":""LocationCoordinates"",""latitude"":(.*),""longitude"":(.*),""isApproximate"":(.*)}";
        var match = Regex.Match(detailsPage, regex);

        if (match.Groups.Count > 1)
        {
            listing.Latitude = double.Parse(match.Groups[1].Value);
            listing.Longitude = double.Parse(match.Groups[2].Value);
        }

        Console.WriteLine($"Listing {listing.Id} parsed");
        return listing;
    }

    protected virtual void OnParsingFailed(HtmlNode node, Exception e)
    {
        ParsingFailed?.Invoke(this, new ParsingFailedEventArgs(e, node));
    }

    public class ParsingFailedEventArgs
    {
        public ParsingFailedEventArgs(Exception exception, HtmlNode node)
        {
            Exception = exception;
            Node = node;
        }

        public Exception Exception { get; set; }

        public HtmlNode Node { get; set; }
    }
}