// SPDX-FileCopyrightText: 2024 Fansana <116083121+Fansana@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared.Floofstation.Leash.Components;

/// <summary>
///     Draws a line between this entity and the target. Same as JointVisualsComponent.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class LeashedVisualsComponent : Component
{
    [DataField(required: true), AutoNetworkedField]
    public SpriteSpecifier Sprite = default!;

    [DataField, AutoNetworkedField]
    public EntityUid Source, Target;

    [DataField, AutoNetworkedField]
    public Vector2 OffsetSource, OffsetTarget;
}
