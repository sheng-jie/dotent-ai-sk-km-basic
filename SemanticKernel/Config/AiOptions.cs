public class AiOptions
{
    public string DefaultProvider { get; set; }
    public List<AiProvider> Providers { get; set; }

    public AiProvider? GetProvider(string providerCode) =>
        Providers.FirstOrDefault(x => x.Code == providerCode);
}