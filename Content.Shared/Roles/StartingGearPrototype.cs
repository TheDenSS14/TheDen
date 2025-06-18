// SPDX-FileCopyrightText: 2019 ZelteHonor <gabrieldionbouchard@gmail.com>
// SPDX-FileCopyrightText: 2020 20kdc <asdd2808@gmail.com>
// SPDX-FileCopyrightText: 2020 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 2020 ike709 <ike709@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Paul <ritter.paul1+git@googlemail.com>
// SPDX-FileCopyrightText: 2021 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2021 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2022 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Vordenburg <114301317+Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 Skubman <ba.fallaria@gmail.com>
// SPDX-FileCopyrightText: 2025 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using System.Linq;
using Content.Shared.Customization.Systems;
using Content.Shared.Preferences;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;

namespace Content.Shared.Roles;

[Prototype("startingGear")]
public sealed partial class StartingGearPrototype : IPrototype, IInheritingPrototype
{
    [DataField]
    [AlwaysPushInheritance]
    public Dictionary<string, EntProtoId> Equipment = new();

    /// <summary>
    ///     If empty, there is no skirt override - instead the uniform provided in equipment is added.
    /// </summary>
    [DataField]
    public EntProtoId? InnerClothingSkirt;

    [DataField]
    public EntProtoId? Satchel;

    [DataField]
    public EntProtoId? Duffelbag;

    [DataField]
    [AlwaysPushInheritance]
    public List<EntProtoId> Inhand = new(0);

    /// <summary>
    ///     Inserts entities into the specified slot's storage (if it does have storage).
    /// </summary>
    [DataField]
    [AlwaysPushInheritance]
    public Dictionary<string, List<EntProtoId>> Storage = new();

    /// <summary>
    ///     The list of starting gears that overwrite the entries on this starting gear
    ///     if their requirements are satisfied.
    /// </summary>
    [DataField("subGear")]
    [AlwaysPushInheritance]
    public List<ProtoId<StartingGearPrototype>> SubGears = new();

    /// <summary>
    ///     The requirements of this starting gear.
    ///     Only used if this starting gear is a sub-gear of another starting gear.
    /// </summary>
    [DataField]
    [AlwaysPushInheritance]
    public List<CharacterRequirement> Requirements = new();

    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = string.Empty;

    /// <inheritdoc/>
    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<StartingGearPrototype>))]
    public string[]? Parents { get; private set; }

    /// <inheritdoc/>
    [AbstractDataField]
    [NeverPushInheritance]
    public bool Abstract { get; }

    public string GetGear(string slot, HumanoidCharacterProfile? profile)
    {
        if (profile != null)
        {
            var forceSkirt = new[] { "Harpy", "Lamia" };

            switch (slot)
            {
                case "jumpsuit" when profile.Clothing == ClothingPreference.Jumpskirt && !string.IsNullOrEmpty(InnerClothingSkirt):
                case "jumpsuit" when forceSkirt.Contains(profile.Species) && !string.IsNullOrEmpty(InnerClothingSkirt):
                    return InnerClothingSkirt;
                case "back" when profile.Backpack == BackpackPreference.Satchel && !string.IsNullOrEmpty(Satchel):
                    return Satchel;
                case "back" when profile.Backpack == BackpackPreference.Duffelbag && !string.IsNullOrEmpty(Duffelbag):
                    return Duffelbag;
            }
        }

        return Equipment.TryGetValue(slot, out var equipment) ? equipment : string.Empty;
    }
}
