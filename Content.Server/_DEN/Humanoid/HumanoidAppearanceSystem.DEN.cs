using Content.Shared.Humanoid;
using Content.Shared.Humanoid.Markings;


namespace Content.Server.Humanoid;


public sealed partial class HumanoidAppearanceSystem
{
    public (bool? Animated, MarkingPrototype? Marking) GetOppositeAnimatedMarking(
        MarkingCategories category,
        string currentMarkingId,
        string necessarySuffix)
    {
        var animated = currentMarkingId.EndsWith(necessarySuffix);
        string oppositeMarkingId;

        if (animated)
            oppositeMarkingId = currentMarkingId[..^necessarySuffix.Length];
        else
            oppositeMarkingId = currentMarkingId + necessarySuffix;

        if (!_markingManager.MarkingsByCategory(category).TryGetValue(oppositeMarkingId, out var markingPrototype))
            return (null, null);

        return (animated, markingPrototype);
    }

    /// <summary>
    ///     Sets the marking ID of the humanoid in a category at an index in the category's list.
    /// </summary>
    /// <param name="uid">Humanoid mob's UID</param>
    /// <param name="category">Category of the marking</param>
    /// <param name="index">Index of the marking</param>
    /// <param name="markingId">The marking ID to use</param>
    /// <param name="animatedSuffix">The suffix to add on and set the current marking to.</param>
    /// <param name="humanoid">Humanoid component of the entity</param>
    public bool? SetAnimatedMarkingId(
        EntityUid uid,
        MarkingCategories category,
        int index,
        string markingId,
        string animatedSuffix,
        HumanoidAppearanceComponent? humanoid = null)
    {
        var (animated, opposite) = GetOppositeAnimatedMarking(category, markingId, animatedSuffix);

        if (opposite == null
            || index < 0
            || !_markingManager.MarkingsByCategory(category).TryGetValue(markingId, out var markingPrototype)
            || !Resolve(uid, ref humanoid)
            || !humanoid.MarkingSet.TryGetCategory(category, out var markings)
            || index >= markings.Count)
            return null;

        var marking = markingPrototype.AsMarking();
        var oppositeMarking = opposite.AsMarking();

        for (var i = 0; i < marking.MarkingColors.Count && i < markings[index].MarkingColors.Count; i++)
        {
            if (opposite.ColorIndexRedirect.Count > i)
            {
                var realIndex = opposite.ColorIndexRedirect[i];
                oppositeMarking.SetColor(realIndex, markings[index].MarkingColors[i]);
                continue;
            }

            oppositeMarking.SetColor(i, markings[index].MarkingColors[i]);
        }

        humanoid.MarkingSet.Replace(category, index, oppositeMarking);
        Dirty(uid, humanoid);
        return !animated;
    }
}
