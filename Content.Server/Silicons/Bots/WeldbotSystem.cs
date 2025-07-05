// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using Content.Server.Chat.Systems;
using Content.Server.Hands.Systems;
using Content.Server.Repairable;
using Content.Shared.Chat;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.DoAfter;
using Content.Shared.Emag.Components;
using Content.Shared.Hands.Components;
using Content.Shared.Silicons.Bots;
using Content.Shared.Tag;
using Content.Shared.Tools.Components;
using Robust.Shared.Prototypes;

namespace Content.Server.Silicons.Bots;

public sealed class WeldbotSystem : SharedWeldbotSystem
{
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly TagSystem _tag = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;

    public const string SiliconTag = "SiliconMob";
    public const string FixableStructureTag = "WeldbotFixableStructure";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<WeldbotComponent, RepairedEvent>(OnRepairedObject);
    }

    public void OnRepairedObject(EntityUid uid, WeldbotComponent component, RepairedEvent args)
    {
        if (WeldingIsFinished(args.Ent.Owner))
        {
            _chat.TrySendInGameICMessage(uid,
                Loc.GetString("weldbot-finish-weld"),
                InGameICChatType.Speak,
                hideChat: true,
                hideLog: true);
        }
    }

    public bool WeldingIsFinished(EntityUid target)
    {
        if (!TryComp<DamageableComponent>(target, out var damage))
            return false;

        var isMob = IsWeldableMob(target);
        var isStructure = IsWeldableStructure(target);

        return isMob && damage.DamagePerGroup["Brute"].Value <= 0
            || isStructure && damage.TotalDamage <= 0;
    }

    public bool CanWeldEntity(Entity<WeldbotComponent> weldbot, EntityUid target)
    {
        return CanWeldMob(weldbot, target)
            || CanWeldStructure(weldbot, target);
    }

    public bool HasEnoughFuel(Entity<WeldbotComponent> weldbot, Entity<WelderComponent> welder)
    {
        var fuelCost = weldbot.Comp.ExpectedFuelCost;

        if (!TryComp<SolutionContainerManagerComponent>(welder.Owner, out var container)
            || !_solutionContainer.TryGetSolution((welder.Owner, container),
                welder.Comp.FuelSolutionName,
                out var solution))
            return false;

        return solution.Value.Comp.Solution.Volume >= fuelCost;
    }

    public bool HasEnoughFuel(Entity<WeldbotComponent> weldbot)
    {
        if (!TryGetWelder(weldbot, out var welderEntity))
            return false;

        return HasEnoughFuel(weldbot, welderEntity.Value);
    }

    public bool TryGetWelder(Entity<WeldbotComponent> weldbot,
        [NotNullWhen(true)] out Entity<WelderComponent>? welder)
    {
        welder = null;

        if (TryComp<HandsComponent>(weldbot.Owner, out var hands)
            && hands.ActiveHandEntity != null
            && TryComp<WelderComponent>(hands.ActiveHandEntity, out var welderComp))
        {
            welder = (hands.ActiveHandEntity.Value, welderComp);
            return true;
        }

        return false;
    }

    public bool CanWeldMob(Entity<WeldbotComponent> weldbot, EntityUid target)
    {
        return IsWeldableMob(target)
            && TryComp<DamageableComponent>(target, out var damage)
            && (damage.DamagePerGroup["Brute"].Value > 0
                || weldbot.Comp.IsEmagged);
    }

    public bool CanWeldStructure(Entity<WeldbotComponent> weldbot, EntityUid target)
    {
        return !weldbot.Comp.IsEmagged
            && IsWeldableStructure(target)
            && TryComp<DamageableComponent>(target, out var damage)
            && damage.TotalDamage > 0;
    }

    public bool IsWeldableMob(EntityUid target) => EntityHasTag(target, SiliconTag);

    public bool IsWeldableStructure(EntityUid target) => EntityHasTag(target, FixableStructureTag);

    private bool EntityHasTag(EntityUid uid, string tag)
    {
        if (!TryComp<TagComponent>(uid, out var tagComp))
            return false;

        var tagProto = _proto.Index<TagPrototype>(tag);
        return _tag.HasTag(tagComp, tagProto);
    }
}
