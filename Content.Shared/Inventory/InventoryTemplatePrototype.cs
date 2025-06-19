// SPDX-FileCopyrightText: 2022 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 DrSmugleaf <10968691+DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <159397573+gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Numerics;
using Content.Shared.Strip;
using Content.Shared.Whitelist;
using Robust.Shared.Prototypes;

namespace Content.Shared.Inventory;

[Prototype("inventoryTemplate")]
public sealed partial class InventoryTemplatePrototype : IPrototype
{
    [IdDataField] public string ID { get; } = string.Empty;

    [DataField("slots")] public SlotDefinition[] Slots { get; private set; } = Array.Empty<SlotDefinition>();
}

[DataDefinition]
public sealed partial class SlotDefinition
{
    [DataField("name", required: true)] public string Name { get; private set; } = string.Empty;
    [DataField("slotTexture")] public string TextureName { get; private set; } = "pocket";
    /// <summary>
    /// The texture displayed in a slot when it has an item inside of it.
    /// </summary>
    [DataField] public string FullTextureName { get; private set; } = "SlotBackground";
    [DataField("slotFlags")] public SlotFlags SlotFlags { get; private set; } = SlotFlags.PREVENTEQUIP;
    [DataField("showInWindow")] public bool ShowInWindow { get; private set; } = true;
    [DataField("slotGroup")] public string SlotGroup { get; private set; } = "Default";
    [DataField("stripTime")] public float StripTime { get; private set; } = 4f;

    [DataField("uiWindowPos", required: true)]
    public Vector2i UIWindowPosition { get; private set; }

    [DataField("strippingWindowPos", required: true)]
    public Vector2i StrippingWindowPos { get; private set; }

    [DataField("dependsOn")] public string? DependsOn { get; private set; }

    [DataField("dependsOnComponents")] public ComponentRegistry? DependsOnComponents { get; private set; }

    [DataField("displayName", required: true)]
    public string DisplayName { get; private set; } = string.Empty;

    /// <summary>
    ///     Whether or not this slot will have its item hidden in the strip menu, and block interactions.
    ///     <seealso cref="SharedStrippableSystem.IsStripHidden"/>
    /// </summary>
    [DataField("stripHidden")] public bool StripHidden { get; private set; }

    /// <summary>
    ///     Offset for the clothing sprites.
    /// </summary>
    [DataField("offset")] public Vector2 Offset { get; private set; } = Vector2.Zero;

    /// <summary>
    ///     Entity whitelist for CanEquip checks.
    /// </summary>
    [DataField("whitelist")] public EntityWhitelist? Whitelist = null;

    /// <summary>
    ///     Entity blacklist for CanEquip checks.
    /// </summary>
    [DataField("blacklist")] public EntityWhitelist? Blacklist = null;

    /// <summary>
    ///     Entity whitelist for CanEquip checks, on spawn only.
    /// </summary>
    [DataField("spawnWhitelist")] public EntityWhitelist? SpawnWhitelist = null;

    /// <summary>
    ///     Entity blacklist for CanEquip checks, on spawn only.
    /// </summary>
    [DataField("spawnBlacklist")] public EntityWhitelist? SpawnBlacklist = null;

    /// <summary>
    ///     Is this slot disabled? Could be due to severing or other reasons.
    /// </summary>
    [DataField] public bool Disabled;
}
