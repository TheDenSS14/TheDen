// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 WarMechanic <69510347+WarMechanic@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Strip.Components;

/// <summary>
/// Give this to an entity when you want to decrease stripping times
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ThievingComponent : Component
{
    /// <summary>
    /// How much the strip time should be shortened by
    /// </summary>
    [DataField]
    public TimeSpan StripTimeReduction = TimeSpan.FromSeconds(0.5f);

    /// <summary>
    ///  A multiplier coefficient for strip time
    /// </summary>
    [DataField]
    public float StripTimeMultiplier = 1f;

    /// <summary>
    /// Should it notify the user if they're stripping a pocket?
    /// </summary>
    [DataField]
    public ThievingStealth Stealth = ThievingStealth.Hidden;

    /// <summary>
    ///  Should the user be able to see hidden items? (i.e pockets)
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool IgnoreStripHidden;
}
