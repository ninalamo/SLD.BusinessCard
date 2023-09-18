using System.Text;
using System.Text.Json;
using BusinessCard.API.Extensions;
using BusinessCard.Application.Application.Commands.AddClient;
using BusinessCard.Application.Application.Commands.AddMember;
using BusinessCard.Application.Application.Commands.AddMemberWithIdentityKey;
using BusinessCard.Application.Application.Commands.AddSubscription;
using BusinessCard.Application.Application.Commands.EditClient;
using BusinessCard.Application.Application.Commands.EditMember;
using BusinessCard.Application.Application.Commands.RemoveClient;
using BusinessCard.Application.Application.Common.Models;
using BusinessCard.Application.Application.Queries.GetClientById;
using BusinessCard.Application.Application.Queries.GetClients;
using BusinessCard.Application.Application.Queries.GetMemberByIdAndUid;
using BusinessCard.Application.Application.Queries.GetMembers;
using BusinessCard.Application.Extensions;
using BusinessCard.Domain.Exceptions;
using ClientService;
using FluentValidation.Results;
using Google.Protobuf.Collections;
using Grpc.Core;

namespace BusinessCard.API.Services;

public class ClientsService : ClientGrpc.ClientGrpcBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ClientsService> _logger;

    public ClientsService(IMediator mediator, ILogger<ClientsService> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        _logger.LogInformation( $"gRPC Service: {nameof(ClientsService)} - {nameof(AddClientGrpc)}. Sending:{request}");

        var result = await _mediator.Send(new AddClientCommand(request.CompanyName, request.Industry));
        
        _logger.LogInformation( $"gRPC Service: {nameof(ClientsService)} - {nameof(AddClientGrpc)}. Received:{result}");

        return new ClientGrpcCommandResult
        {
            Id = result.ToString(),
        };
    }
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<MemberGrpcCommandResult> AddMemberWithIdentityGrpc(AddMemberWithIdentityGrpcCommand request, ServerCallContext context)
    {
        _logger.LogInformation( $"gRPC Service: {nameof(ClientsService)} - {nameof(AddMemberWithIdentityGrpc)}. Sending:{request}");

        var result = await _mediator.Send(ToAddMemberWithIdentityKeyCommand(request));
        
        _logger.LogInformation( $"gRPC Service: {nameof(ClientsService)} - {nameof(AddMemberWithIdentityGrpc)}. Received:{result}");

        return new MemberGrpcCommandResult()
        {
            MemberId = result.ToString()
        };
    }

    /// <summary>
    /// Updates client (company) details.
    /// 'gRPC' implementation
    /// </summary>d
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="BusinessCardApiException"></exception>
    public override async Task<ClientGrpcCommandResult> EditClientGrpc(EditClientGrpcCommand request, ServerCallContext context)
    {
        _logger.LogInformation( $"gRPC Service: {nameof(ClientsService)} - {nameof(EditClientGrpc)}. Sending:{request}");

         var result = await _mediator.Send(new EditClientCommand(request.Id.ToGuid(),request.Name,request.Industry));
         
         _logger.LogInformation( $"gRPC Service: {nameof(ClientsService)} - {nameof(EditClientGrpc)}. Received:{result}");

         return new ClientGrpcCommandResult
         {
             Id = result.ToString(),
         };
    }

    /// <summary>
    /// Get Client by Id
    /// 'gRPC' implementation
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<GetListClientGrpcResult> GetClientByIdGrpc(GetClientByIdGrpcQuery request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetClientByIdQuery(request.Id.ToGuid()));

        var data = new GetListClientGrpcResult();
        data.Result.AddRange(result.Select(r => ToClientResult(r)));

        return data;
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

        return new ClientGrpcCommandResult { Id = guid.ToString() };

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
    /// Get member information using MemberId and Uid. This to be used when credentials are pre-generated as dummy person objects in the database.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<GetMemberWithUidGrpcResult> GetMemberByIdAndUidGrpc(GetMemberByIdAndUidGrpcQuery request, ServerCallContext context)
    {
        var response = await _mediator.Send(new GetMemberByIdAndUidQuery(request.ClientId.ToGuid(),  request.MemberId.ToGuid(), request.Uid));


        var result = new GetMemberWithUidGrpcResult();

        if (response.Member == null)
        {
            result.Members.AddRange(Array.Empty<MemberGrpcResult>());
            return result;
        }
        
        result.Members.AddRange(new [] {
            new MemberGrpcResult()
            {
                Address = response.Member.FirstOrDefault()?.Address,
                ClientId = response.Member.FirstOrDefault()?.ClientId.ToString(),
                SubscriptionId = response.Member.FirstOrDefault()?.SubscriptionId.ToString(),
                CardLevel = response.Member.FirstOrDefault().CardLevel,
                FullName = NameBuilder(
                    response.Member.FirstOrDefault()?.FirstName,
                    response.Member.FirstOrDefault()?.LastName,
                    response.Member.FirstOrDefault()?.MiddleName,
                    response.Member.FirstOrDefault()?.NameSuffix),
                FirstName = response.Member.FirstOrDefault()?.FirstName,
                LastName = response.Member.FirstOrDefault()?.LastName,
                MiddleName = response.Member.FirstOrDefault()?.MiddleName,
                NameSuffix = response.Member.FirstOrDefault()?.NameSuffix,
                Email = response.Member.FirstOrDefault()?.Email,
                Facebook = response.Member.FirstOrDefault()?.Facebook,
                MemberId = response.Member.FirstOrDefault()?.MemberId.ToString(),
                Instagram = response.Member.FirstOrDefault()?.Instagram,
                Occupation = response.Member.FirstOrDefault()?.Occupation,
                Pinterest = response.Member.FirstOrDefault()?.Pinterest,
                Twitter = response.Member.FirstOrDefault()?.Twitter,
                LinkedIn = response.Member.FirstOrDefault()?.LinkedIn,
                CardKey = response.Member.FirstOrDefault()?.CardKey,
                PhoneNumber = response.Member.FirstOrDefault()?.PhoneNumber,
                Identity = response.Member.FirstOrDefault()?.IdentityUserId,
                Company = response.Member.FirstOrDefault()?.Company,
                CardId = Guid.Empty.ToString()
            }
        });

        return result;
    }

    

    #region Transform
    
    private static SocialMediaObject? GetSocialMedia(MembersResult member)
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
            Id = c.Id.ToString(),
            Industry = c.Industry,
            Name = c.Name,
            Subscriptions = c.Subscriptions,
            IsActive = c.IsActive
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
            SubscriptionId = c.Subscription,
            CardLevel = 0,
            FullName = NameBuilder(c),
            Email = c.Email,
            Facebook = GetSocialMedia(c)?.Facebook ?? "N/A",
            MemberId = c.Id.ToString(),
            Instagram = GetSocialMedia(c)?.Instagram ?? "N/A",
            Occupation = c.Occupation,
            Pinterest =  GetSocialMedia(c)?.Pinterest ?? "N/A",
            Twitter =  GetSocialMedia(c)?.Twitter ?? "N/A",
            LinkedIn =  GetSocialMedia(c)?.LinkedIn ?? "N/A",
            CardKey = c.CardKey,
            PhoneNumber = c.PhoneNumber,
        };
    }
    
   


    private static AddMemberCommand ToAddMemberCommand(AddMemberGrpcCommand request)
    {
        return new AddMemberCommand(request.ClientId.ToGuid(), request.FirstName, request.LastName,
            request.MiddleName, request.NameSuffix, request.PhoneNumber, request.Email, request.Address,
            request.Occupation, request.Facebook, request.LinkedIn, request.Instagram, request.Pinterest,
            request.Twitter, request.SubscriptionId.ToGuid());
    }
    
    private static AddMemberWithIdentityKeyCommand ToAddMemberWithIdentityKeyCommand(AddMemberWithIdentityGrpcCommand request)
    {
        return new AddMemberWithIdentityKeyCommand(
            request.ClientId.ToGuid(), 
            request.FirstName, 
            request.LastName,
            request.MiddleName,
            request.NameSuffix,
            request.PhoneNumber, 
            request.Email, 
            request.Address,
            request.Occupation,
            request.Identity,
            request.CardKey, 
            request.SubscriptionId.ToGuid(), 
            new SocialMediaObject
            {
                Facebook = request.Facebook,
                Instagram = request.Instagram,
                LinkedIn = request.LinkedIn,
                Pinterest = request.Pinterest,
                Twitter = request.Twitter
            });
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