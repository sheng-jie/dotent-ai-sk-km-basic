public  class AiProvider
{
    /// <summary>
    /// AI 服务提供商名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// AI 服务提供商编码
    /// </summary>
    public string Code { get; set; }

    public string ApiKey { get; set; }

    public string ApiEndpoint { get; set; }
    
    
    public  AiProviderType AiType { get; set; }

    public List<ApiService> ApiServices { get; set; }

    public ApiService? GetEmbeddingApiService() => GetApiService("embeddings");

    public ApiService? GetChatCompletionApiService() => GetApiService("chat-completions");

    private ApiService? GetApiService(string apiServiceName) => ApiServices.FirstOrDefault(x => x.Name == apiServiceName);
}

public enum AiProviderType
{
    OpenAI,
    AzureOpenAI,
    OpenAI_Compatible,
    Onnx,
    Other
}

/// <summary>
/// AI 服务提供商提供的API 服务
/// </summary>
/// <param name="Name">Api名称</param>
/// <param name="ModelId">模型编码</param>
///  <param name="Models">可用模型列表</param>
public record ApiService(string Name, string ModelId, string[]? ModelIds = null);