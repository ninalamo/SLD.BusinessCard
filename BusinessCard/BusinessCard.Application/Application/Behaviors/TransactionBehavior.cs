using BusinessCard.API.Extensions;
using BusinessCard.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog.Context;

namespace BusinessCard.API.Application.Behaviors;

public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly LokiContext _dbContext;

    private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
    //TODO: Not this time ->private readonly ICatalogIntegrationEventService _orderingIntegrationEventService;

    public TransactionBehaviour(LokiContext dbContext,
        //TODO: Not this time ->ICatalogIntegrationEventService orderingIntegrationEventService,
        ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentException(nameof(LokiContext));
        //TODO: Not this time ->_orderingIntegrationEventService = orderingIntegrationEventService ?? throw new ArgumentException(nameof(orderingIntegrationEventService));
        _logger = logger ?? throw new ArgumentException(nameof(ILogger));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = default(TResponse);
        var typeName = request.GetGenericTypeName();


        try
        {

            //TODO: we might not need this but just in case we need to do transactions in SQL
             if (_dbContext.HasActiveTransaction)
             {
                 return await next();
             }
            
             var strategy = _dbContext.Database.CreateExecutionStrategy();

             await strategy.ExecuteAsync(async () =>
             {
                 Guid transactionId;
            
                 using var transaction = await _dbContext.BeginTransactionAsync();
                 using (LogContext.PushProperty("TransactionContext", transaction.TransactionId))
                 {
                    _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                    response = await next();

                    _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                     await _dbContext.CommitTransactionAsync(transaction);
            
                     transactionId = transaction.TransactionId;
                 }
            
                 //TODO: Not this time -> await _orderingIntegrationEventService.PublishEventsThroughEventBusAsync(transactionId);
             });

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);

            throw;
        }
    }
}