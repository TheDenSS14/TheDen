// SPDX-FileCopyrightText: 2021 Acruid
// SPDX-FileCopyrightText: 2021 DrSmugleaf
// SPDX-FileCopyrightText: 2021 Silver
// SPDX-FileCopyrightText: 2021 Vera Aguilera Puerto
// SPDX-FileCopyrightText: 2021 Visne
// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2022 wrexbe
// SPDX-FileCopyrightText: 2023 metalgearsloth
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: MIT AND AGPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using Content.Server._DEN.Markings;
using Content.Server.Administration;
using Content.Server.Administration.Systems;
using Content.Server.GameTicking;
using Content.Shared._DEN.Species;
using Content.Shared.Administration;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.HeightAdjust;
using Content.Shared.Humanoid;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server._DEN.Administration.Commands;

[AdminCommand(AdminFlags.Admin)]
sealed class TestAnimationCommand : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _entityManager = null!;

    public string Command => "testanimden";
    public string Description => "Give yourself the glowy marking animations. CVar needs to be turned on.";
    public string Help => "testanimden";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var animatedMarkings = _entityManager.System<AnimatedMarkingsSystem>();

        if (shell.Player == null || shell.Player.AttachedEntity is not { Valid: true } attachedEntity)
        {
            shell.WriteError("bad");
            return;
        }

        if (!_entityManager.TryGetComponent<HumanoidAppearanceComponent>(attachedEntity, out var humanoidMaybe)
            || humanoidMaybe is not { } humanoid)
        {
            shell.WriteError("bad, no humanoid");
            return;
        }

        animatedMarkings.DoDebugAnimation((attachedEntity, humanoid));
    }
}

