using System.IO;
using Microsoft.Extensions.Configuration;
public static class AiSettings
{
    public static AiOptions LoadAiProvidersFromFile()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Config/appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var aiOptions = configuration.GetSection("AI").Get<AiOptions>();
        return aiOptions;
    }
}