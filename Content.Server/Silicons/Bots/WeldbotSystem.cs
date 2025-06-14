using Content.Server.Chat.Systems;
using Content.Shared.Chat;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.DoAfter;
using Content.Shared.Emag.Components;
using Content.Shared.Silicons.Bots;
using Content.Shared.Tag;
using Robust.Shared.Prototypes;

namespace Content.Server.Silicons.Bots;

public sealed class WeldbotSystem : SharedWeldbotSystem
{
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly TagSystem _tag = default!;

    public const string SiliconTag = "SiliconMob";
    public const string FixableStructureTag = "WeldbotFixableStructure";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<WeldbotWeldEntityDoAfterEvent>(OnDoWeldEntity);
    }

    private void OnDoWeldEntity(ref WeldbotWeldEntityDoAfterEvent args)
    {
        if (args.Target == null
            || args.Handled
            || args.Cancelled
            || !TryComp<WeldbotComponent>(args.User, out var weldbotComp)
            || !HasComp<DamageableComponent>(args.Target))
            return;

        var weldbot = new Entity<WeldbotComponent>(args.User, weldbotComp);
        var target = args.Target.Value;
        var isStructure = CanWeldStructure(weldbot, target);
        var isMob = CanWeldMob(weldbot, target);

        Audio.PlayPvs(weldbotComp.WeldSound, target);

        if (isStructure)
            WeldStructure(weldbot, target);
        else if (isMob)
        {
            if (HasComp<EmaggedComponent>(weldbot.Owner))
            {
                SabotageMob(weldbot, target);
                return;
            }

            WeldMob(weldbot, target);
        }

        if (WeldingIsFinished(target))
            _chat.TrySendInGameICMessage(weldbot.Owner,
                Loc.GetString("weldbot-finish-weld"),
                InGameICChatType.Speak,
                hideChat: true,
                hideLog: true);
    }

    public bool TryWeldEntity(Entity<WeldbotComponent> weldbot, EntityUid target, bool sayTheLine = false)
    {
        if (!CanWeldEntity(weldbot, target))
            return false;

        if (sayTheLine)
            _chat.TrySendInGameICMessage(weldbot.Owner,
                Loc.GetString("weldbot-start-weld"),
                InGameICChatType.Speak,
                hideChat: true,
                hideLog: true);

        var doAfterEventArgs = new DoAfterArgs(EntityManager,
            weldbot.Owner,
            weldbot.Comp.DoAfterLength,
            new WeldbotWeldEntityDoAfterEvent(),
            weldbot.Owner,
            target)
        {
            BreakOnMove = target != weldbot.Owner,
            BreakOnWeightlessMove = false,
            NeedHand = false,
            Broadcast = true
        };

        _doAfter.TryStartDoAfter(doAfterEventArgs);
        return true;
    }

    public void WeldMob(Entity<WeldbotComponent> weldbot, EntityUid target)
    {

        if (!_proto.TryIndex<DamageGroupPrototype>("Brute", out var brute))
            return;

        var specifier = new DamageSpecifier(brute, -weldbot.Comp.SiliconRepairAmount);
        _damageable.TryChangeDamage(target, specifier, true, false);
    }

    public void WeldStructure(Entity<WeldbotComponent> weldbot, EntityUid target)
    {
        if (!TryComp<DamageableComponent>(target, out var damage))
            return;

        _damageable.ChangeAllDamage(target, damage, -weldbot.Comp.StructureRepairAmount);
    }

    public void SabotageMob(Entity<WeldbotComponent> weldbot, EntityUid target)
    {

        if (!_proto.TryIndex<DamageGroupPrototype>("Burn", out var burn))
            return;

        var specifier = new DamageSpecifier(burn, weldbot.Comp.EmaggedBurnDamage);
        _damageable.TryChangeDamage(target, specifier, true, false);
    }

    private bool WeldingIsFinished(EntityUid target)
    {
        if (!TryComp<DamageableComponent>(target, out var damage))
            return false;

        var isMob = IsWeldableMob(target);
        var isStructure = IsWeldableStructure(target);

        return isMob && damage.DamagePerGroup["Brute"].Value <= 0
            || isStructure && damage.TotalDamage <= 0;
    }

    public bool IsWeldableMob(EntityUid target) => EntityHasTag(target, SiliconTag);

    public bool IsWeldableStructure(EntityUid target) => EntityHasTag(target, FixableStructureTag);

    public bool CanWeldEntity(Entity<WeldbotComponent> weldbot, EntityUid target)
    {
        return CanWeldMob(weldbot, target)
            || CanWeldStructure(weldbot, target);
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

    private bool EntityHasTag(EntityUid uid, string tag)
    {
        if (!TryComp<TagComponent>(uid, out var tagComp))
            return false;

        var tagProto = _proto.Index<TagPrototype>(tag);
        return _tag.HasTag(tagComp, tagProto);
    }
}
