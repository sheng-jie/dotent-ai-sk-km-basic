using System.Diagnostics.CodeAnalysis;
using Microsoft.SemanticKernel;
public class OpenAiRegister : AiProviderRegister
{
    public override AiProviderType AiProviderType => AiProviderType.OpenAI;

    protected override void RegisterChatCompletionService(IKernelBuilder builder, IServiceProvider provider,
        AiProvider aiProvider)
    {
        var chatModelId = aiProvider.GetChatCompletionApiService()?.ModelId;
        if (string.IsNullOrWhiteSpace(chatModelId))
        {
            return;
        }

        builder.AddOpenAIChatCompletion(modelId: chatModelId, apiKey: aiProvider.ApiKey);
    }

    [Experimental("SKEXP0010")]
    protected override void RegisterEmbeddingService(IKernelBuilder builder, IServiceProvider provider,
        AiProvider aiProvider)
    {
        var embeddingModelId = aiProvider.GetEmbeddingApiService()?.ModelId;
        if (string.IsNullOrWhiteSpace(embeddingModelId))
        {
            return;
        }

        builder.AddOpenAITextEmbeddingGeneration(embeddingModelId, aiProvider.ApiKey);
    }
}