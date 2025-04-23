// 注册 MCP Server
public static async Task AddMcpServer(
  this Kernel kernel, string name, string command, 
  string version = "1.0.0", 
  string[] args = null, 
  Dictionary<string, string> env = null
  ) {
    var clientOptions = new McpClientOptions() {
        ClientInfo = new McpDotNet.Protocol.Types.Implementation() { 
          Name = name, Version = version 
        },
    };

    var serverConfig = new McpServerConfig() {
        Id = name,
        Name = name,
        TransportType = "stdio",
        TransportOptions = new Dictionary<string, string> {
            ["command"] = command,
            ["arguments"] = string.Join(' ', args ?? []),
        }
    };

    var loggerFactory = kernel.Services.GetRequiredService<ILoggerFactory>();
    var clientFactory = new McpClientFactory([serverConfig], clientOptions, loggerFactory);

    var client = await clientFactory.GetClientAsync(serverConfig.Id).ConfigureAwait(false);
    var kernelFunctions = await client.GetKernelFunctionsAsync();
    kernel.Plugins.AddFromFunctions(name, kernelFunctions);
}