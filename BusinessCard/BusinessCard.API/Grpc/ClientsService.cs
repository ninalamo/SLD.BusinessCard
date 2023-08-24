using System.Text;
using BusinessCard.API.Application.Commands.AddMember;
using BusinessCard.API.Application.Commands.EditClientCommandHandler;
using BusinessCard.API.Application.Commands.UpsertClient;
using BusinessCard.API.Application.Common.Models;
using BusinessCard.API.Application.Queries.GetClientById;
using BusinessCard.API.Application.Queries.GetClients;
using BusinessCard.API.Application.Queries.GetMembers;
using BusinessCard.API.Extensions;
using BusinessCard.Domain.Exceptions;
using ClientService;
using FluentValidation;
using FluentValidation.Results;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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

    public override async Task<ClientGrpcResult> GetClientByIdGrpc(GetClientByIdGrpcQuery request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetClientByIdQuery(request.Id.ToGuid()));
        
        return ToClientResult(result);
    }

    public override async Task<GetPaginatedClientsGrpcQueryResult> GetClientsGrpc(GetPaginatedClientsGrpcQuery request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetClientsQuery(request.PageSize, request.PageNumber, request.Name));

        var grpcResult = new GetPaginatedClientsGrpcQueryResult()
        {
            PageSize = result.PageSize,
            PageNumber = result.PageNumber,
            TotalCount = result.TotalCount,
        };
        
        grpcResult.Clients.AddRange(result.Clients.Select(c => ToClientResult(c)));

        return grpcResult;
    }

    public override async Task<ClientGrpcCommandResult> AddMemberGrpc(AddMemberGrpcCommand request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.ClientId, out var guid))
            throw new ValidationException("Validation Error",new []{new ValidationFailure("Id","Id is not a Guid.")});

        var result = await _mediator.Send(ToAddMemberCommand(request));
        
        return new ClientGrpcCommandResult
        {
            Id = result.ToString(),
        };
    }

    public override async Task<GetPaginatedMembersByIdQueryResult> GetMembersByClientIdGrpc(GetMembersByIdQuery request,
        ServerCallContext context)
    {
        if (!Guid.TryParse(request.ClientId, out var guid))
            throw new ValidationException("Validation Error",
                new[] { new ValidationFailure("Id", "Id is not a Guid.") });

        var result = await _mediator.Send(new GetMembersQuery(request.PageSize,request.PageNumber,request.ClientId.ToGuid()));

        var grpcResult = new GetPaginatedMembersByIdQueryResult()
        {
            PageSize = result.PageSize,
            PageNumber = result.PageNumber,
            TotalCount = result.TotalCount,
        };

        grpcResult.Members.AddRange(result.Members.Select(c => new MemberGrpcResult()
        {
            Address = c.Address,
            ClientId = c.ClientId.ToString(),
            Subscription = c.Subscription,
            SubscriptionLevel = c.SubscriptionLevel,
            FullName = NameBuilder(c),
            Email = c.Email,
            Facebook = GetSocialMedia(c).Facebook,
            Id = c.Id.ToString(),
            Instagram = GetSocialMedia(c).Instagram,
            Occupation = c.Occupation,
            Pinterest =  GetSocialMedia(c).Pinterest,
            Twitter =  GetSocialMedia(c).Twitter,
            LinkedIn =  GetSocialMedia(c).LinkedIn,
            CardKey = c.CardKey,
            PhoneNumber = c.PhoneNumber,
        }));

        return grpcResult;
    }

    

    #region Transform
    
    private static SocialMediaObject GetSocialMedia(MembersResult member)
    {
        return JsonSerializer.Deserialize<SocialMediaObject>(member.SocialMedia);
    }

    private static string NameBuilder(MembersResult result)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(result.FirstName);
        builder.Append(" ");

        if (string.IsNullOrEmpty(result.MiddleName))
        {
            builder.Append(result.LastName);
        }
        else
        {
            builder.Append(result.MiddleName);
            builder.Append(" ");
            builder.Append(result.LastName);
        }

        builder.Append(" ");
        builder.Append(result.NameSuffix);

        return builder.ToString().TrimEnd();
    }
    private static ClientGrpcResult ToClientResult(ClientsResult c)
    {
        return  new ClientGrpcResult
        {
            CardHolders = c.CardHolders,
            CompanyName = c.CompanyName,
            ClientId = c.ClientId.ToString(),
            IsDiscreet = c.IsDiscreet,
            NonCardHolders = c.NonCardHolders,
            Subscription = c.Subscription,
            SubscriptionLevel = c.SubscriptionLevel,
            CreatedBy = c.CreatedBy,
            CreatedOn = c.CreatedOn?.ToString("yyyy-MMM-dd"),
            ModifiedBy = c.ModifiedBy,
            ModifiedOn = c.ModifiedOn?.ToString("yyyy-MMM-dd"),
            IsActive = c.IsActive
        };
    }
    
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