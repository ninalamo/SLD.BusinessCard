using BusinessCard.Application.Application.Commands.ExportCards;
using BusinessCard.Application.Application.Queries.GetMembers;
using BusinessCard.Application.Extensions;
using FluentValidation.Results;
using Grpc.Core;
using CardService;

namespace BusinessCard.GrpcServices.Services;

internal class KardsService : KardGrpc.KardGrpcBase
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
        var response = await _mediator.Send(
            new GeneratePlaceholdersCommand(request.ClientId.ToGuid(), request.Count, request.SubscriptionId.ToGuid()));
    
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
            .Select(p => $"ext/v1/tenants/{p.ClientId}/members/{p.Id}")
            .ToArray());

        return response;

    }
}