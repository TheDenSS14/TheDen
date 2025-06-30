// SPDX-FileCopyrightText: 2020 Víctor Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 20kdc <asdd2808@gmail.com>
// SPDX-FileCopyrightText: 2021 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2021 E F R <602406+Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Galactic Chimp <63882831+GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Galactic Chimp <GalacticChimpanzee@gmail.com>
// SPDX-FileCopyrightText: 2021 Mariner102 <bcarcham@asu.edu>
// SPDX-FileCopyrightText: 2021 Paul <ritter.paul1+git@googlemail.com>
// SPDX-FileCopyrightText: 2021 Paul <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2021 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2021 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2021 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Ygg01 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 2022 Kevin Zheng <kevinz5000@gmail.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 Rane <60792108+Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 TemporalOroboros <temporaloroboros@gmail.com>
// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Chemistry.Components;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Botany.Components;

[RegisterComponent]
public sealed partial class PlantHolderComponent : Component
{
    [DataField("nextUpdate", customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan NextUpdate = TimeSpan.Zero;
    [ViewVariables(VVAccess.ReadWrite), DataField("updateDelay")]
    public TimeSpan UpdateDelay = TimeSpan.FromSeconds(3);

    [DataField("lastProduce")]
    public int LastProduce;

    [ViewVariables(VVAccess.ReadWrite), DataField("missingGas")]
    public int MissingGas;

    [DataField("cycleDelay")]
    public TimeSpan CycleDelay = TimeSpan.FromSeconds(15f);

    [DataField("lastCycle", customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan LastCycle = TimeSpan.Zero;

    [ViewVariables(VVAccess.ReadWrite), DataField("updateSpriteAfterUpdate")]
    public bool UpdateSpriteAfterUpdate;

    [ViewVariables(VVAccess.ReadWrite), DataField("drawWarnings")]
    public bool DrawWarnings = false;

    [ViewVariables(VVAccess.ReadWrite), DataField("waterLevel")]
    public float WaterLevel = 100f;

    [ViewVariables(VVAccess.ReadWrite), DataField("nutritionLevel")]
    public float NutritionLevel = 100f;

    [ViewVariables(VVAccess.ReadWrite), DataField("pestLevel")]
    public float PestLevel;

    [ViewVariables(VVAccess.ReadWrite), DataField("weedLevel")]
    public float WeedLevel;

    [ViewVariables(VVAccess.ReadWrite), DataField("toxins")]
    public float Toxins;

    [ViewVariables(VVAccess.ReadWrite), DataField("age")]
    public int Age;

    [ViewVariables(VVAccess.ReadWrite), DataField("skipAging")]
    public int SkipAging;

    [ViewVariables(VVAccess.ReadWrite), DataField("dead")]
    public bool Dead;

    [ViewVariables(VVAccess.ReadWrite), DataField("harvest")]
    public bool Harvest;

    [ViewVariables(VVAccess.ReadWrite), DataField("sampled")]
    public bool Sampled;

    [ViewVariables(VVAccess.ReadWrite), DataField("yieldMod")]
    public int YieldMod = 1;

    [ViewVariables(VVAccess.ReadWrite), DataField("mutationMod")]
    public float MutationMod = 1f;

    [ViewVariables(VVAccess.ReadWrite), DataField("mutationLevel")]
    public float MutationLevel;

    [ViewVariables(VVAccess.ReadWrite), DataField("health")]
    public float Health;

    [ViewVariables(VVAccess.ReadWrite), DataField("weedCoefficient")]
    public float WeedCoefficient = 1f;

    [ViewVariables(VVAccess.ReadWrite), DataField("seed")]
    public SeedData? Seed;

    [ViewVariables(VVAccess.ReadWrite), DataField("improperHeat")]
    public bool ImproperHeat;

    [ViewVariables(VVAccess.ReadWrite), DataField("improperPressure")]
    public bool ImproperPressure;

    [ViewVariables(VVAccess.ReadWrite), DataField("improperLight")]
    public bool ImproperLight;

    [ViewVariables(VVAccess.ReadWrite), DataField("forceUpdate")]
    public bool ForceUpdate;

    [ViewVariables(VVAccess.ReadWrite), DataField("solution")]
    public string SoilSolutionName = "soil";

    [ViewVariables]
    public Entity<SolutionComponent>? SoilSolution = null;
}
