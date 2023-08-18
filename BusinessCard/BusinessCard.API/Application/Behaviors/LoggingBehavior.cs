using BusinessCard.API.Extensions;
using BusinessCard.API.Logging;
using MediatR;

namespace BusinessCard.API.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILoggerAdapter<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILoggerAdapter<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("----- Handling command {CommandName} ({@Command})", request.GetGenericTypeName(),
            request);
        var response = await next();
        _logger.LogInformation("----- Command {CommandName} handled - response: {@Response}",
            request.GetGenericTypeName(), response);

        return response;
    }
}

