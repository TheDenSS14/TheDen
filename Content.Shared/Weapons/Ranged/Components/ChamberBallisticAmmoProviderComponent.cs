// SPDX-FileCopyrightText: 2022 Kara
// SPDX-FileCopyrightText: 2022 metalgearsloth
// SPDX-FileCopyrightText: 2023 TaralGit
// SPDX-FileCopyrightText: 2023 and_a
// SPDX-FileCopyrightText: 2024 BramvanZijp
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.Audio;

namespace Content.Shared.Weapons.Ranged.Components;

/// <summary>
/// Chamber + internal mags in one package. If you need just magazine then use <see cref="BallisticAmmoProviderComponent"/>
/// </summary>

[RegisterComponent, AutoGenerateComponentState]
public sealed partial class ChamberBallisticAmmoProviderComponent : BallisticAmmoProviderComponent
{
    /// <summary>
    /// If the gun has a bolt and whether that bolt is closed. Firing is impossible
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("boltClosed"), AutoNetworkedField]
    public bool? BoltClosed = false;

    /// <summary>
    /// Does the gun automatically open and close the bolt upon shooting.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("autoCycle"), AutoNetworkedField]
    public bool AutoCycle = true;

    /// <summary>
    /// Can the gun be racked, which opens and then instantly closes the bolt to cycle a round.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("canRack"), AutoNetworkedField]
    public bool CanRack = true;

    /// <summary>
    ///  Some weapons such as bolt actions would have their internal magazines blocked, whiles ones such as shotguns, would not.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("reloadWhenBolted"), AutoNetworkedField]
    public bool ReloadWhenBolted = true;

    [ViewVariables(VVAccess.ReadWrite), DataField("soundBoltClosed"), AutoNetworkedField]
    public SoundSpecifier? BoltClosedSound = new SoundPathSpecifier("/Audio/Weapons/Guns/Bolt/rifle_bolt_closed.ogg");

    [ViewVariables(VVAccess.ReadWrite), DataField("soundBoltOpened"), AutoNetworkedField]
    public SoundSpecifier? BoltOpenedSound = new SoundPathSpecifier("/Audio/Weapons/Guns/Bolt/rifle_bolt_open.ogg");
}
