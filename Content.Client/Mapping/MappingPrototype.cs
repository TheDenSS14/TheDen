// SPDX-FileCopyrightText: 2024 DrSmugleaf <10968691+DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 poemota <142114334+poeMota@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Decals;
using Content.Shared.Maps;
using Robust.Shared.Prototypes;

namespace Content.Client.Mapping;

/// <summary>
///     Used to represent a button's data in the mapping editor.
/// </summary>
public sealed class MappingPrototype
{
    /// <summary>
    ///     The prototype instance, if any.
    ///     Can be one of <see cref="EntityPrototype"/>, <see cref="ContentTileDefinition"/> or <see cref="DecalPrototype"/>
    ///     If null, this is a top-level button (such as Entities, Tiles or Decals)
    /// </summary>
    public readonly IPrototype? Prototype;

    /// <summary>
    ///     The text to display on the UI for this button.
    /// </summary>
    public readonly string Name;

    /// <summary>
    ///     Whether the prototype is in the “Favorites” list.
    /// </summary>
    public bool Favorite;

    /// <summary>
    ///     Which other prototypes (buttons) this one is nested inside of.
    /// </summary>
    public List<MappingPrototype>? Parents;

    /// <summary>
    ///     Which other prototypes (buttons) are nested inside this one.
    /// </summary>
    public List<MappingPrototype>? Children;

    public MappingPrototype(IPrototype? prototype, string name)
    {
        Prototype = prototype;
        Name = name;
    }
}
