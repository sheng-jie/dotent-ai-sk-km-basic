using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

public static class SkHelper
{
    public static Kernel BuildKernel()
    {
        // 引入交互式的内核命名空间，以便用户输入
        using PolyglotKernel = Microsoft.DotNet.Interactive.Kernel;
        var aiProviderCode = await PolyglotKernel.GetInputAsync("请输入AI服务提供商编码：");

        //Create Kernel builder
        var builder = Kernel.CreateBuilder();

        builder.AddChatCompletionService(aiProviderCode);

        var kernel = builder.Build();

        return kernel;
    }
}