using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

public abstract class AiProviderRegister
{
    public abstract AiProviderType AiProviderType { get; }

    public virtual void Register(IServiceCollection services, AiProvider aiProvider)
    {
        // 为指定AiProvider 注册专用Kernel服务
        services.AddKeyedTransient<Kernel>(aiProvider.Code, (sp, key) => BuildKernel(sp, aiProvider));
    }

    public virtual Kernel BuildKernel(IServiceProvider serviceProvider, AiProvider aiProvider)
    {
        // 创建Kernel构建器
        var builder = Kernel.CreateBuilder();

        // 注册日志服务
        builder.Services.AddNLogging();

        RegisterChatCompletionService(builder, serviceProvider, aiProvider);
        RegisterEmbeddingService(builder, serviceProvider, aiProvider);
        
        // Register other services if needed

        return builder.Build();
    }

    protected abstract void RegisterChatCompletionService(IKernelBuilder builder, IServiceProvider provider, AiProvider aiProvider);

    protected abstract void RegisterEmbeddingService(IKernelBuilder builder, IServiceProvider provider, AiProvider aiProvider);
}