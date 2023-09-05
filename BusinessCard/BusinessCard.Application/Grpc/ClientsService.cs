using System.Text;
using System.Text.Json;
using BusinessCard.API.Application.Commands.AddMember;
using BusinessCard.API.Application.Commands.EditClient;
using BusinessCard.API.Application.Commands.EditMember;
using BusinessCard.API.Application.Commands.RemoveClient;
using BusinessCard.API.Application.Commands.UpsertClient;
using BusinessCard.API.Application.Common.Models;
using BusinessCard.API.Application.Queries.GetClientById;
using BusinessCard.API.Application.Queries.GetClients;
using BusinessCard.API.Application.Queries.GetMemberId;
using BusinessCard.API.Application.Queries.GetMembers;
using BusinessCard.API.Extensions;
using BusinessCard.Domain.Exceptions;
using ClientService;
using FluentValidation;
using FluentValidation.Results;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Primitives;

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
            ClientId = result.Id?.ToString(),
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
             ClientId = result.ToString(),
         };
    }

    /// <summary>
    /// Get Client by Id
    /// 'gRPC' implementation
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ClientGrpcResult> GetClientByIdGrpc(GetClientByIdGrpcQuery request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetClientByIdQuery(request.Id.ToGuid()));
        
        return ToClientResult(result);
    }

    /// <summary>
    /// Get client list - paginated
    /// 'gRPC' implementation
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<GetPaginatedClientsGrpcQueryResult> GetPaginatedClientsGrpc(GetPaginatedClientsGrpcQuery request, ServerCallContext context)
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
    
    /// <summary>
    /// Remove (soft delete) clients - deactivate all children as well
    /// 'gRPC' implementation
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    public override async Task<ClientGrpcCommandResult> RemoveClientGrpc(RemoveClientGrpcCommand request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var guid))
            throw new ValidationException("Validation Error",new []{new ValidationFailure("Id","Id is not a Guid.")});

        await _mediator.Send(new RemoveClientCommand(guid));

        return new ClientGrpcCommandResult { ClientId = guid.ToString() };

    }

    
    /// <summary>
    /// Add member to client
    /// 'gRPC' implementation
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    public override async Task<MemberGrpcCommandResult> AddMemberGrpc(AddMemberGrpcCommand request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.ClientId, out var guid))
            throw new ValidationException("Validation Error",new []{new ValidationFailure("Id","Id is not a Guid.")});

        var result = await _mediator.Send(ToAddMemberCommand(request));
        
        return new MemberGrpcCommandResult
        {
            MemberId = result.ToString(),
        };
    }

    public override async Task<MemberGrpcCommandResult> EditMemberGrpc(EditMemberGrpcCommand request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.ClientId, out var guid))
            throw new ValidationException("Validation Error",new []{new ValidationFailure("Id","Id is not a Guid.")});

        var result = await _mediator.Send(ToEditMemberCommand(request));
        
        return new MemberGrpcCommandResult
        {
            MemberId = result.ToString(),
        };
    }

    /// <summary>
    /// Get members of a client / company
    /// 'gRPC' implementation
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    public override async Task<GetPaginatedMembersGrpcQueryResult> GetPaginatedMembersGrpc(GetPaginatedMembersGrpcQuery request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.ClientId, out var guid))
            throw new ValidationException("Validation Error",
                new[] { new ValidationFailure("Id", "Id is not a Guid.") });

        var result = await _mediator.Send(new GetMembersQuery(request.PageSize,request.PageNumber,request.ClientId.ToGuid()));

        var grpcResult = new GetPaginatedMembersGrpcQueryResult()
        {
            PageSize = result.PageSize,
            PageNumber = result.PageNumber,
            TotalCount = result.TotalCount,
        };

        grpcResult.Members.AddRange(result.Members.Select(c => ToMemberGrpcResult(c)));

        return grpcResult;
    }

    /// <summary>
    /// Get member by Id
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<MemberGrpcResult> GetMemberByIdGrpc(GetMemberByIdGrpcQuery request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetMemberIdQuery(request.ClientId.ToGuid(), request.MemberId.ToGuid()));

        return ToMemberGrpcResult(result);
    }

    #region Transform
    
    private static SocialMediaObject GetSocialMedia(MembersResult member)
    {
        return JsonSerializer.Deserialize<SocialMediaObject>(member.SocialMedia);
    }
    

    private static string NameBuilder(string firstname, string lastname, string middleName, string nameSuffix)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(firstname).Append(" ");
        
        if (string.IsNullOrEmpty(middleName))
        {
            builder.Append(lastname);
        }
        else
        {
            builder.Append(middleName);
            builder.Append(" ");
            builder.Append(lastname);
        }
        
        builder.Append(" ");
        builder.Append(nameSuffix);

        return builder.ToString().TrimEnd();
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
            SubscriptionLevel = c.SubscriptionLevel.Value,
            CreatedBy = c.CreatedBy,
            CreatedOn = c.CreatedOn?.ToString("yyyy-MMM-dd"),
            ModifiedBy = c.ModifiedBy,
            ModifiedOn = c.ModifiedOn?.ToString("yyyy-MMM-dd"),
            IsActive = c.IsActive
        };
    }
    
    private static MemberGrpcResult ToMemberGrpcResult(GetMemberByIdQueryResult c)
    {
        return new MemberGrpcResult()
        {
            Address = c.Address,
            ClientId = c.ClientId.ToString(),
            Subscription = c.Subscription,
            SubscriptionLevel = c.SubscriptionLevel,
            FullName = NameBuilder(c.FirstName,c.LastName, c.MiddleName,c.NameSuffix),
            FirstName = c.FirstName,
            LastName = c.LastName,
            MiddleName = c.MiddleName,
            NameSuffix = c.NameSuffix,
            Email = c.Email,
            Facebook = c.Facebook,
            Id = c.Id.ToString(),
            Instagram = c.Instagram,
            Occupation = c.Occupation,
            Pinterest =  c.Pinterest,
            Twitter =  c.Twitter,
            LinkedIn =  c.LinkedIn,
            CardKey = c.CardKey,
            PhoneNumber = c.PhoneNumber,
        };
    }
    
    private static MemberGrpcResult ToMemberGrpcResult(MembersResult c)
    {
        return new MemberGrpcResult()
        {
            Address = c.Address,
            FirstName = c.FirstName,
            LastName = c.LastName,
            MiddleName = c.MiddleName,
            NameSuffix = c.NameSuffix,
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
    
    
    private static EditMemberCommand ToEditMemberCommand(EditMemberGrpcCommand request)
    {
        return new EditMemberCommand(
            clientId: request.ClientId.ToGuid(), 
            request.Id.ToGuid(),
            request.FirstName, 
            request.LastName,
            request.MiddleName, 
            request.NameSuffix, 
            request.PhoneNumber,
            request.Email, 
            request.Address,
            request.Occupation, 
            request.Facebook, 
            request.LinkedIn, 
            request.Instagram,
            request.Pinterest,
            request.Twitter,
            request.CardKey,
            request.SubscriptionLevel
            );
    }
    
    
    #endregion
}