// SPDX-FileCopyrightText: 2022 0x6273 <0x40@keemail.me>
// SPDX-FileCopyrightText: 2022 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 2022 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 rolfero <45628623+rolfero@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 chromiumboy <50505512+chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Nemanja <98561806+emogarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Server.Construction;
using Content.Server.Power.Components;
using JetBrains.Annotations;

namespace Content.Server.Power.EntitySystems
{
    [UsedImplicitly]
    public sealed class UpgradeBatterySystem : EntitySystem
    {
        [Dependency] private readonly BatterySystem _batterySystem = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<UpgradeBatteryComponent, RefreshPartsEvent>(OnRefreshParts);
            SubscribeLocalEvent<UpgradeBatteryComponent, UpgradeExamineEvent>(OnUpgradeExamine);
        }

        public void OnRefreshParts(EntityUid uid, UpgradeBatteryComponent component, RefreshPartsEvent args)
        {
            var powerCellRating = args.PartRatings[component.MachinePartPowerCapacity];

            if (TryComp<BatteryComponent>(uid, out var batteryComp))
            {
                _batterySystem.SetMaxCharge(uid, MathF.Pow(component.MaxChargeMultiplier, powerCellRating - 1) * component.BaseMaxCharge, batteryComp);
            }
        }

        private void OnUpgradeExamine(EntityUid uid, UpgradeBatteryComponent component, UpgradeExamineEvent args)
        {
            // UpgradeBatteryComponent.MaxChargeMultiplier is not the actual multiplier, so we have to do this.
            if (TryComp<BatteryComponent>(uid, out var batteryComp))
            {
                args.AddPercentageUpgrade("upgrade-max-charge", batteryComp.MaxCharge / component.BaseMaxCharge);
            }
        }
    }
}
