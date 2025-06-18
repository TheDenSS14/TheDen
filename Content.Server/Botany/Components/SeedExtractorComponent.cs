// SPDX-FileCopyrightText: 2020 Víctor Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 20kdc <asdd2808@gmail.com>
// SPDX-FileCopyrightText: 2021 Acruid <shatter66@gmail.com>
// SPDX-FileCopyrightText: 2021 Galactic Chimp <63882831+GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 2021 Paul <ritter.paul1+git@googlemail.com>
// SPDX-FileCopyrightText: 2021 Paul <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 2021 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 0x6273 <0x40@keemail.me>
// SPDX-FileCopyrightText: 2022 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2022 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2022 Vera Aguilera Puerto <6766154+Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+emogarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Botany.Systems;
using Content.Server.Construction;
using Content.Shared.Construction.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Botany.Components;

[RegisterComponent]
[Access(typeof(SeedExtractorSystem))]
public sealed partial class SeedExtractorComponent : Component
{
    /// <summary>
    ///     The minimum amount of seed packets dropped with no machine upgrades.
    /// </summary>
    [DataField]
    public int BaseMinSeeds = 1;

    /// <summary>
    ///     The maximum amount of seed packets dropped with no machine upgrades.
    /// </summary>
    [DataField]
    public int BaseMaxSeeds = 3;

    /// <summary>
    ///     Modifier to the amount of seeds outputted, set on <see cref="RefreshPartsEvent"/>.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float SeedAmountMultiplier;

    /// <summary>
    ///     Machine part whose rating modifies the amount of seed packets dropped.
    /// </summary>
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>))]
    public string MachinePartSeedAmount = "Manipulator";

    /// <summary>
    ///     How much the machine part quality affects the amount of seeds outputted.
    ///     Going up a tier will multiply the seed output by this amount.
    /// </summary>
    [DataField]
    public float PartRatingSeedAmountMultiplier = 1.5f;
}
