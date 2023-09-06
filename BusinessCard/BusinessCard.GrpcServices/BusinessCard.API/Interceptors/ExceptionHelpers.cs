using BusinessCard.Domain.Exceptions;
using Grpc.Core;
using Microsoft.Data.SqlClient;

namespace BusinessCard.API.Interceptors;

public static class ExceptionHelpers
{  private const int HresultAlreadyExists = -2146232060;
    public static RpcException Handle<T>(this Exception exception, ServerCallContext context, ILogger<T> logger, 
        Guid correlationId) =>
        exception switch
        {
            TimeoutException => HandleTimeoutException((TimeoutException)exception, context, logger, correlationId),
            SqlException => HandleSqlException((SqlException)exception, context, logger, correlationId),
            RpcException => HandleRpcException((RpcException)exception, logger, correlationId),
            BusinessCardDomainException => HandleApiException((BusinessCardDomainException)exception, logger, correlationId),
            DbUpdateException => HandleDbUpdateException((DbUpdateException) exception, context,logger,correlationId),
            KeyNotFoundException => HandleNotFoundException((KeyNotFoundException)exception,context, logger, correlationId),
            ValidationException=> HandleValidationException((ValidationException) exception,context, logger, correlationId),
            _ => HandleDefault(exception, context, logger, correlationId)
        };

    private static RpcException HandleTimeoutException<T>(TimeoutException exception, ServerCallContext context, ILogger<T> logger, Guid correlationId)
    {
        logger.LogError(exception, "CorrelationId: {CorrelationId} - A timeout occurred", correlationId);

        var status = new Status(StatusCode.Internal, "An external resource did not answer within the time limit");

        return new RpcException(status, CreateTrailers(correlationId));
    }
    
  

    private static RpcException HandleDbUpdateException<T>(DbUpdateException exception, ServerCallContext context,
        ILogger<T> logger, Guid correlationId)
    {
        logger.LogError(exception, exception.InnerException?.Message);
        var status = exception.InnerException.HResult switch
        {
            HresultAlreadyExists => new Status(StatusCode.AlreadyExists, "Cannot insert duplicate key / index."),
            _ => new Status(StatusCode.Internal, "Error has occurred.")
        };
        return new RpcException(status, CreateTrailers(correlationId));
    }

    private static RpcException HandleSqlException<T>(SqlException exception, ServerCallContext context, ILogger<T> logger, Guid correlationId)
    {
        logger.LogError(exception, "CorrelationId: {CorrelationId} - An SQL error occurred", correlationId);
        Status status;

        if (exception.Number == -2)
        {
            status = new Status(StatusCode.DeadlineExceeded, "SQL timeout");
        }
        else
        {
            status = new Status(StatusCode.Internal, "SQL error");
        }
        return new RpcException(status, CreateTrailers(correlationId));
    }

    private static RpcException HandleRpcException<T>(RpcException exception, ILogger<T> logger, Guid correlationId)
    {
        logger.LogError(exception, "CorrelationId: {CorrelationId} - An error occurred", correlationId);
        var trailers = exception.Trailers;
        trailers.Add(CreateTrailers(correlationId)[0]);
        return new RpcException(new Status(exception.StatusCode, exception.Message), trailers);
    }
    
    private static RpcException HandleApiException<T>(BusinessCardDomainException exception,  ILogger<T> logger, Guid correlationId)
    {
        logger.LogError(exception, "CorrelationId: {CorrelationId} - An error occurred", correlationId);
        
            var errors = "";
            StatusCode status;

            if (exception.InnerException is ValidationException validationException)
            {
                errors = string.Join(Environment.NewLine, validationException.Errors.Select(c => c.ErrorMessage));
                status = StatusCode.InvalidArgument;
            }
            else if (exception.InnerException is KeyNotFoundException)
            {
                errors = exception.InnerException.Message;
                status = StatusCode.NotFound;
            }
            else
            {
                errors = exception.InnerException?.Message ?? exception.Message;
                status = StatusCode.Internal;
            }

        return new RpcException(new Status(status,errors), CreateTrailers(correlationId));
    }

    private static RpcException HandleDefault<T>(Exception exception, ServerCallContext context, ILogger<T> logger, Guid correlationId)
    {
        logger.LogError(exception, "CorrelationId: {CorrelationId} - An error occurred", correlationId);
        return new RpcException(new Status(StatusCode.Internal, exception.Message), CreateTrailers(correlationId));
    }
    
    private static RpcException HandleNotFoundException<T>(KeyNotFoundException exception, ServerCallContext context, ILogger<T> logger, Guid correlationId)
    {
        logger.LogError(exception, "CorrelationId: {CorrelationId} - A timeout occurred", correlationId);

        var status = new Status(StatusCode.NotFound, exception.Message);

        return new RpcException(status, CreateTrailers(correlationId));
    }
    
    private static RpcException HandleValidationException<T>(ValidationException exception, ServerCallContext context, ILogger<T> logger, Guid correlationId)
    {
        logger.LogError(exception, "CorrelationId: {CorrelationId} - A timeout occurred", correlationId);
        var errors = string.Join(Environment.NewLine, exception.Errors.Select(c => c.ErrorMessage));
        var status = new Status(StatusCode.InvalidArgument, errors);

        return new RpcException(status, CreateTrailers(correlationId));
    }

    /// <summary>
    ///  Adding the correlation to Response Trailers
    /// </summary>
    /// <param name="correlationId"></param>
    /// <returns></returns>
    private static Metadata CreateTrailers(Guid correlationId)
    {
        var trailers = new Metadata();
        trailers.Add("CorrelationId", correlationId.ToString());
        return trailers;
    }
}