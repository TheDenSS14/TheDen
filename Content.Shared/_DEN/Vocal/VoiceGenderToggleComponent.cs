using Content.Shared.Actions;
using Content.Shared.Humanoid;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared._DEN.Vocal;

[RegisterComponent]
public sealed partial class VoiceGenderToggleComponent : Component
{
    [DataField]
    public EntProtoId ToggleAction = "ActionToggleVoiceGender";

    [ViewVariables(VVAccess.ReadOnly)]
    public EntityUid? ToggleActionEntity;

    [ViewVariables(VVAccess.ReadOnly)]
    public Sex CurrentVoiceGender;
}

public sealed partial class ToggleVoiceGenderEvent : InstantActionEvent;
