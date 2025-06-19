// SPDX-FileCopyrightText: 2022 Emisse <99158783+Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 ShatteredSwords <135023515+ShatteredSwords@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 SimpleStation14 <130339894+SimpleStation14@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 2024 WarMechanic <69510347+WarMechanic@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Inventory;
using Content.Shared.Popups;
using Content.Shared.Strip.Components;
using Robust.Shared.Serialization;

namespace Content.Shared.Strip;

public sealed class ThievingSystem : EntitySystem
{

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ThievingComponent, BeforeStripEvent>(OnBeforeStrip);
        SubscribeLocalEvent<ThievingComponent, InventoryRelayedEvent<BeforeStripEvent>>((e, c, ev) => OnBeforeStrip(e, c, ev.Args));
    }

    private void OnBeforeStrip(EntityUid uid, ThievingComponent component, BeforeStripEvent args)
    {
        args.Stealth = (ThievingStealth) Math.Max((sbyte) args.Stealth, (sbyte) component.Stealth);
        args.Additive -= component.StripTimeReduction;
        args.Multiplier *= component.StripTimeMultiplier;
    }

    public PopupType? GetPopupTypeFromStealth(ThievingStealth stealth)
    {
        switch (stealth)
        {
            case ThievingStealth.Hidden:
                return null;

            case ThievingStealth.Subtle:
                return PopupType.Small;

            default:
                return PopupType.Large;
        }
    }
}
[Serializable, NetSerializable]
public enum ThievingStealth : sbyte
{
    /// <summary>
    /// 	Target sees a large popup indicating that an item is being stolen by who
    /// </summary>
    Obvious = 0,

    /// <summary>
    /// 	Target sees a small popup indicating that an item is being stolen
    /// </summary>
    Subtle = 1,

    /// <summary>
    /// 	Target does not see any popup regarding the stealing of an item
    /// </summary>
    Hidden = 2
}
