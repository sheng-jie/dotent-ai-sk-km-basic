// KM 核心库
#r "nuget: Microsoft.KernelMemory.Core" 
#r "nuget: Microsoft.KernelMemory.AI.AzureOpenAI"
#r "nuget: Microsoft.KernelMemory.MemoryDb.Qdrant"
// 用于从.env 文件中读取配置
#r "nuget: dotenv.net"

using Microsoft.KernelMemory;
using Microsoft.KernelMemory.DocumentStorage.DevTools;
using Microsoft.KernelMemory.FileSystem.DevTools;
using Microsoft.KernelMemory.MemoryStorage.DevTools;
using dotenv.net;

public MemoryServerless GetSimpleMemory()
{
    var memoryBuilder = GetSimpleKmBuilderWithAzure();
    
    var memory = memoryBuilder.Build<MemoryServerless>();

    return memory;
}


public IKernelMemoryBuilder GetSimpleKmBuilderWithAzure()
{
    // 读取配置
    var env = DotEnv.Fluent()
        .WithEnvFiles("Config/.env")
        .WithExceptions()
        .WithTrimValues()
        .WithDefaultEncoding()
        .Read();

    var chatConfig = new AzureOpenAIConfig()
    {
        Auth = AzureOpenAIConfig.AuthTypes.APIKey,
        Endpoint = env["AZURE_OPENAI_ENDPOINT"],
        APIKey = env["AZURE_OPENAI_API_KEY"],
        Deployment = env["AZURE_CHAT_DEPLOYMENT"]
    };
    var embeddingConfig = new AzureOpenAIConfig()
    {
        Auth = AzureOpenAIConfig.AuthTypes.APIKey,
        Endpoint = env["AZURE_OPENAI_ENDPOINT"],
        APIKey = env["AZURE_OPENAI_API_KEY"],
        Deployment = env["AZURE_EMBEDDINGS_DEPLOYMENT"]
    };

    // 配置文件存储，用于存储生成的文件，存储在本地磁盘的 tmp_files 目录下
    var simpleFileStorageConfig = new SimpleFileStorageConfig()
    {
        StorageType = FileSystemTypes.Disk,
        Directory = "tmp_files"
    };

    // 配置向量数据库，用于存储生成的向量，存储在本地磁盘的 tmp_vectors 目录下
    var simpleVectorDbConfig = new SimpleVectorDbConfig()
    {
        StorageType = FileSystemTypes.Disk,
        Directory = "tmp_vectors"
    };

    var memoryBuilder = new KernelMemoryBuilder()
        .WithAzureOpenAITextEmbeddingGeneration(embeddingConfig) // 使用 Azure OpenAI 作为文本嵌入生成器
        .WithAzureOpenAITextGeneration(chatConfig) // 使用 Azure OpenAI 作为文本生成器
        .WithSimpleFileStorage(simpleFileStorageConfig)
        .WithSimpleVectorDb(simpleVectorDbConfig);

    return memoryBuilder;
}

public MemoryServerless GetQdrantMemory()
{
    // 读取配置
    var env = DotEnv.Fluent()
        .WithEnvFiles("Config/.env")
        .WithExceptions()
        .WithTrimValues()
        .WithDefaultEncoding()
        .Read();

    var chatConfig = new AzureOpenAIConfig()
    {
        Auth = AzureOpenAIConfig.AuthTypes.APIKey,
        Endpoint = env["AZURE_OPENAI_ENDPOINT"],
        APIKey = env["AZURE_OPENAI_API_KEY"],
        Deployment = env["AZURE_CHAT_DEPLOYMENT"]
    };
    var embeddingConfig = new AzureOpenAIConfig()
    {
        Auth = AzureOpenAIConfig.AuthTypes.APIKey,
        Endpoint = env["AZURE_OPENAI_ENDPOINT"],
        APIKey = env["AZURE_OPENAI_API_KEY"],
        Deployment = env["AZURE_EMBEDDINGS_DEPLOYMENT"]
    };

    var memory = new KernelMemoryBuilder()
        .WithAzureOpenAITextEmbeddingGeneration(embeddingConfig) // 使用 Azure OpenAI 作为文本嵌入生成器
        .WithAzureOpenAITextGeneration(chatConfig) // 使用 Azure OpenAI 作为文本生成器
        .WithQdrantMemoryDb("http://localhost:6333") // 使用 Qdrant 作为向量数据库
        .Build<MemoryServerless>();

    return memory;
}