using BusinessCard.API.Application.Commands.AddMember;
using BusinessCard.API.Application.Commands.EditClientCommandHandler;
using BusinessCard.API.Application.Commands.UpsertClient;
using BusinessCard.API.Exceptions;
using BusinessCard.API.Extensions;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using BusinessCard.Domain.Exceptions;
using ClientService;
using FluentValidation;
using FluentValidation.Results;
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

    /// <summary>
    /// Adds client (company) to database.
    /// 'gRPC' implementation
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ClientGrpcCommandResult> AddClientGrpc(AddClientGrpcCommand request, ServerCallContext context)
    {
        var result = await _mediator.Send(ToAddClientCommand(request));
        return new ClientGrpcCommandResult
        {
            Id = result.Id?.ToString(),
        };
    }

    /// <summary>
    /// Updates client (company) details.
    /// 'gRPC' implementation
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="BusinessCardApiException"></exception>
    public override async Task<ClientGrpcCommandResult> EditClientGrpc(EditClientGrpcCommand request, ServerCallContext context)
    {
         var result = await _mediator.Send(ToEditClientCommand(request));
         return new ClientGrpcCommandResult
         {
             Id = result.ToString(),
         };
    }

    public override async Task<ClientGrpcCommandResult> AddMemberGrpc(AddMemberGrpcCommand request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.ClientId, out var guid))
            throw BusinessCardApiException.Create(new ArgumentException("Id is not a Guid."));

        var result = await _mediator.Send(ToAddMemberCommand(request));
        
        return new ClientGrpcCommandResult
        {
            Id = result.ToString(),
        };
    }
    
    #region Transform
    
    private static AddClientCommand ToAddClientCommand(AddClientGrpcCommand request)
    {
        return new AddClientCommand(request.CompanyName, request.IsDiscreet, request.Subscription);
    }
    
    private static EditClientCommand ToEditClientCommand(EditClientGrpcCommand request)
    {
        return new EditClientCommand(request.Id.ToGuid(), request.CompanyName,request.Subscription,request.IsDiscreet);
    }


    private static AddMemberCommand ToAddMemberCommand(AddMemberGrpcCommand request)
    {
        return new AddMemberCommand(request.ClientId.ToGuid(), request.FirstName, request.LastName,
            request.MiddleName, request.NameSuffix, request.PhoneNumber, request.Email, request.Address,
            request.Occupation, request.Facebook, request.LinkedIn, request.Instagram, request.Pinterest,
            request.Twitter);
    }
    #endregion
}