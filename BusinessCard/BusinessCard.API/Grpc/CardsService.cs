using BusinessCard.API.Application.Commands.ExportCards;
using BusinessCard.API.Application.Queries.GetMembers;
using BusinessCard.API.Extensions;
using ClientService;
using FluentValidation;
using FluentValidation.Results;
using Grpc.Core;
using KardService;
using MediatR;

namespace BusinessCard.API.Grpc;

public class KardsService : KardGrpc.KardGrpcBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<KardsService> _logger;

    public KardsService(IMediator mediator, ILogger<KardsService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    
    public override async Task<ExportCardsGrpcCommandResult> ExportCardsGrpc(ExportCardsGrpcCommand request, ServerCallContext context)
    {
        var response = await _mediator.Send(new ExportCardsCommand(request.ClientId.ToGuid(), request.Count));
    
        var result = new ExportCardsGrpcCommandResult();
        result.Urls.AddRange(response.Urls);
    
        return result;
    }

    public override async Task<DownloadUrlsGrpcCommandResult> DownloadUrlsGrpc(DownloadUrlsGrpcCommand request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.ClientId, out var guid))
            throw new ValidationException("Validation Error",
                new[] { new ValidationFailure("Id", "Id is not a Guid.") });

        var data = await _mediator.Send(new GetMembersQuery(int.MaxValue,1,request.ClientId.ToGuid()));

        var response = new DownloadUrlsGrpcCommandResult();
        response.Urls.AddRange(data.Members
            .Where(p => !p.IsActive && p.CardKey == "")
            .Select(p => p.Id.ToString())
            .ToArray());

        return response;

    }
}