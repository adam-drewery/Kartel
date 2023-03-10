using HtmlAgilityPack;

namespace Zoopla.Scraping;

public abstract class ZooplaScraper
{
    protected readonly HttpClient Http = new(new LoggingHttpMessageHandler())
    {
        DefaultRequestHeaders =
        {
            { "Accept-Language", "en,en-US;q=0.5" },
            { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8" },
            { "user-agent", "Mozilla/5.0 (X11; Linux x86_64; rv:109.0) Gecko/20100101 Firefox/110.0" }
        }
    };

    private class LoggingHttpMessageHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

                var result = await base.SendAsync(request, cancellationToken);
                
                if (result.IsSuccessStatusCode) return result;

                var lines = new[]
                {
                    "Request failed to: " + request.RequestUri,
                    "Status Code: " + result.StatusCode,
                    "Response Body:" + Environment.NewLine 
                        + await result.Content.ReadAsStringAsync(cancellationToken)
                };

                throw new HttpRequestException(string.Join(Environment.NewLine, lines));
        }
    }
    
    protected async Task<HtmlDocument> GetDocument(string url)
    {
        await using var stream = await Http.GetStreamAsync(url);
        var document = new HtmlDocument();
        document.Load(stream);
        return document;
    }
}