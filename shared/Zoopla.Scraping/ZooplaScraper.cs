using HtmlAgilityPack;

namespace Zoopla.Scraping;

public abstract class ZooplaScraper
{
    private const string UserAgent = "Mozilla/5.0 (X11; Linux x86_64) " +
                                     "AppleWebKit/537.36 (KHTML, like Gecko) " +
                                     "Chrome/108.0.0.0 Safari/537.36";
    
    protected readonly HttpClient Http = new(new LoggingHttpMessageHandler()) { DefaultRequestHeaders = { { "user-agent", UserAgent } } };

    private class LoggingHttpMessageHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{request.Method}: {request.RequestUri}");

            try
            {
                var result = await base.SendAsync(request, cancellationToken);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{request.Method}: {request.RequestUri} failed");
                Console.WriteLine(e);
                throw;
            }
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