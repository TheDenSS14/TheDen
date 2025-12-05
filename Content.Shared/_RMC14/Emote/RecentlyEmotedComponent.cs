using Content.Shared.Chat.Prototypes;
using Robust.Shared.Prototypes;


namespace Content.Shared._RMC14.Emote;

[RegisterComponent]
public sealed partial class RecentlyEmotedComponent : Component
{
    [DataField]
    public TimeSpan Cooldown = TimeSpan.FromSeconds(0.8);

    [DataField]
    public TimeSpan NextEmote = TimeSpan.Zero;
}
