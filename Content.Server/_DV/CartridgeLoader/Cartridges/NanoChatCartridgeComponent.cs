// SPDX-FileCopyrightText: 2024 Milon <milonpl.git@proton.me>
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy <73762869+BlitzTheSquishy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Radio;
using Robust.Shared.Prototypes;

namespace Content.Server._DV.CartridgeLoader.Cartridges;

[RegisterComponent, Access(typeof(NanoChatCartridgeSystem))]
public sealed partial class NanoChatCartridgeComponent : Component
{
    /// <summary>
    ///     Station entity to keep track of.
    /// </summary>
    [DataField]
    public EntityUid? Station;

    /// <summary>
    ///     The NanoChat card to keep track of.
    /// </summary>
    [DataField]
    public EntityUid? Card;

    /// <summary>
    ///     The <see cref="RadioChannelPrototype" /> required to send or receive messages.
    /// </summary>
    [DataField]
    public ProtoId<RadioChannelPrototype> RadioChannel = "Common";
}
