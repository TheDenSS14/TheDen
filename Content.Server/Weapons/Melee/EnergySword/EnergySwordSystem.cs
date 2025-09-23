// SPDX-FileCopyrightText: 2022 CommieFlowers
// SPDX-FileCopyrightText: 2022 Flipp Syder
// SPDX-FileCopyrightText: 2022 Júlio César Ueti
// SPDX-FileCopyrightText: 2022 Kara
// SPDX-FileCopyrightText: 2022 Leon Friedrich
// SPDX-FileCopyrightText: 2022 Rane
// SPDX-FileCopyrightText: 2022 keronshb
// SPDX-FileCopyrightText: 2022 metalgearsloth
// SPDX-FileCopyrightText: 2022 mirrorcult
// SPDX-FileCopyrightText: 2022 rolfero
// SPDX-FileCopyrightText: 2023 Darkie
// SPDX-FileCopyrightText: 2023 DrSmugleaf
// SPDX-FileCopyrightText: 2023 Errant
// SPDX-FileCopyrightText: 2023 FacePluslll
// SPDX-FileCopyrightText: 2023 Nemanja
// SPDX-FileCopyrightText: 2023 Pieter-Jan Briers
// SPDX-FileCopyrightText: 2023 Slava0135
// SPDX-FileCopyrightText: 2023 deltanedas
// SPDX-FileCopyrightText: 2023 dontbetank
// SPDX-FileCopyrightText: 2024 Debug
// SPDX-FileCopyrightText: 2024 Verm
// SPDX-FileCopyrightText: 2025 sleepyyapril
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Interaction;
using Content.Shared.Light;
using Content.Shared.Light.Components;
using Content.Shared.Toggleable;
using Content.Shared.Tools.Systems;
using Robust.Shared.Random;

namespace Content.Server.Weapons.Melee.EnergySword;

public sealed class EnergySwordSystem : EntitySystem
{
    [Dependency] private readonly SharedRgbLightControllerSystem _rgbSystem = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedToolSystem _toolSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<EnergySwordComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<EnergySwordComponent, InteractUsingEvent>(OnInteractUsing);
    }
    // Used to pick a random color for the blade on map init.
    private void OnMapInit(EntityUid uid, EnergySwordComponent comp, MapInitEvent args)
    {
        if (comp.ColorOptions.Count != 0)
            comp.ActivatedColor = _random.Pick(comp.ColorOptions);

        if (!TryComp(uid, out AppearanceComponent? appearanceComponent))
            return;
        _appearance.SetData(uid, ToggleableVisuals.Color, comp.ActivatedColor, appearanceComponent);
    }

    // Used to make the make the blade multicolored when using a multitool on it.
    private void OnInteractUsing(EntityUid uid, EnergySwordComponent comp, InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        if (!_toolSystem.HasQuality(args.Used, SharedToolSystem.PulseQuality))
            return;

        args.Handled = true;
        comp.Hacked = !comp.Hacked;

        if (comp.Hacked)
        {
            var rgb = EnsureComp<RgbLightControllerComponent>(uid);
            _rgbSystem.SetCycleRate(uid, comp.CycleRate, rgb);
        }
        else
            RemComp<RgbLightControllerComponent>(uid);
    }
}
