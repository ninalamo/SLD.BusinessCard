using BusinessCard.Application.Application.Queries.GetClients;
using FluentValidation;

namespace BusinessCard.Application.Application.Queries.GetClientById;

public class GetClientByIdQuery : IRequest<IEnumerable<ClientsResult>>
{
    public GetClientByIdQuery(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; private set; }
}

public class GetClientByIdQueryValidator : AbstractValidator<GetClientByIdQuery>
{
    public GetClientByIdQueryValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}