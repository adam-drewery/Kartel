using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Zoopla.ApiClient;

public class ZooplaClient : IDisposable
{
    private readonly HttpClient _http = new() {BaseAddress = new Uri("https://api.zoopla.co.uk/api/v1/")};

    public ZooplaClient(string apiKey) => ApiKey = apiKey;

    public string ApiKey { get; }

    public async Task<PropertyListingsSearchResult> PropertyListings(PropertyListingsSearchParams searchParams)
    {
        var querystring = HttpUtility.ParseQueryString(string.Empty);
        var properties = searchParams
            .GetType()
            .GetProperties()
            .Select(p =>
            {
                Debug.Assert(p.PropertyType.FullName != null);
                    
                return new
                {
                    name = p.Name,
                    value = p.GetValue(searchParams),
                    attribute = p.GetCustomAttribute(typeof(ParameterAttribute)),
                    type = p.PropertyType.FullName.StartsWith("System.Nullable")
                        ? p.PropertyType.GenericTypeArguments[0]
                        : p.PropertyType
                };
            })
            .Where(x => x.attribute != null);

        foreach (var property in properties)
        {
            if (property.value == null) continue;
            string value;

            if (property.value is Enum)
            {
                value = property.type
                    .GetMember(property.value.ToString())
                    .Single()
                    .GetCustomAttribute<ParameterAttribute>()
                    .Name;
            }

            else if (property.value is string)
                value = property.value.ToString();
                
            else if (property.value is IEnumerable enumerable)
            {
                var array = enumerable as object[] ?? enumerable.Cast<object>().ToArray();
                    
                if (!array.Any()) continue;
                value = string.Join(",", array.Select(o => o.ToString()));
            }
            else value = property.value.ToString();

            querystring.Add(ToSnakeCase(property.name), value);
        }

        var response = await _http.GetAsync($"property_listings.json?{querystring}&api_key={ApiKey}");
        var content = await response.Content.ReadAsStringAsync();
            
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(response.ReasonPhrase + ": " + content);

        return JsonConvert.DeserializeObject<PropertyListingsSearchResult>(content);
    }

    private string ToSnakeCase(string input)
    {
        var first = true;
        var builder = new StringBuilder();

        foreach (var letter in input)
        {
            if (char.IsUpper(letter) && !first) builder.Append('_');

            builder.Append(char.ToLowerInvariant(letter));
            first = false;
        }

        return builder.ToString();
    }
        
    public void Dispose()
    {
        _http?.Dispose();
    }
}