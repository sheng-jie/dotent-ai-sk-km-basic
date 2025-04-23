using Microsoft.SemanticKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Net.Http;
using System.Threading;
public class OpenAiHttpClientHandler(ILogger<OpenAiHttpClientHandler> logger): HttpClientHandler
{  
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // if (request.RequestUri!.LocalPath=="/v1/chat/completions")
        // {
        //     var apiSvc = provider.GetChatCompletionApiService();
        //     if (apiSvc == null)
        //     {
        //         throw new ArgumentNullException($"未找到名称为 chat-completions 的 API 服务");
        //     }
            
        //     var uriBuilder = new UriBuilder(request.RequestUri)
        //     {
        //         Scheme = provider.ApiScheme,
        //         Port = provider.ApiPort,
        //         Host = provider.ApiHost,
        //         Path = apiSvc?.RoutePath
        //     };
            
        //     request.RequestUri = uriBuilder.Uri;
        // }

        // if (request.RequestUri!.LocalPath=="/v1/embeddings")
        // {
        //     var apiSvc = provider.GetEmbeddingApiService();
        //     if (apiSvc == null)
        //     {
        //         throw new ArgumentNullException($"未找到名称为 embeddings 的 API 服务");
        //     }
            
        //     var uriBuilder = new UriBuilder(request.RequestUri)
        //     {
        //         Scheme = provider.ApiScheme,
        //         Port = provider.ApiPort,
        //         Host = provider.ApiHost,
        //         Path = apiSvc?.RoutePath
        //     };
            
        //     request.RequestUri = uriBuilder.Uri;
        // }

        var content = await request.Content.ReadAsStringAsync(cancellationToken);
        
        logger.LogInformation("Sending '{Request.Method}' to '{Request.Host}{Request.Path}' with content {Request.Content}",
            request.Method,
            request.RequestUri?.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped),
            request.RequestUri!.PathAndQuery,
            content);
        
        var response = await base.SendAsync(request, cancellationToken);


        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        
        logger.LogInformation(
            "Received '{Response.StatusCodeInt} {Response.StatusCodeString}' with content {Response.Content}",
            (int)response.StatusCode,
            response.StatusCode,
            responseContent
        );
        
        return response;
    }
}