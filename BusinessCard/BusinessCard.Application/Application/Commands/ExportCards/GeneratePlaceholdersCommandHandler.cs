using BusinessCard.Application.Application.Commands.RemoveClient;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessCard.Application.Application.Commands.ExportCards;

public class GeneratePlaceholdersCommandHandler : IRequestHandler<GeneratePlaceholdersCommand, GeneratePlaceholdersCommandResult>
{
    private readonly IClientsRepository _repository;
    private readonly ILogger<GeneratePlaceholdersCommandHandler> _logger;

    public GeneratePlaceholdersCommandHandler(IClientsRepository repository, ILogger<GeneratePlaceholdersCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }


    public async Task<GeneratePlaceholdersCommandResult> Handle(GeneratePlaceholdersCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting {nameof(Handle)} in {nameof(RemoveClientCommandHandler)} with request: {request}. {DateTime.UtcNow}");

        var client = await _repository.GetWithPropertiesByIdAsync(request.ClientId);

        _logger.LogInformation($"Checking if {nameof(request.ClientId)} exists. {DateTime.UtcNow}");

        if (client == null)
        {
            _logger.LogInformation($"{nameof(request.ClientId)} does not exists. {DateTime.UtcNow}");
            throw new ValidationException("Validation error.",
                new ValidationFailure[] { new ValidationFailure("Id", "Client not found.") });
        }


        var subscription = client.Subscriptions.FirstOrDefault(i => i.Id == request.SubscriptionId);
        
        
      
        var guid = Guid.NewGuid().ToString();

        for (var i = 0; i < request.Count; i++)
        {
            var person = new Person();
            // entity.AddMember(
            // "N/A", 
            // "N/A", 
            // "N/A",
            // "N/A",
            // Guid.NewGuid().ToString(),
            // $"{Guid.NewGuid().ToString()}@tuldok.co", 
            // "N/A",
            // "N/A",
            // "{\n  \"Facebook\": \"N/A\",\n  \"LinkedIn\": \"N/A\",\n  \"Pinterest\": \"N/A\",\n  \"Instagram\": \"N/A\",\n  \"Twitter\": \"N/A\"\n}");

            //person.SetSubscription(entity.MembershipTier.Level);
            // person.DisableCard();
            person.Deactivate();
            
            _repository.Update(client);

            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new GeneratePlaceholdersCommandResult()
        {
            Urls = Array.Empty<string>()//TODO: Refactor entity.Persons.Where(p => p.IsActive == false).Select(p => $"ext/v1/tenants/{entity.Id}/members/{p.Id}").ToArray()
        };
    }
}