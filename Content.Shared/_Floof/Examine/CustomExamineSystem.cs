using System.Text.RegularExpressions;
using Content.Shared.ActionBlocker;
using Content.Shared.Administration.Managers;
using Content.Shared.Consent;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Utility;


namespace Content.Shared._Floof.Examine;


public abstract class SharedCustomExamineSystem : EntitySystem
{
    public static ProtoId<ConsentTogglePrototype> NsfwDescConsent = "NSFWDescriptions";
    public static int PublicMaxLength = 256, SubtleMaxLength = 256;
    /// <summary>Max length of any content field, INCLUDING markup.</summary>
    public static int AbsolutelyMaxLength = 1024;

    private static readonly Regex BadMarkupRegex = new("\\[.*?head.*?\\]", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(5));

    [Dependency] private readonly SharedConsentSystem _consent = default!;
    [Dependency] private readonly ExamineSystemShared _examine = default!;
    [Dependency] private readonly ISharedAdminManager _admin = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly ActionBlockerSystem _actionBlocker = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<CustomExamineComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(Entity<CustomExamineComponent> ent, ref ExaminedEvent args)
    {
        CheckExpirations(ent);
        if (ent.Comp.PublicData.Content is null && ent.Comp.SubtleData.Content is null)
            return;

        var publicData = ent.Comp.PublicData;
        var subtleData = ent.Comp.SubtleData;

        using (args.PushGroup(nameof(CustomExamineComponent), -1))
        {
            // Lots of code duplication, blegh.
            var allowNsfw = _consent.HasConsent(args.Examiner, NsfwDescConsent);
            bool hasPublic = publicData.Content is not null, hasSubtle = subtleData.Content is not null;

            bool publicConsentHidden = hasPublic && publicData.RequiresConsent && !allowNsfw,
                 subtleConsentHidden = hasSubtle && subtleData.RequiresConsent && !allowNsfw;

            // If subtle is shown, then public is guaranteed to also be shown - this is to avoid extra raycasts
            bool subtleRangeHidden = hasSubtle && !_examine.InRangeUnOccluded(args.Examiner, args.Examined, subtleData.VisibilityRange),
                 publicRangeHidden = hasPublic && (!hasSubtle || subtleRangeHidden) && !_examine.InRangeUnOccluded(args.Examiner, args.Examined, publicData.VisibilityRange);

            if (hasPublic && !publicConsentHidden && !publicRangeHidden)
                args.PushMarkup(publicData.Content!);

            if (hasSubtle && !subtleConsentHidden && !subtleRangeHidden)
                args.PushMarkup(subtleData.Content!);

            // If something is hidden due to consent preferences, add a note (but only if in range)
            if (hasPublic && !publicRangeHidden && publicConsentHidden || hasSubtle && !subtleRangeHidden && subtleConsentHidden)
                args.PushMarkup(Loc.GetString("custom-examine-nsfw-hidden"));
        }
    }

    protected bool CanChangeExamine(ICommonSession actor, EntityUid examinee)
    {
        return actor.AttachedEntity == examinee && _actionBlocker.CanConsciouslyPerformAction(examinee)
            || _admin.IsAdmin(actor);
    }

    private void CheckExpirations(Entity<CustomExamineComponent> ent)
    {
        bool Check(CustomExamineData data)
        {
            if (data.Content is null
                || data.ExpireTime.Ticks <= 0
                || data.ExpireTime > _timing.CurTime)
                return false;

            data.Content = null;
            return true;
        }

        // Note: using | (bitwise or) instead of || (logical or) because the former is not short-circuiting
        if (Check(ent.Comp.PublicData) | Check(ent.Comp.SubtleData))
            Dirty(ent);
    }

    protected void TrimData(ref CustomExamineData publicData, ref CustomExamineData subtleData)
    {
        TrimData(ref publicData);
        TrimData(ref subtleData);

        if (publicData.VisibilityRange < subtleData.VisibilityRange)
            publicData.VisibilityRange = subtleData.VisibilityRange;
    }

    protected void TrimData(ref CustomExamineData data)
    {
        if (data.Content is null)
            return;

        // Exclude forbidden markup. Unlike ss14's chat cleanup code, this should also remove nested markup.
        data.Content = BadMarkupRegex.Replace(data.Content, "<bad markup>").Trim();

        // Shitty way to preserve and ignore markup while trimming
        var markupLength = MarkupLength(data.Content);
        if (data.Content.Length > AbsolutelyMaxLength)
            data.Content = data.Content[..AbsolutelyMaxLength];
        if (data.Content.Length - markupLength > PublicMaxLength)
            data.Content = data.Content[..(PublicMaxLength - markupLength)];

        if (data.Content.Length == 0)
            data.Content = null;
    }

    protected int LengthWithoutMarkup(string text) => FormattedMessage.RemoveMarkupPermissive(text).Length;

    protected int MarkupLength(string text) => text.Length - LengthWithoutMarkup(text);
}
