// SPDX-FileCopyrightText: 2023 AJCM-git <60196617+AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.Ranged.Components;

/// <summary>
///     Handles pulling entities from the given container to use as ammunition.
/// </summary>
[RegisterComponent]
public sealed partial class ContainerAmmoProviderComponent : AmmoProviderComponent
{
    [DataField("container", required: true)]
    [ViewVariables]
    public string Container = default!;

    [DataField("provider")]
    [ViewVariables]
    public EntityUid? ProviderUid;
}
