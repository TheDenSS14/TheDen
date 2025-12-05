// SPDX-FileCopyrightText: 2025 Tanix
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._RMC14.Emote;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class RecentlyEmotedComponent : Component
{
    [DataField, AutoNetworkedField]
    public TimeSpan Cooldown = TimeSpan.FromSeconds(0.8);

    [DataField, AutoNetworkedField]
    public TimeSpan NextEmote = TimeSpan.Zero;
}
