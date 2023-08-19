using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BusinessCard.API.Application.Commands.UpsertClient;

public class AddClientCommand : IRequest<CommandResult>
{
    public AddClientCommand(string companyName, bool isDiscreet, int memberTierLevel)
    {
        CompanyName = companyName;
        IsDiscreet = isDiscreet;
        MemberTierLevel = memberTierLevel;
    }
    public string CompanyName { get; private set; }
    public int MemberTierLevel { get; private set; }
    public bool IsDiscreet { get; private set; }
}