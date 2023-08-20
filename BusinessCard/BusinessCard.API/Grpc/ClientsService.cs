using BusinessCard.API.Application.Commands.EditClientCommandHandler;
using BusinessCard.API.Application.Commands.UpsertClient;
using BusinessCard.API.Exceptions;
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

    public override async Task<ClientGrpcCommandResult> AddClientGrpc(AddClientGrpcCommand request, ServerCallContext context)
    {
        var result =
            await _mediator.Send(
                new AddClientCommand(request.CompanyName, request.IsDiscreet, request.Subscription));
        return new ClientGrpcCommandResult
        {
            Id = result.Id?.ToString(),
        };
    }

    public override async Task<ClientGrpcCommandResult> EditClientGrpc(EditClientGrpcCommand request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var guid))
            throw BusinessCardApiException.Create(new ArgumentException("Id is not a Guid."));
        
         var result = await _mediator.Send(new EditClientCommand(guid, request.CompanyName,request.Subscription,request.IsDiscreet));
         return new ClientGrpcCommandResult
         {
             Id = result.ToString(),
         };

        return null;

    }
}