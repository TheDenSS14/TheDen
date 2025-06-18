// SPDX-FileCopyrightText: 2025 Eris <eris@erisws.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared._Goobstation.Clothing.Systems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Clothing.Components;

/// Defines the clothing entity that can be sealed by <see cref="SealableClothingControlComponent"/>
[RegisterComponent]
[NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedSealableClothingSystem))]
public sealed partial class SealableClothingComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool IsSealed = false;

    [DataField, AutoNetworkedField]
    public TimeSpan SealingTime = TimeSpan.FromSeconds(1.75);

    [DataField]
    public LocId SealUpPopup = "sealable-clothing-seal-up";

    [DataField]
    public LocId SealDownPopup = "sealable-clothing-seal-down";

    [DataField]
    public SoundSpecifier SealUpSound = new SoundPathSpecifier("/Audio/Mecha/mechmove03.ogg");

    [DataField]
    public SoundSpecifier SealDownSound = new SoundPathSpecifier("/Audio/Mecha/mechmove03.ogg");
}
