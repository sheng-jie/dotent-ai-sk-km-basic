public static class AiProviderRegisterFactory
{
    public static AiProviderRegister Create(AiProviderType aiProviderType)
    {
        return aiProviderType switch
        {
            AiProviderType.OpenAI => new OpenAiRegister(),
            AiProviderType.OpenAI_Compatible => new OpenAiCompatibleAiRegister(),
            AiProviderType.AzureOpenAI => new AzureOpenAiRegister(),
            _ => throw new NotImplementedException($"No AI register for nameof(aiProviderType)")
        };
    }
}