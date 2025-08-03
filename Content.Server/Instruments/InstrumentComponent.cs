// SPDX-FileCopyrightText: 2020 Jackson Lewis
// SPDX-FileCopyrightText: 2020 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2020 Tyler Young
// SPDX-FileCopyrightText: 2020 Víctor Aguilera Puerto
// SPDX-FileCopyrightText: 2020 chairbender
// SPDX-FileCopyrightText: 2020 derek
// SPDX-FileCopyrightText: 2020 zumorica
// SPDX-FileCopyrightText: 2021 20kdc
// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 Galactic Chimp
// SPDX-FileCopyrightText: 2021 Paul
// SPDX-FileCopyrightText: 2021 Paul Ritter
// SPDX-FileCopyrightText: 2021 Visne
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 Debug
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Kara
// SPDX-FileCopyrightText: 2023 Leon Friedrich
// SPDX-FileCopyrightText: 2023 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT
// SPDX-FileCopyrightText: 2024 VMSolidus
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.UserInterface;
using Content.Shared.Instruments;
using Robust.Shared.Player;
using ActivatableUIComponent = Content.Shared.UserInterface.ActivatableUIComponent;

namespace Content.Server.Instruments;

[RegisterComponent]
public sealed partial class InstrumentComponent : SharedInstrumentComponent
{
    [Dependency] private readonly IEntityManager _entMan = default!;

    [ViewVariables] public float Timer = 0f;
    [ViewVariables] public int BatchesDropped = 0;
    [ViewVariables] public int LaggedBatches = 0;
    [ViewVariables] public int MidiEventCount = 0;
    [ViewVariables] public uint LastSequencerTick = 0;

    // TODO Instruments: Make this ECS
    public EntityUid? InstrumentPlayer =>
        _entMan.GetComponentOrNull<ActivatableUIComponent>(Owner)?.CurrentSingleUser
        ?? _entMan.GetComponentOrNull<ActorComponent>(Owner)?.PlayerSession.AttachedEntity;
}