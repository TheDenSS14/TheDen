// SPDX-FileCopyrightText: 2023 Debug
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2025 Blitz
// SPDX-FileCopyrightText: 2025 BlitzTheSquishy
// SPDX-FileCopyrightText: 2025 EctoplasmIsGood
// SPDX-FileCopyrightText: 2025 VMSolidus
// SPDX-FileCopyrightText: 2025 pathetic meowmeow
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Lathe;
using Content.Shared.Research.Prototypes;
using Content.Shared.Research.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Shared.Research.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedResearchSystem), typeof(SharedLatheSystem)), AutoGenerateComponentState]
public sealed partial class TechnologyDatabaseComponent : Component
{
    /// <summary>
    ///     A main discipline that bypasses the T3 Softcap
    /// </summary>
    [AutoNetworkedField]
    [DataField("mainDiscipline", customTypeSerializer: typeof(PrototypeIdSerializer<TechDisciplinePrototype>))]
    public string? MainDiscipline;

    [AutoNetworkedField]
    [DataField("currentTechnologyCards")]
    public List<string> CurrentTechnologyCards = new();

    /// <summary>
    /// Which research disciplines are able to be unlocked
    /// </summary>
    [AutoNetworkedField]
    [DataField("supportedDisciplines", customTypeSerializer: typeof(PrototypeIdListSerializer<TechDisciplinePrototype>))]
    public List<string> SupportedDisciplines = new();

    /// <summary>
    /// The ids of all the technologies which have been unlocked.
    /// </summary>
    [AutoNetworkedField]
    [DataField("unlockedTechnologies", customTypeSerializer: typeof(PrototypeIdListSerializer<TechnologyPrototype>))]
    public List<string> UnlockedTechnologies = new();

    /// <summary>
    /// The ids of all the lathe recipes which have been unlocked.
    /// This is maintained alongside the TechnologyIds
    /// </summary>
    /// todo: if you unlock all the recipes in a tech, it doesn't count as unlocking the tech. sadge
    [AutoNetworkedField]
    [DataField("unlockedRecipes", customTypeSerializer: typeof(PrototypeIdListSerializer<LatheRecipePrototype>))]
    public List<string> UnlockedRecipes = new();

    [DataField, AutoNetworkedField]
    public float SoftCapMultiplier = 1;
}

/// <summary>
/// Event raised on the database whenever its
/// technologies or recipes are modified.
/// </summary>
/// <remarks>
/// This event is forwarded from the
/// server to all of it's clients.
/// </remarks>
[ByRefEvent]
public readonly record struct TechnologyDatabaseModifiedEvent // Goobstation - Lathe message on recipes update
{
    public readonly EntityUid Server;
    public readonly ProtoId<TechnologyPrototype>? Technology;
    public readonly List<ProtoId<LatheRecipePrototype>> UnlockedRecipes = new();

    public TechnologyDatabaseModifiedEvent(
        EntityUid server,
        ProtoId<TechnologyPrototype>? technology,
        List<ProtoId<LatheRecipePrototype>>? unlockedRecipes = null)
    {
        Server = server;
        Technology = technology;
        UnlockedRecipes = unlockedRecipes ?? new();
    }
};
