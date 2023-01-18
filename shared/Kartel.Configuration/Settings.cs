using Microsoft.Extensions.Configuration;

namespace Kartel.Configuration;

public abstract class Settings
{
    public static T FromArgs<T>(string[] args)
    {
        var fileName = args.Any()
            ? $"appsettings.{args[0].ToLower()}.json"
            : "appsettings.json";

        var config = new ConfigurationBuilder().AddJsonFile(fileName).Build();
        return config.GetRequiredSection("Network").Get<T>();
    }
}