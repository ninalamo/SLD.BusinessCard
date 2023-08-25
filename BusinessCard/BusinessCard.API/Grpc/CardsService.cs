using BusinessCard.API.Application.Commands.ExportCards;
using BusinessCard.API.Extensions;
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
}