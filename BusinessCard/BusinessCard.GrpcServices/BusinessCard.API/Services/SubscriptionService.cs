using BusinessCard.API.Extensions;
using BusinessCard.Application.Application.Commands.AddSubscription;
using BusinessCard.Application.Application.Queries.GetClients;
using BusinessCard.Application.Application.Queries.GetClientSubscriptions;
using BusinessCard.Application.Extensions;

using ClientService;
using Common;
using Grpc.Core;
using SubscriptionService;

namespace BusinessCard.API.Services;

public class SubscriptionService : SubscriptionGrpc.SubscriptionGrpcBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SubscriptionService> _logger;

    public SubscriptionService(IMediator mediator, ILogger<SubscriptionService> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    
    public override async Task<GrpcCommandResult> AddSubscriptionGrpc(AddSubscriptionGrpcCommand request, ServerCallContext context)
    {
        
        _logger.LogInformation( $"gRPC Service: {nameof(AddSubscriptionGrpc)} - {nameof(AddSubscriptionGrpcCommand)}. Transforming:{request}");

        var command = new AddSubscriptionCommand(
            clientId: request.ClientId.ToGuid(),
            planId: request.PlanId.ToGuid(),
            startDate: request.StartDate.ToDateTimeOffset(),
            endDate: request.EndDate.ToDateTimeOffset(),
            cardExpiryInMonth: request.NumberOfMonthToExpire,
            level: request.CardLevel);
        
        _logger.LogInformation( $"gRPC Service: {nameof(AddSubscriptionGrpc)} - {nameof(AddSubscriptionCommand)}. Sending:{command}");

        var result = await _mediator.Send(command, context.CancellationToken);

        return new GrpcCommandResult() { Id = result.ToString() };
    }


    public override async Task<ClientSubscriptionsGrpcQueryResult> GetClientSubscriptionsGrpc(GetByIdGrpcQuery request, ServerCallContext context)
    {
        _logger.LogInformation( $"gRPC Service: {nameof(GetClientSubscriptionsGrpc)} - {nameof(GetByIdGrpcQuery)}. Transforming:{request}");

        var query = new GetClientSubscriptionsQuery(request.Id.ToGuid());

        _logger.LogInformation( $"gRPC Service: {nameof(GetClientSubscriptionsGrpc)} - {nameof(GetByIdGrpcQuery)}. Sending: {query}");

        var result = await _mediator.Send(query, context.CancellationToken);

        var grpcResult = new ClientSubscriptionsGrpcQueryResult();

        var subscriptions = result.Result.Select(i => new ClientSubscriptionGrpcResult()
        {
            ClientId = i.ClientId.ToString(),
            CardLevel = i.Level,
            EndDate = i.EndDate.ToString(),
            NumberOfMonthToExpire = i.CardExpiryInMonths,
            PlanId = i.BillingPlanId.ToString(),
            StatusDescription = i.StateDescription,
            StartDate = i.StartDate.ToString(),
            Status = (int)i.State,
            SubscriptionId = i.Id.ToString(),
            ActualEndDate = i.ActualEndDate?.ToString() ?? default(DateTimeOffset).ToString(),
            Description = i.Description ?? string.Empty,
            Reason = i.Reason
        });
        
        grpcResult.Result.AddRange(subscriptions);
        
        _logger.LogInformation( $"gRPC Service: {nameof(GetClientSubscriptionsGrpc)} - {nameof(ClientSubscriptionsGrpcQueryResult)}. Received: {result}");

        return grpcResult;
    }

  
}