using System.Linq;
using Content.Shared.DoAfter;

namespace Content.Shared.Tools.Systems;

public abstract partial class SharedToolSystem
{
    /// <summary>
    ///     Checks if an entity is in the middle of a DoAfter of this tool event.
    /// </summary>
    /// <typeparam name="T">The event to check for.</typeparam>
    /// <param name="ent">The entity performing the DoAfters.</param>
    /// <returns>Whether or not the entity is performing the toolEvent.</returns>
    public bool IsPerformingToolEvent<T>(Entity<DoAfterComponent?> ent) where T : DoAfterEvent
    {
        if (!Resolve(ent.Owner, ref ent.Comp))
            return false;

        if (!_doAfterSystem.TryGetDoAftersByEvent<ToolDoAfterEvent>(ent.Owner, out var doAfters))
            return false;

        foreach (var doAfter in doAfters)
        {
            var toolDoAfter = (ToolDoAfterEvent) doAfter.Args.Event;
            if (toolDoAfter is T)
                return true;
        }

        return false;
    }

    /// <summary>
    ///     Checks if an entity is in the middle of using ANY tool.
    ///     Using this is highly not recommended; use <see cref="IsPerformingToolEvent"/> to narrow in on
    ///     specific events.
    /// </summary>
    /// <param name="ent">The entity performing the DoAfters.</param>
    /// <param name="target">The target entity.</param>
    /// <returns>Whether an entity is using any tool at all on the target.</returns>
    public bool IsUsingAnyToolOnTarget(Entity<DoAfterComponent?> ent, EntityUid? target)
    {
        if (!Resolve(ent.Owner, ref ent.Comp))
            return false;

        if (!_doAfterSystem.TryGetDoAftersByEvent<ToolDoAfterEvent>(ent.Owner, out var doAfters))
            return false;

        if (doAfters.Count() == 0)
            return false;

        var targetedDoAfters = doAfters.Where(d => d.Args.Target == target);
        return targetedDoAfters.Count() > 0;
    }
}
