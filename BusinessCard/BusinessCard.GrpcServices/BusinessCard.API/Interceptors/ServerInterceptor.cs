using Grpc.Core;
using Grpc.Core.Interceptors;

namespace BusinessCard.API.Interceptors;

public class ServerInterceptor : Interceptor
{
    private readonly ILogger<ServerInterceptor> _logger;
    private readonly Guid _correlationId;

    public ServerInterceptor(ILogger<ServerInterceptor> logger)
    {
        _logger = logger;
        _correlationId = Guid.NewGuid();
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation("Starting receiving call. Type/Method: {Type} / {Method}",
            MethodType.Unary, context.Method);
        try
        {
            _logger.LogInformation( "CorrelationId: {CorrelationId} - Server Interceptor", _correlationId);

            return await continuation(request, context);
        }
        catch (Exception e)
        {
            throw e.Handle(context, _logger, _correlationId);
        }
    }
}