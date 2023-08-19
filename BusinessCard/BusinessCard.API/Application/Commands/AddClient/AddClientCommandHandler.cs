using BusinessCard.API.Logging;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using MediatR;

namespace BusinessCard.API.Application.Commands.UpsertClient;

public class AddClientCommandHandler : IRequestHandler<AddClientCommand, CommandResult>
{
    private readonly IClientsRepository _repository;
    private readonly ILoggerAdapter<AddClientCommandHandler> _logger;

    public AddClientCommandHandler(IClientsRepository repository, ILoggerAdapter<AddClientCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(AddClientCommand request, CancellationToken cancellationToken)
    {
        var tier = MemberTier.GetLevels().First(i => i.Level == request.MemberTierLevel).Id;
        var id = _repository.Create(request.CompanyName, request.IsDiscreet,tier).Id;
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return CommandResult.Success(id);
    }
}