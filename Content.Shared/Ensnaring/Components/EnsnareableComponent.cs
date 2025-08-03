// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Alert;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Ensnaring.Components;
/// <summary>
/// Use this on an entity that you would like to be ensnared by anything that has the <see cref="EnsnaringComponent"/>
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class EnsnareableComponent : Component
{
    /// <summary>
    /// How much should this slow down the entities walk?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("walkSpeed")]
    public float WalkSpeed = 1.0f;

    /// <summary>
    /// How much should this slow down the entities sprint?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("sprintSpeed")]
    public float SprintSpeed = 1.0f;

    /// <summary>
    /// Is this entity currently ensnared?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("isEnsnared")]
    public bool IsEnsnared;

    /// <summary>
    /// The container where the <see cref="EnsnaringComponent"/> entity will be stored
    /// </summary>
    public Container Container = default!;

    [DataField("sprite")]
    public string? Sprite;

    [DataField("state")]
    public string? State;

    [DataField]
    public ProtoId<AlertPrototype> EnsnaredAlert = "Ensnared";
}

[Serializable, NetSerializable]
public sealed class EnsnareableComponentState : ComponentState
{
    public readonly bool IsEnsnared;

    public EnsnareableComponentState(bool isEnsnared)
    {
        IsEnsnared = isEnsnared;
    }
}

public sealed class EnsnaredChangedEvent : EntityEventArgs
{
    public readonly bool IsEnsnared;

    public EnsnaredChangedEvent(bool isEnsnared)
    {
        IsEnsnared = isEnsnared;
    }
}
