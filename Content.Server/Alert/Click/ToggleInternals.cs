// SPDX-FileCopyrightText: 2022 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Body.Systems;
using Content.Shared.Alert;
using JetBrains.Annotations;

namespace Content.Server.Alert.Click;

/// <summary>
/// Attempts to toggle the internals for a particular entity
/// </summary>
[UsedImplicitly]
[DataDefinition]
public sealed partial class ToggleInternals : IAlertClick
{
    public void AlertClicked(EntityUid player)
    {
        var internalsSystem = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<InternalsSystem>();
        internalsSystem.ToggleInternals(player, player, false);
    }
}
