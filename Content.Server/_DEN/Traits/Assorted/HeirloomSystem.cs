using Content.Shared.Examine;
using Content.Shared.Traits.Assorted.Components;

namespace Content.Server.Traits.Assorted;

public sealed partial class HeirloomSystem
{
    private void OnExamined(Entity<HeirloomComponent> ent, ref ExaminedEvent args)
    {
        if (IsHeirloomOf(args.Examiner, args.Examined))
            args.PushMarkup(Loc.GetString("heirloom-component-own-heirloom"));
        else
            args.PushMarkup(Loc.GetString("heirloom-component-other-heirloom", ("ent", ent)));
    }

    /// <summary>
    /// Checks if a given entity is another entity's heirloom.
    /// </summary>
    /// <param name="haver">The potential owner of the heirloom.</param>
    /// <param name="heirloom">The potential heirloom entity.</param>
    /// <returns>Whether or not the given entity is the haver's heirloom.</returns>
    /// <remarks>
    /// This only checks the owner's component, because this is what is used to refresh the moodlet.
    /// </remarks>
    public bool IsHeirloomOf(Entity<HeirloomHaverComponent?> haver, EntityUid heirloom)
    {
        return Resolve(haver.Owner, ref haver.Comp, logMissing: false)
            && haver.Comp.Heirloom == heirloom;
    }
}
