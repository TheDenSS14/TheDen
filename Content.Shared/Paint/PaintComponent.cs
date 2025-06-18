// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Content.Shared.Whitelist;
using Robust.Shared.Prototypes;
using Robust.Shared.GameStates;

namespace Content.Shared.Paint;

/// Entity when used on another entity will paint target entity
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedPaintSystem))]
public sealed partial class PaintComponent : Component
{
    /// Noise made when paint gets applied
    [DataField]
    public SoundSpecifier Spray = new SoundPathSpecifier("/Audio/Effects/spray2.ogg");

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntityWhitelist? Whitelist;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntityWhitelist? Blacklist;

    /// How long the doafter will take
    [DataField]
    public int Delay = 2;

    [DataField, AutoNetworkedField]
    public Color Color = Color.FromHex("#c62121");

    /// Solution on the entity that contains the paint
    [DataField]
    public string Solution = "drink";

    /// Reagent that will be used as paint
    [DataField, AutoNetworkedField]
    public ProtoId<ReagentPrototype> Reagent = "SpaceGlue";

    /// Reagent consumption per use
    [DataField]
    public FixedPoint2 ConsumptionUnit = FixedPoint2.New(5);

    [DataField]
    public TimeSpan DurationPerUnit = TimeSpan.FromSeconds(6);
}
