using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{

    /// <summary>
    ///     How many Low Danger rounds should there be after a High Danger round?
    /// </summary>
    public static readonly CVarDef<int> LowDangerRoundsAfterHighDanger =
        CVarDef.Create("game.low_danger_rounds_after_high_danger", 1, CVar.SERVER | CVar.ARCHIVE);
}
