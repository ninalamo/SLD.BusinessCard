namespace BusinessCard.Application.Application.Common.Models;

public record CardResult
{
    public Guid Id { get; init; }
    public string Uid { get; init; }
    public Guid MemberId { get; init; }
    public Guid SubscriptionId { get; init; }
    public Guid ClientId { get; init; }
}