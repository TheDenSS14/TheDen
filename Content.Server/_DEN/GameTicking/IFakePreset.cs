namespace Content.Server._DEN.GameTicking.Rules;

public interface IFakePreset
{
    public HashSet<EntityUid> AdditionalGameRules { get; set; }
}

