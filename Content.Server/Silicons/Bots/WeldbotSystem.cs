// SPDX-FileCopyrightText: 2025 portfiend
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using Content.Server.Chat.Systems;
using Content.Server.DoAfter;
using Content.Server.Hands.Systems;
using Content.Server.Repairable;
using Content.Server.Tools;
using Content.Shared.Chat;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Damage;
using Content.Shared.DoAfter;
using Content.Shared.Hands.Components;
using Content.Shared.NPC;
using Content.Shared.Silicon.WeldingHealing;
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
    [Dependency] private readonly ToolSystem _tool = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;

    public const string SiliconTag = "SiliconMob";
    public const string FixableStructureTag = "WeldbotFixableStructure";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<WeldbotComponent, RepairedEvent>(OnRepairedObject);
        SubscribeLocalEvent<WeldbotComponent, RepairedSiliconEvent>(OnRepairedSilicon);
        SubscribeLocalEvent<WeldbotComponent, ToolUserAttemptUseEvent>(PreventRedundantWelding);
    }

    public void OnRepairedObject(Entity<WeldbotComponent> weldbot, ref RepairedEvent args)
    {
        AnnounceIfFinished(weldbot, args.Ent.Owner);
    }

    public void OnRepairedSilicon(Entity<WeldbotComponent> weldbot, ref RepairedSiliconEvent args)
    {
        if (args.Target == null)
            return;

        AnnounceIfFinished(weldbot, args.Target.Value);
    }

    private void AnnounceIfFinished(Entity<WeldbotComponent> weldbot, EntityUid target)
    {
        if (!WeldingIsFinished(target))
            return;

        _chat.TrySendInGameICMessage(weldbot.Owner,
            Loc.GetString("weldbot-finish-weld"),
            InGameICChatType.Speak,
            hideChat: true,
            hideLog: true);
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

    public void PreventRedundantWelding(Entity<WeldbotComponent> weldbot, ref ToolUserAttemptUseEvent args)
    {
        // Weldbots with no AI (i.e. they are player controlled) are unaffected.
        if (!HasComp<ActiveNPCComponent>(weldbot.Owner))
            return;

        if (!TryComp<DoAfterComponent>(weldbot.Owner, out var doAfter))
            return;

        // TODO: Wow, this sucks! Here's a list of reasons why:
        // Always assumes the weldbot has one tool.
        // Always assumes that single tool is a welder / repair tool.
        // Always assumes that the tool is being used to repair the target.
        // Weirdly hard-coded for tool events.
        // Unfortunately, DoAfter doesn't really have a good way to check if
        // a specific DoAfterEvent is in progress, so this will have to do.

        // This event is executed before the DoAfterSystem checks for duplicates (and cancels them).
        // So if we cancel this event, then the check for duplicates never happens, and our weldbot
        // can continue welding on the same DoAfter attempt it is already using.
        // Without this, it'll get in an infinite loop of cancelling its repair attempts.
        if (_tool.IsUsingAnyToolOnTarget((weldbot.Owner, doAfter), args.Target))
            args.Cancelled = true;
    }
}
