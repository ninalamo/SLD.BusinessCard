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

    public override async Task<ClientGrpcCommandResult> AddClientGrpc(AddClientGrpcCommand request, ServerCallContext context)
    {
        var result = await _mediator.Send(new AddClientCommand( request.CompanyName,request.IsDiscreet,request.Subscription));
        return new ClientGrpcCommandResult
        {
            ErrorMessage = result.ErrorMessage,
            Id = result.Id?.ToString(),
            IsSuccess = result.IsSuccess
        };
    }

    public override Task<ClientGrpcCommandResult> EditClientGrpc(EditClientGrpcCommand request, ServerCallContext context)
    {
        //TODO: Fix this shit
        // var result = await _mediator.Send(new EditClientCommand(request.Id, request.CompanyName,request.IsDiscreet,request.Subscription));
        // return new ClientGrpcCommandResult
        // {
        //     ErrorMessage = result.ErrorMessage,
        //     Id = result.Id?.ToString(),
        //     IsSuccess = result.IsSuccess
        // };

        return null;

    }
}