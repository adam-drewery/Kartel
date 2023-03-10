using FluentAssertions;
using Xunit;

namespace Zoopla.Scraping.Tests;

public class ScraperTests
{
    [Fact]
    public async Task Retrieve_listings()
    {
        var request = new ListingRequest(ListingType.Buy)
        {
            County = "west-yorkshire",
            Radius = 3,
            PropertyTypes = { PropertyType.Flats },
            PriceMax = 80000,
            PriceMin = 40000
        };

        var results = new ListingScraper().Scrape(request);

        foreach (var result in await results)
        {
            result.Address.Lines.Should().NotBeEmpty();
            result.Price.Should().BeGreaterThan(0);
        }
    }
}