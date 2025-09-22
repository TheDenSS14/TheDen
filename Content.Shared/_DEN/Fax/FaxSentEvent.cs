using Robust.Shared.Serialization;


namespace Content.Shared._DEN.Fax;

public sealed class FaxSentEvent
{
    public string Content;
    public string? DestinationAddress;
    public List<string> StampedBy;

    public FaxSentEvent(string content, string? destinationAddress, List<string> stampedBy)
    {
        Content = content;
        DestinationAddress = destinationAddress;
        StampedBy = stampedBy;
    }
}
