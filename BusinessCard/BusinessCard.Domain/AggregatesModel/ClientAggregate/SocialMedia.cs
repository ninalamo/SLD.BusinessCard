using BusinessCard.Domain.Seedwork;

namespace BusinessCard.Domain.AggregatesModel.ClientAggregate;

public class SocialMedia : ValueObject
{
    public SocialMedia(string facebook, string instagram, string linkedIn, string pinterest, string twitter)
    {
        Facebook = facebook;
        Instagram = instagram;
        LinkedIn = linkedIn;
        Pinterest = pinterest;
        Twitter = twitter;
    }
    
    public string Facebook { get; }
    public string Instagram { get; }
    public string LinkedIn { get; }
    public string Pinterest { get; }
    public string Twitter { get; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Facebook;
        yield return Instagram;
        yield return LinkedIn;
        yield return Pinterest;
        yield return Twitter;
    }
}