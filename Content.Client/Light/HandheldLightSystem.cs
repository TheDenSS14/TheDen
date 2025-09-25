// SPDX-FileCopyrightText: 2021 metalgearsloth
// SPDX-FileCopyrightText: 2022 Francesco
// SPDX-FileCopyrightText: 2022 Kara
// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2023 Checkraze
// SPDX-FileCopyrightText: 2023 Visne
// SPDX-FileCopyrightText: 2023 deltanedas
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2025 Winter
// SPDX-FileCopyrightText: 2025 sleepyyapril
// SPDX-FileCopyrightText: 2025 spess-empyrean
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

        if (!_appearance.TryGetData<bool>(uid, ToggleableVisuals.Enabled, out var enabled, args.Component))
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
