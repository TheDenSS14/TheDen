// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2024 deltanedas
// SPDX-FileCopyrightText: 2025 Winter
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Actions;
using Robust.Shared.Serialization;

namespace Content.Shared.Toggleable;

/// <summary>
/// Generic action-event for toggle-able components.
/// </summary>
/// <remarks>
/// If you are using <c>ItemToggleComponent</c> subscribe to <c>ItemToggledEvent</c> instead.
/// </remarks>
public sealed partial class ToggleActionEvent : InstantActionEvent;

/// <summary>
///     Generic enum keys for toggle-visualizer appearance data & sprite layers.
/// </summary>
[Serializable, NetSerializable]
public enum ToggleableVisuals : byte
{
    Enabled,
    Layer,
    Color,
}

/// <summary>
///     Generic sprite layer keys.
/// </summary>
[Serializable, NetSerializable]
public enum LightLayers : byte
{
    Light,

    /// <summary>
    ///     Used as a key for generic unshaded layers. Not necessarily related to an entity with an actual light source.
    ///     Use this instead of creating a unique single-purpose "unshaded" enum for every visualizer.
    /// </summary>
    Unshaded,
}
