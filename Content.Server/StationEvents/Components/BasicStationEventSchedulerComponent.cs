// SPDX-FileCopyrightText: 2023 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.EntityTable.EntitySelectors;
namespace Content.Server.StationEvents.Components;

[RegisterComponent, Access(typeof(BasicStationEventSchedulerSystem))]
public sealed partial class BasicStationEventSchedulerComponent : Component
{
    [DataField]
    public const float MinimumTimeUntilFirstEvent = 300;

    /// <summary>
    /// How long until the next check for an event runs
    /// </summary>
    /// Default value is how long until first event is allowed
    [ViewVariables(VVAccess.ReadWrite)]
    public float TimeUntilNextEvent = MinimumTimeUntilFirstEvent;

    /// <summary>
    /// The gamerules that the scheduler can choose from
    /// </summary>
    /// Reminder that though we could do all selection via the EntityTableSelector, we also need to consider various <see cref="StationEventComponent"/> restrictions.
    /// As such, we want to pass a list of acceptable game rules, which are then parsed for restrictions by the <see cref="EventManagerSystem"/>.
    [DataField(required: true)]
    public EntityTableSelector ScheduledGameRules = default!;
}
