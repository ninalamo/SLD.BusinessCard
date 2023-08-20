using BusinessCard.API.Exceptions;
using BusinessCard.Domain.Exceptions;
using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace BusinessCard.API.Interceptors;

public class ServerInterceptor : Interceptor
{
    private readonly ILogger _logger;
    private const int HresultAlreadyExists = -2146232060;

    public ServerInterceptor(ILogger<ServerInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation("Starting receiving call. Type/Method: {Type} / {Method}",
            MethodType.Unary, context.Method);
        try
        {
            return await continuation(request, context);
        }
        catch (DbUpdateException dbEx) when (dbEx.InnerException is not null)
        {
            _logger.LogError(dbEx, dbEx.InnerException?.Message);
            var status = dbEx.InnerException.HResult switch
            {
                HresultAlreadyExists => new Status(StatusCode.AlreadyExists, "Value already exists."),
                _ => new Status(StatusCode.Internal, "Error has occurred.")
            };
            throw new RpcException(status, dbEx.InnerException.Message);
        }
        catch (BusinessCardDomainException domEx) when (domEx.InnerException is ValidationException inEx)
        {
            _logger.LogError(domEx, inEx.Message);
            var errors = string.Join(Environment.NewLine, inEx.Errors.Select(c => c.ErrorMessage));
            throw new RpcException(new Status(StatusCode.InvalidArgument, errors));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error thrown by {context.Method}.");
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }
}
