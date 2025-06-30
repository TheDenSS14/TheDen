// SPDX-FileCopyrightText: 2024 FoxxoTrystan <45297731+FoxxoTrystan@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 FoxxoTrystan <trystan.garnierhein@gmail.com>
// SPDX-FileCopyrightText: 2025 SixplyDev <einlichen@gmail.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <flyingkarii@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Content.Shared.Examine;
using Content.Shared.Abilities.Psionics;
using Content.Shared.Humanoid;
using Content.Shared.Psionics;
using Content.Shared.Bed.Sleep;
using Content.Shared.Damage.Components;
using Content.Shared.Damage.Systems;
using Content.Shared.Shadowkin;
using Content.Shared.Rejuvenate;
using Content.Shared.Alert;
using Content.Shared.Rounding;
using Content.Shared.Actions;
using Robust.Shared.Prototypes;
using Content.Server.Abilities.Psionics;
using Content.Shared.Mobs; // Floofstation Edit
using Content.Server.FloofStation; // Floofstation Edit
using Content.Shared.Inventory; // Floofstation Edit
using Robust.Shared.Physics.Systems; // Floofstation Edit
using Robust.Shared.Random; // Floofstation Edit
using Robust.Shared.Utility; // Floofstation Edit
using System.Linq; // Floofstation Edit

namespace Content.Server.Shadowkin;

public sealed class ShadowkinSystem : EntitySystem
{
    [Dependency] private readonly StaminaSystem _stamina = default!;
    [Dependency] private readonly PsionicAbilitiesSystem _psionicAbilitiesSystem = default!;
    [Dependency] private readonly AlertsSystem _alerts = default!;
    [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!; // Floofstation Edit
    [Dependency] private readonly InventorySystem _inventorySystem = default!; // Floofstation Edit
    [Dependency] private readonly IRobustRandom _random = default!; // Floofstation Edit
    [Dependency] private readonly EntityLookupSystem _lookup = default!; // Floofstation Edit
    [Dependency] private readonly SharedJointSystem _joints = default!; // Floofstation

    public const string ShadowkinSleepActionId = "ShadowkinActionSleep";

    private const int MaxRandomTeleportAttempts = 20; // Floofstation Edit
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ShadowkinComponent, ComponentStartup>(OnInit);
        SubscribeLocalEvent<ShadowkinComponent, OnMindbreakEvent>(OnMindbreak);
        SubscribeLocalEvent<ShadowkinComponent, RejuvenateEvent>(OnRejuvenate);
        SubscribeLocalEvent<ShadowkinComponent, EyeColorInitEvent>(OnEyeColorChange);
    }

    private void OnInit(EntityUid uid, ShadowkinComponent component, ComponentStartup args)
    {
        _actionsSystem.AddAction(uid, ref component.ShadowkinSleepAction, ShadowkinSleepActionId, uid);
    }

    private void OnEyeColorChange(EntityUid uid, ShadowkinComponent component, EyeColorInitEvent args)
    {
        if (!TryComp<HumanoidAppearanceComponent>(uid, out var humanoid)
            || humanoid.EyeColor == component.OldEyeColor)
            return;

        component.OldEyeColor = humanoid.EyeColor;
        Dirty(uid, humanoid);
    }

    private void OnMindbreak(EntityUid uid, ShadowkinComponent component, ref OnMindbreakEvent args)
    {
        if (TryComp<MindbrokenComponent>(uid, out var mindbreak))
            mindbreak.MindbrokenExaminationText = "examine-mindbroken-shadowkin-message";

        if (TryComp<HumanoidAppearanceComponent>(uid, out var humanoid))
        {
            component.OldEyeColor = humanoid.EyeColor;
            humanoid.EyeColor = component.BlackEyeColor;
            Dirty(uid, humanoid);
        }

        if (TryComp<StaminaComponent>(uid, out var stamina))
            _stamina.TakeStaminaDamage(uid, stamina.CritThreshold, stamina, uid);
    }

    private void OnRejuvenate(EntityUid uid, ShadowkinComponent component, RejuvenateEvent args)
    {
        if (!HasComp<MindbrokenComponent>(uid))
            return;

        RemComp<MindbrokenComponent>(uid);

        if (TryComp<HumanoidAppearanceComponent>(uid, out var humanoid))
        {
            humanoid.EyeColor = component.OldEyeColor;
            Dirty(uid, humanoid);
        }

        EnsureComp<PsionicComponent>(uid, out _);
        if (_prototypeManager.TryIndex<PsionicPowerPrototype>("ShadowkinPowers", out var shadowkinPowers))
            _psionicAbilitiesSystem.InitializePsionicPower(uid, shadowkinPowers);
    }
}
