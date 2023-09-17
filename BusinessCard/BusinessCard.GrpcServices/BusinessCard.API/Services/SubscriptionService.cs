using BusinessCard.API.Extensions;
using BusinessCard.Application.Application.Commands.AddSubscription;
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

    public override async Task<PaginatedSubscriptionGrpcQueryResult> GetSubscriptionsByIdGrpc(GetByIdGrpcQuery request, ServerCallContext context)
    {
        _logger.LogInformation( $"gRPC Service: {nameof(GetSubscriptionsByIdGrpc)} - {nameof(GetByIdGrpcQuery)}. Transforming:{request}");

        // var command = new AddSubscriptionCommand(
        //     clientId: request.ClientId.ToGuid(),
        //     planId: request.PlanId.ToGuid(),
        //     startDate: request.StartDate.ToDateTimeOffset(),
        //     endDate: request.EndDate.ToDateTimeOffset(),
        //     cardExpiryInMonth: request.NumberOfMonthToExpire,
        //     level: request.CardLevel);
        //
        // _logger.LogInformation( $"gRPC Service: {nameof(GetSubscriptionsByIdGrpc)} - {nameof(AddSubscriptionCommand)}. Sending:{command}");

        //var result = await _mediator.Send(command, context.CancellationToken);

        return null; //new GrpcCommandResult() { Id = result.ToString() };
    }
}