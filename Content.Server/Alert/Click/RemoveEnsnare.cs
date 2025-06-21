// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Vordenburg <114301317+Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 sleepyyapril <flyingkarii@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Ensnaring;
using Content.Shared.Alert;
using Content.Shared.Ensnaring.Components;
using JetBrains.Annotations;

namespace Content.Server.Alert.Click;
[UsedImplicitly]
[DataDefinition]
public sealed partial class RemoveEnsnare : IAlertClick
{
    public void AlertClicked(EntityUid player)
    {
        var entManager = IoCManager.Resolve<IEntityManager>();
        if (entManager.TryGetComponent(player, out EnsnareableComponent? ensnareableComponent))
        {
            foreach (var ensnare in ensnareableComponent.Container.ContainedEntities)
            {
                if (!entManager.TryGetComponent(ensnare, out EnsnaringComponent? ensnaringComponent))
                    return;

                entManager.EntitySysManager.GetEntitySystem<EnsnareableSystem>().TryFree(player, player, ensnare, ensnaringComponent);

                // Only one snare at a time.
                break;
            }
        }
    }
}
