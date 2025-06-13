using Content.Shared.Mobs;
using System.Diagnostics.CodeAnalysis;

namespace Content.Shared.Silicons.Bots;

/// <summary>
/// Handles emagging medibots and provides api.
/// </summary>
public abstract class SharedMedibotSystem : EntitySystem
{
    /// <summary>
    /// Get a treatment for a given mob state.
    /// </summary>
    /// <remarks>
    /// This only exists because allowing other execute would allow modifying the dictionary, and Read access does not cover TryGetValue.
    /// </remarks>
    public bool TryGetTreatment(MedibotComponent comp, MobState state, [NotNullWhen(true)] out MedibotTreatment? treatment)
    {
        return comp.Treatments.TryGetValue(state, out treatment);
    }
}
