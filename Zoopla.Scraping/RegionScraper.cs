namespace Zoopla.Scraping;

public class RegionScraper : ZooplaScraper
{
    public async Task<IEnumerable<string>> Scrape()
    {
        var document = await GetDocument("https://www.zoopla.co.uk/sitemap/");
        
        return document.DocumentNode.SelectNodes("/html/body/div[2]/div/div/div/ul/li/a")
            .Select(x => x.Attributes["href"].Value
                .Replace("sitemap", string.Empty)
                .Replace("/", string.Empty));
        
    }
}