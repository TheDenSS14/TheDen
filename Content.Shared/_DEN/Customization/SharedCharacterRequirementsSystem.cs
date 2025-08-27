using System.Diagnostics.CodeAnalysis;
using Content.Shared.Humanoid;
using Content.Shared.Preferences;

namespace Content.Shared.Customization.Systems;

public abstract partial class SharedCharacterRequirementsSystem
{
    protected bool TryGetProfile(EntityUid uid, [NotNullWhen(true)] out HumanoidCharacterProfile? profile)
    {
        profile = null;

        if (!TryComp<HumanoidAppearanceComponent>(uid, out var humanoid))
            return false;

        profile = humanoid.LastProfileLoaded;
        return profile != null;
    }
}
