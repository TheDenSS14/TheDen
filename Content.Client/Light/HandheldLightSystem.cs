// SPDX-FileCopyrightText: 2021 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Francesco <frafonia@gmail.com>
// SPDX-FileCopyrightText: 2022 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Checkraze <71046427+Cheackraze@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Visne <39844191+Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <39013340+deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 spess-empyrean <spessempyrean@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Client.Items;
using Content.Client.Light.Components;
using Content.Shared.Light;
using Content.Shared.Light.Components;
using Content.Shared.Toggleable;
using Robust.Client.GameObjects;
using Content.Client.Light.EntitySystems;

namespace Content.Client.Light;

public sealed class HandheldLightSystem : SharedHandheldLightSystem
{
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly LightBehaviorSystem _lightBehavior = default!;

    public override void Initialize()
    {
        base.Initialize();

        Subs.ItemStatus<HandheldLightComponent>(ent => new HandheldLightStatus(ent));
        SubscribeLocalEvent<HandheldLightComponent, AppearanceChangeEvent>(OnAppearanceChange);
    }

    private void OnAppearanceChange(EntityUid uid, HandheldLightComponent? component, ref AppearanceChangeEvent args)
    {
        if (!Resolve(uid, ref component))
        {
            return;
        }

        if (!_appearance.TryGetData<bool>(uid, ToggleableLightVisuals.Enabled, out var enabled, args.Component))
        {
            return;
        }

        if (!_appearance.TryGetData<HandheldLightPowerStates>(uid, HandheldLightVisuals.Power, out var state, args.Component))
        {
            return;
        }

        if (TryComp<LightBehaviourComponent>(uid, out var lightBehaviour))
        {
            // Reset any running behaviour to reset the animated properties back to the original value, to avoid conflicts between resets
            if (_lightBehavior.HasRunningBehaviours((uid, lightBehaviour)))
            {
                _lightBehavior.StopLightBehaviour((uid, lightBehaviour), resetToOriginalSettings: true);
            }

            if (!enabled)
            {
                return;
            }

            switch (state)
            {
                case HandheldLightPowerStates.FullPower:
                    break; // We just needed to reset all behaviours
                case HandheldLightPowerStates.LowPower:
                    _lightBehavior.StartLightBehaviour((uid, lightBehaviour), component.RadiatingBehaviourId);
                    break;
                case HandheldLightPowerStates.Dying:
                    _lightBehavior.StartLightBehaviour((uid, lightBehaviour), component.BlinkingBehaviourId);
                    break;
            }
        }
    }
}
