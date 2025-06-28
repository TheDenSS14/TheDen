// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Menshin <Menshin@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Rosycup <178287475+Rosycup@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Alert;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Clothing;

[RegisterComponent, NetworkedComponent]
public sealed partial class MagbootsComponent : Component
{
    [DataField]
    public ProtoId<AlertPrototype> MagbootsAlert = "Magboots";

    /// <summary>
    /// If true, the user must be standing on a grid or planet map to experience the weightlessness-canceling effect
    /// </summary>
    [DataField]
    public bool RequiresGrid = true;

    /// <summary>
    /// Slot the clothing has to be worn in to work.
    /// </summary>
    [DataField]
    public string Slot = "shoes";

    /// <summary>
    ///     Whether or not activating the magboots changes a sprite.
    /// </summary>
    [DataField]
    public bool ChangeClothingVisuals;

    /// <summary>
    ///     Whether or not the magboots are currently Active.
    /// </summary>
    [DataField]
    public bool Active;

    /// <summary>
    ///     Walk speed modifier to use while the magnets are active.
    /// </summary>
    [DataField]
    public float ActiveWalkModifier = 0.85f;

    /// <summary>
    ///     Sprint speed modifier to use while the magnets are active.
    /// </summary>
    [DataField]
    public float ActiveSprintModifier = 0.80f;

    /// <summary>
    ///     Walk speed modifier to use while the magnets are off.
    /// </summary>
    [DataField]
    public float InactiveWalkModifier = 1f;

    /// <summary>
    ///     Sprint speed modifier to use while the magnets are off.
    /// </summary>
    [DataField]
    public float InactiveSprintModifier = 1f;
}
