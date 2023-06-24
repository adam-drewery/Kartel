using PuppeteerSharp;

namespace Zoopla.Scraping;

public abstract class ZooplaScraper
{
    
    protected static IBrowser Browser;
    protected IPage Page;

    public ZooplaScraper()
    {
        Task.Run(async () =>
        {
            Page = await Browser.NewPageAsync();
            await Page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.82 Safari/537.36");
        }).Wait();
}
    
    public static async Task Initialize()
    {
        var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        Browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
    }
    
    public static async Task Main(string[] args)
    {
        // Setup Puppeteer to use the installed version of Chrome
        await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        await using var page = await browser.NewPageAsync();

        // Navigate to the Zoopla website
        await page.GoToAsync("https://www.zoopla.co.uk/for-sale/property/west-yorkshire/leeds/?q=leeds&search_source=home");

        // Use a CSS selector to find the elements containing the property prices
        var propertyPrices = await page.QuerySelectorAllAsync(".css-1e28vvi-PriceContainer");

        // Loop through each property price and print it out
        foreach (var price in propertyPrices)
        {
            var priceText = await page.EvaluateFunctionAsync<string>("element => element.textContent", price);
            Console.WriteLine(priceText);
        }
    }
}

    // private class LoggingHttpMessageHandler : HttpClientHandler
    // {
    //     protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    //     {
    //
    //             var result = await base.SendAsync(request, cancellationToken);
    //             
    //             if (result.IsSuccessStatusCode) return result;
    //
    //             var lines = new[]
    //             {
    //                 "Request failed to: " + request.RequestUri,
    //                 "Status Code: " + result.StatusCode,
    //                 "Response Body:" + Environment.NewLine 
    //                     + await result.Content.ReadAsStringAsync(cancellationToken)
    //             };
    //
    //             throw new HttpRequestException(string.Join(Environment.NewLine, lines));
    //     }
    // }