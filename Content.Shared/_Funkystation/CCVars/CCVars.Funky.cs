using Robust.Shared.Configuration;

namespace Content.Shared._Funkystation.CCVars;

[CVarDefs]
public sealed class CCVars_Funky
{
    /// <summary>
    ///     Is bluespace gas enabled.
    /// </summary>
    public static readonly CVarDef<bool> BluespaceGasEnabled =
        CVarDef.Create("funky.bluespace_gas_enabled", true, CVar.SERVER | CVar.REPLICATED);
}
