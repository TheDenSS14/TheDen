using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Content.Shared.DoAfter;

public abstract partial class SharedDoAfterSystem
{
    /// <summary>
    ///     Attempt to get all DoAfters an entity is performing based on DoAfterEvent type.
    /// </summary>
    /// <typeparam name="T">The DoAfterEvent to search for.</typeparam>
    /// <param name="ent">The entity performing the DoAfters.</param>
    /// <param name="doAfters">The resulting DoAfters.</param>
    /// <returns>Whether or not the operation was successful.</returns>
    public bool TryGetDoAftersByEvent<T>(Entity<DoAfterComponent?> ent,
        [NotNullWhen(true)] out IEnumerable<DoAfter>? doAfters) where T : DoAfterEvent
    {
        doAfters = null;
        if (!Resolve(ent.Owner, ref ent.Comp))
            return false;

        doAfters = ent.Comp.DoAfters.Values.Where(d => d.Args.Event is T);
        return true;
    }
}
