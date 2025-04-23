using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.Net.Http;
using System.Diagnostics.CodeAnalysis;

#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010

public static IServiceCollection RegisterKernels(this IServiceCollection services)
{
    // 从配置文件中加载AI配置
    var aiOptions = AiSettings.LoadAiProvidersFromFile();
    // 注册其他AI服务提供商
    foreach (var aiProvider in aiOptions.Providers)
    {
        var providerRegister = AiProviderRegisterFactory.Create(aiProvider!.AiType);
    
        providerRegister.Register(services, aiProvider);
    }
    return services;
}