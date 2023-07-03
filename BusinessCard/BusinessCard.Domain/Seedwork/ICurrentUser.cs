namespace BusinessCard.Domain.Seedwork
{
    public interface ICurrentUser
    {
        string Name { get; }
        string Email { get; }
        string IdentityId { get; }
        string[] Roles { get; }


        bool IsAdmin();
    }
}
