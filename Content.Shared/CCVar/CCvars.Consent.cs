using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// <summary>
    /// How many characters the consent text can be. // Floofstation
    /// </summary>
    public static readonly CVarDef<int> ConsentFreetextMaxLength =
        CVarDef.Create("consent.freetext_max_length", 1000, CVar.REPLICATED | CVar.SERVER);
}