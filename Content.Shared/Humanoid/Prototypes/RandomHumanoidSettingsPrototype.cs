// SPDX-FileCopyrightText: 2022 Flipp Syder <76629141+vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;

namespace Content.Shared.Humanoid.Prototypes;

/// <summary>
///     This is what is used to change a humanoid spawned by RandomHumanoidSystem in Content.Server.
/// </summary>
[Prototype("randomHumanoidSettings")]
public sealed partial class RandomHumanoidSettingsPrototype : IPrototype, IInheritingPrototype
{
    [IdDataField] public string ID { get; } = default!;

    [ParentDataField(typeof(PrototypeIdArraySerializer<RandomHumanoidSettingsPrototype>))]
    public string[]? Parents { get; }

    [AbstractDataField]
    public bool Abstract { get; }

    /// <summary>
    ///     Whether the humanoid's name should take from the randomized profile or not.
    /// </summary>
    [DataField("randomizeName")]
    public bool RandomizeName { get; private set; } = true;

    /// <summary>
    ///     Species that will be ignored by the randomizer.
    /// </summary>
    [DataField("speciesBlacklist")]
    public HashSet<string> SpeciesBlacklist { get; private set; } = new();

    /// <summary>
    ///     Extra components to add to this entity.
    /// </summary>
    [DataField("components")]
    public ComponentRegistry? Components { get; private set; }
}
