using Content.Shared.Emag.Systems;
using Robust.Shared.Audio.Systems;

namespace Content.Shared.Silicons.Bots;

public abstract class SharedWeldbotSystem : EntitySystem
{
    [Dependency] protected readonly SharedAudioSystem Audio = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<WeldbotComponent, GotEmaggedEvent>(OnEmagged);
    }

    private void OnEmagged(EntityUid uid, WeldbotComponent comp, ref GotEmaggedEvent args)
    {
        Audio.PlayPredicted(comp.EmagSparkSound, uid, args.UserUid);

        comp.IsEmagged = true;
        args.Handled = true;
    }
}
