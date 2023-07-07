using BusinessCard.API.Application.Commands.UpsertClient;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Exceptions;
using ClientService;
using Grpc.Core;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BusinessCard.API.Grpc;

public class ClientsService : ClientGrpc.ClientGrpcBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ClientsService> _logger;

    public ClientsService(IMediator mediator, ILogger<ClientsService> logger)
    {
        _mediator = mediator ?? throw BusinessCardDomainException.CreateArgumentNullException(nameof(mediator));
        _logger = logger ?? throw BusinessCardDomainException.CreateArgumentNullException(nameof(logger));
    }

    public override async Task<UpsertClientGrpcCommandResult> UpsertClient(UpsertClientGrpcCommand request, ServerCallContext context)
    {
        Guid? id = string.IsNullOrEmpty(request.Id) ? default : Guid.Parse(request.Id);
        var result = await _mediator.Send(new UpsertClientCommand(id, request.CompanyName,request.IsDiscreet,(Tier)request.Subscription));
        return new UpsertClientGrpcCommandResult
        {
            ErrorMessage = result.ErrorMessage,
            Id = result.Id?.ToString(),
            IsSuccess = result.IsSuccess
        };
    }
    


    #region snippet
    
    #endregion
}