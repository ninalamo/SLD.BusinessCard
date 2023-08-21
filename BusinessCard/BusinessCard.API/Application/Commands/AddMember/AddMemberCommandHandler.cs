using BusinessCard.API.Application.Commands.UpsertClient;
using BusinessCard.API.Logging;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using MediatR;

namespace BusinessCard.API.Application.Commands.AddMember;

public class AddMemberCommandHandler : IRequestHandler<AddMemberCommand, Guid>
{
    private readonly IClientsRepository _repository;
    private readonly ILoggerAdapter<AddClientCommandHandler> _logger;

    public AddMemberCommandHandler(IClientsRepository repository, ILoggerAdapter<AddClientCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Guid> Handle(AddMemberCommand request, CancellationToken cancellationToken)
    {
        var client = await _repository.GetWithPropertiesByIdAsync(request.ClientId);

        var person = await client.AddMember(request.FirstName, request.LastName, request.MiddleName, request.NameSuffix, request.PhoneNumber,request.Email,request.Address,request.Occupation,request.SocialMedia);

        _repository.Update(client);

        return person.Id;
    }
}