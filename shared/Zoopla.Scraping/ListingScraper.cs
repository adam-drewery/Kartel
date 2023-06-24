using Audacia.Random.Extensions;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PuppeteerSharp;
using static System.Environment;

namespace Zoopla.Scraping;  

public class ListingScraper : ZooplaScraper
{
    public ICollection<string> IgnoreIds { get; set; } = new HashSet<string>();

    public async IAsyncEnumerable<PropertyListing> Scrape(ListingRequest @params)
    {
        var response = await Page.GoToAsync(@params.ToString(), WaitUntilNavigation.Networkidle0);
        var text = await response.TextAsync();
        var document = new HtmlDocument();
        document.LoadHtml(text);

        var json = await Page.EvaluateFunctionAsync<string>(@"
            () => {
                const scriptElement = document.querySelector('#__NEXT_DATA__');
                return scriptElement ? scriptElement.innerText : null;
            }");

        if (json == null)
            throw new InvalidDataException($"No data found in response to {@params}." + NewLine + text);
        
        var data = JsonConvert.DeserializeObject<JObject>(json);
        var listings = data.SelectToken("props")
            !.SelectToken("pageProps")
            !.SelectToken("regularListingsFormatted");

        foreach (var listing in listings!.Children())
        {
            var result = new PropertyListing
            {
                Id = listing.Value<string>("listingId"),
                Address = { Value = listing.Value<string>("address") },
                Price = Convert.ToInt32(listing.Value<string>("price").Replace("£", string.Empty)
                    .Replace(",", string.Empty)),
                Latitude = listing
                    !.SelectToken("location")
                    !.SelectToken("coordinates")
                    !.SelectToken("latitude")
                    !.Value<double>(),
                Longitude = listing
                    !.SelectToken("location")
                    !.SelectToken(".coordinates")
                    !.SelectToken("longitude")
                    !.Value<double>()
            };

            foreach (var room in listing!.SelectToken("features")!)
            {
                var content = room.Value<short>("content");
                switch (room.Value<string>("iconId"))
                {
                    case "bath":
                        result.Bathrooms = content;
                        break;
                    case "bed": 
                        result.Bedrooms = content;
                        break;
                    case "chair": 
                        result.LivingRooms = content;
                        break;
                }
            }

            await Page.SetUserAgentAsync(UserAgents.Random());
            
            yield return result;
        }
    }
}