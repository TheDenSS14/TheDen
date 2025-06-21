// SPDX-FileCopyrightText: 2025 M3739 <47579354+M3739@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Tabitha <64847293+KyuPolaris@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Polymorph;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._DV.Abilities.Kitsune;

/// <summary>
/// This component assigns the entity with a polymorph action
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class KitsuneComponent : Component
{
    [DataField] public ProtoId<PolymorphPrototype> KitsunePolymorphId = "KitsuneMorph";

    [DataField] public EntProtoId KitsuneAction = "ActionKitsuneMorph";

    [DataField, AutoNetworkedField] public EntityUid? KitsuneActionEntity;

    /// <summary>
    /// The foxfire prototype to use.
    /// </summary>
    [DataField] public EntProtoId FoxfirePrototype = "Foxfire";

    [DataField] public EntProtoId FoxfireActionId = "ActionFoxfire";

    [DataField, AutoNetworkedField] public EntityUid? FoxfireAction;

    [DataField, AutoNetworkedField] public List<EntityUid> ActiveFoxFires = [];

    [DataField, AutoNetworkedField] public Color? Color;

    /// <summary>
    /// Represents a light coming from a light source.
    /// As such it has its value maximised while not touching hue or saturation.
    /// </summary>
    [DataField, AutoNetworkedField] public Color? ColorLight;
}

[Serializable, NetSerializable]
public enum KitsuneColorVisuals : byte
{
    Color,
    Layer
}
