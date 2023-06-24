using HtmlAgilityPack;

namespace Zoopla.Scraping;

public class RegionScraper : ZooplaScraper
{
    public async Task<IEnumerable<string>> Scrape()
    {
        var response = await Page.GoToAsync("https://www.zoopla.co.uk/sitemap/");
        var text = await response.TextAsync();
        var document = new HtmlDocument();
        document.LoadHtml(text);
        
        return document.DocumentNode.SelectNodes("/html/body/div[2]/div/div/div/ul/li/a")
            .Select(x => x.Attributes["href"].Value
                .Replace("sitemap", string.Empty)
                .Replace("/", string.Empty));
    }
}