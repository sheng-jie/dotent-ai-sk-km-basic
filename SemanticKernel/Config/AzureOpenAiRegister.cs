using System.Diagnostics.CodeAnalysis;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
public class AzureOpenAiRegister : AiProviderRegister
{
    public override AiProviderType AiProviderType => AiProviderType.AzureOpenAI;

    protected override void RegisterChatCompletionService(IKernelBuilder builder, IServiceProvider provider,
        AiProvider aiProvider)
    {
        var modelId = aiProvider.GetChatCompletionApiService()?.ModelId;

        // 如果未找到ModelId，说明该AI提供商未提供对应服务，则直接返回builder
        if (string.IsNullOrWhiteSpace(modelId))
        {
            return;
        }

        // builder.AddAzureOpenAIChatCompletion(
        //     deploymentName: modelId,
        //     endpoint: aiProvider.ApiEndpoint,
        //     apiKey: aiProvider.ApiKey);

        var logger = provider.GetRequiredService<ILoggerFactory>().CreateLogger<OpenAiHttpClientHandler>();
        var httpClient = new HttpClient(new OpenAiHttpClientHandler(logger));

        builder.AddAzureOpenAIChatCompletion(
            deploymentName: modelId,
            endpoint: aiProvider.ApiEndpoint,
            apiKey: aiProvider.ApiKey,
            httpClient: httpClient);
        builder.AddAzureOpenAIChatCompletion(
            deploymentName: modelId,
            endpoint: aiProvider.ApiEndpoint,
            apiKey: aiProvider.ApiKey,
            httpClient: httpClient);
    }

    [Experimental("SKEXP0010")]
    protected override void RegisterEmbeddingService(IKernelBuilder builder, IServiceProvider provider,
        AiProvider aiProvider)
    {
        var modelId = aiProvider.GetEmbeddingApiService()?.ModelId;

        // 如果未找到ModelId，说明该AI提供商未提供对应服务，则直接返回builder
        if (string.IsNullOrWhiteSpace(modelId))
        {
            return;
        }

        builder.AddAzureOpenAITextEmbeddingGeneration(
            deploymentName: modelId,
            endpoint: aiProvider.ApiEndpoint,
            apiKey: aiProvider.ApiKey);
    }
}