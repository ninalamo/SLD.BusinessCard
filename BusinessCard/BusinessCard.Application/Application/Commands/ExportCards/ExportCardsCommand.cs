using BusinessCard.Application.Application.Commands.RemoveClient;
using BusinessCard.Domain.AggregatesModel.ClientAggregate;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessCard.Application.Application.Commands.ExportCards;

public class ExportCardsCommand : IRequest<ExportCardsCommandResult>
{
    public ExportCardsCommand(Guid clientId, int count)
    {
        ClientId = clientId;
        Count = count;
    }
    
    public Guid ClientId { get; private set; }
    public int Count { get; private set; }
}

public class ExportCardsCommmandValidator : AbstractValidator<ExportCardsCommand>
{
    public ExportCardsCommmandValidator()
    {
        RuleFor(i => i.ClientId).NotEmpty();
        RuleFor(i => i.Count).Must(x => x <= 1000 && x > 0).WithMessage("Must be between 0 and 1000 only.");
    }
}

public class ExportCardsCommmandHandler : IRequestHandler<ExportCardsCommand, ExportCardsCommandResult>
{
    private readonly IClientsRepository _repository;
    private readonly ILogger<ExportCardsCommmandHandler> _logger;

    public ExportCardsCommmandHandler(IClientsRepository repository, ILogger<ExportCardsCommmandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }


    public async Task<ExportCardsCommandResult> Handle(ExportCardsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting {nameof(Handle)} in {nameof(RemoveClientCommandHandler)} with request: {request}. {DateTime.UtcNow}");

        var entity = await _repository.GetWithPropertiesByIdAsync(request.ClientId);

        _logger.LogInformation($"Checking if {nameof(request.ClientId)} exists. {DateTime.UtcNow}");

        if (entity == null)
        {
            _logger.LogInformation($"{nameof(request.ClientId)} does not exists. {DateTime.UtcNow}");
            throw new ValidationException("Validation error.",
                new ValidationFailure[] { new ValidationFailure("Id", "Client not found.") });
        }

      
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
            
            _repository.Update(entity);

            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new ExportCardsCommandResult()
        {
            Urls = Array.Empty<string>()//TODO: Refactor entity.Persons.Where(p => p.IsActive == false).Select(p => $"ext/v1/tenants/{entity.Id}/members/{p.Id}").ToArray()
        };
    }
} 


public record ExportCardsCommandResult
{
    public IEnumerable<string> Urls { get; init; }
}