// SPDX-FileCopyrightText: 2024 Remuchi <72476615+Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Rosycup <178287475+Rosycup@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.DoAfter;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.WhiteDream.BloodCult.Runes;

[RegisterComponent, NetworkedComponent]
public sealed partial class RuneDrawerComponent : Component
{
    [DataField]
    public float EraseTime = 4f;

    [DataField]
    public SoundSpecifier StartDrawingSound = new SoundPathSpecifier("/Audio/_White/BloodCult/butcher.ogg");

    public SoundSpecifier EndDrawingSound = new SoundPathSpecifier("/Audio/_White/BloodCult/blood.ogg");
}

[Serializable, NetSerializable]
public enum RuneDrawerBuiKey
{
    Key
}

[Serializable, NetSerializable]
public sealed class RuneDrawerMenuState(List<ProtoId<RuneSelectorPrototype>> availalbeRunes) : BoundUserInterfaceState
{
    public List<ProtoId<RuneSelectorPrototype>> AvailalbeRunes { get; private set; } = availalbeRunes;
}

[Serializable, NetSerializable]
public sealed class RuneDrawerSelectedMessage(ProtoId<RuneSelectorPrototype> selectedRune) : BoundUserInterfaceMessage
{
    public ProtoId<RuneSelectorPrototype> SelectedRune { get; private set; } = selectedRune;
}

[Serializable, NetSerializable]
public sealed partial class RuneEraseDoAfterEvent : SimpleDoAfterEvent;

[Serializable, NetSerializable]
public sealed partial class DrawRuneDoAfter : SimpleDoAfterEvent
{
    public ProtoId<RuneSelectorPrototype> Rune;
    public SoundSpecifier EndDrawingSound;
}
